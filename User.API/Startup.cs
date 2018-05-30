using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Models;
using UserContext = User.API.Data.UserContext;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(options => { options.UseMySQL(Configuration.GetConnectionString("MysqlUser")); });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            InitUserData(app);
        }

        public void InitUserData(IApplicationBuilder app)
        {
            Thread.Sleep(5000);
            Console.WriteLine("starting....");
            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
                    userContext.Database.Migrate();
                    if (!userContext.Users.Any())
                    {
                        userContext.Users.Add(new AppUser()
                        {
                            Name = "jesse"
                        });
                        userContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("will retry after 3 seconds");
                Thread.Sleep(3000);
                InitUserData(app);
            }

        }
    }
}
