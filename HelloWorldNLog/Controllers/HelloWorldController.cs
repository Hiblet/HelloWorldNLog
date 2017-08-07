using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using HelloWorldCore.Models;
using HelloWorldCore.ViewModels;
using Microsoft.Extensions.Logging;



namespace HelloWorldCore.Controllers
{
    public class HelloWorldController : Controller
    {
        private readonly ILogger _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger) { _logger = logger; }

        public ViewResult Index()
        {
            string prefix = "Index() - ";
            _logger.LogInformation(prefix + "Entering");

            dumpDiagnostics(prefix, "BEFORE");

            int i = 0;
            int loopLimit = 10000;
            for (; i < loopLimit; ++i)
            {
                string wouldLogThis = prefix + $"Writing element [{i}]";

                // Using the MS simplified wrapper:
                // _logger.LogInformation(prefix + wouldLogThis);
                // _logger.LogTrace(prefix + wouldLogThis);

                // Using the MS primitive Log function, with custom formatter or lambda function
                //_logger.Log<string>(LogLevel.Debug, 0, wouldLogThis, null, ((x,ex) => x.ToString() + " " + ex.ToString()));
                //_logger.Log<string>(LogLevel.Information, 0, wouldLogThis, null, NZ01.Log4NetLogger.MyFormatter);

                // Using super fast direct logging (1000 times faster than through MS Facade)                
                NZ01.NLogAsyncLog.Trace(prefix + wouldLogThis);
            }

            dumpDiagnostics(prefix, "AFTER");


            // Example log messages to test ability to change log level in running
            _logger.LogDebug(prefix + "MS LogDebug Call would log here...");
            _logger.LogInformation(prefix + "MS LogInformation Call would log here...");
            _logger.LogTrace(prefix + "MS LogTrace Call would log here...");
            _logger.LogWarning(prefix + "MS LogWarning Call would log here...");
            _logger.LogError(prefix + "MS LogError Call would log here...");
            _logger.LogCritical(prefix + "MS LogCritical Call would log here...");

            NZ01.NLogAsyncLog.Trace(prefix + "Direct Trace Call would log here...");
            NZ01.NLogAsyncLog.Debug(prefix + "Direct Debug Call would log here...");
            NZ01.NLogAsyncLog.Info(prefix + "Direct Info Call would log here...");
            NZ01.NLogAsyncLog.Warn(prefix + "Direct Warn Call would log here...");
            NZ01.NLogAsyncLog.Error(prefix + "Direct Error Call would log here...");
            NZ01.NLogAsyncLog.Fatal(prefix + "Direct Fatal Call would log here...");


            return View(new HelloWorldViewModel { Message = (new HelloWorld()).Timestamp });
        }



        private void dumpDiagnostics(string prefix, string text)
        {
            if (string.IsNullOrEmpty(prefix))
                prefix = "dumpDiagnostics() - "; // Override

            if (!string.IsNullOrWhiteSpace(text))
                prefix += $"[{text}] - ";

            string msg = "";

            msg = "NLogProvider: ";
            msg += $"InstanceCtor[{NZ01.NLogProvider.CountCalls_InstanceCtor}], ";
            msg += $"CreateLogger[{NZ01.NLogProvider.CountCalls_CreateLogger}], ";
            msg += $"Dispose[{NZ01.NLogProvider.CountCalls_Dispose}], ";
            _logger.LogInformation(prefix + msg);

            msg = "NLogLogger: ";
            msg += $"InstanceCtor[{NZ01.NLogLogger.CountCalls_InstanceCtor}], ";
            msg += $"BeginScope[{NZ01.NLogLogger.CountCalls_BeginScope}], ";
            msg += $"IsEnabled[{NZ01.NLogLogger.CountCalls_IsEnabled}], ";
            msg += $"Log[{NZ01.NLogLogger.CountCalls_Log}]";
            _logger.LogInformation(prefix + msg);

            msg = "NLogExtensions: ";
            msg += $"AddNLog[{NZ01.NLogExtensions.CountCalls_AddNLog}], ";
            _logger.LogInformation(prefix + msg);
        }

    }
}
