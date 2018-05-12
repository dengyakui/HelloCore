using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HelloCore
{
    public class Conf
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class Startup
    {
        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Conf>(_configuration);
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
 
            Task Handler(HttpContext context) => context.Response.WriteAsync("this is an action");
            var route = new Route(new RouteHandler(Handler),"action",app.ApplicationServices.GetRequiredService<IInlineConstraintResolver>() );
            //app.UseRouter(builder => { builder.MapGet("action", context => context.Response.WriteAsync("this is a action")); });
            app.UseRouter(route);
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("Started");
            });

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("Stopping...");
            });


            applicationLifetime.ApplicationStopped.Register(() =>
            {
                Console.WriteLine("Stoped");
            });

            // first pipe

            app.Use(next => { return context =>
            {
                context.Response.WriteAsync("first pipe");
                return next(context);
            }; });

            //second pipe
            app.Use((context, next) =>
            {
                context.Response.WriteAsync("second pipe");
                return next.Invoke();
            });

            app.Run(context => context.Response.WriteAsync("final pipe"));
        }
    }
}
