using Autofac;
using Autofac.Extensions.DependencyInjection;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace PaySlipCalculator.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //gembox setting
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });


            // setup Dependency Injection with AutoFac
            var builder = new ContainerBuilder();
            var dataAndServiceAccess = Assembly.GetExecutingAssembly();
            var dataAndServiceAccessCore = Assembly.Load("PaySlipCalculator.Core");
            builder.RegisterAssemblyTypes(dataAndServiceAccessCore)
                .PublicOnly()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(dataAndServiceAccess)
                .PublicOnly()
                .AsImplementedInterfaces();

            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}