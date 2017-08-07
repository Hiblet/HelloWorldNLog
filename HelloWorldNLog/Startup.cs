using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

using NZ01;

namespace HelloWorldNLog
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IApplicationActions, ApplicationActions>(); // Relevant only to hooking up start/stop events.

            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            IServiceProvider serviceProvider,
            IApplicationLifetime appLifetime)
        {
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            //////////////////////////////////////////////////////////////////////////////////////////////
            // Add logging middleware: This is for recording the http request and response in log files.
            app.UseMiddleware<LogRequestAndResponseMiddleware>();

            //////////////////////////////////////////////////////////////////////////////////
            // Hook the Start and Stop events to allow actions to be performed, if required.
            // Note that the Register functions return cancellation tokens, but these are not yet used.
            // The idea is that we can stop the log thread when the application stops, gracefully.
            // MVC framework does not yet have this working.
            // I would use these to maybe fire off an email when the app starts/stops, so I 
            // am informed if it carks.

            //IApplicationActions appActions = serviceProvider.GetService<IApplicationActions>(); // DI method, retrieve your DI managed instance
            //var ctokenStarted = appLifetime.ApplicationStarted.Register(appActions.Started);
            //var ctokenStopping = appLifetime.ApplicationStopping.Register(appActions.Stopping);
            //var ctokenStopped = appLifetime.ApplicationStopped.Register(appActions.Stopped);
            

            app.UseDeveloperExceptionPage(); 
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "controller-action", template: "{controller}/{action}"); // Exercise: localhost:55109/HelloWorld/Index

                routes.MapRoute(name: "default", template: "{controller=HelloWorld}/{action=Index}"); // Exercise: localhost:55109
            });
        }
    }
}
