using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPipeLine
{
    public class FakeHttpContext { }
    public delegate Task RequestDelegate(FakeHttpContext context);
    class Program
    {
        private static List<Func<RequestDelegate,RequestDelegate>> _pipes { get; set; } = new List<Func<RequestDelegate, RequestDelegate>>();

        static void Use(Func<RequestDelegate,RequestDelegate> func)
        {
            _pipes.Add(func);
        }
        static void Main(string[] args)
        {
           
            Use(next =>
            {
                return (context) =>
                {
                    Console.WriteLine("111");
                    return next(context);
                };
            });

            Use(next =>
            {
                return (context) =>
                {
                    Console.WriteLine("222");
                    return next(context);
                };
            });

            RequestDelegate end = context =>
            {
                Console.WriteLine("end..");
                return Task.CompletedTask;
            };
            foreach (var pipe in _pipes.AsEnumerable().Reverse())
            {
                end = pipe.Invoke(end);
            }

            end.Invoke(new FakeHttpContext());


        }

    }
}
