using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace NZ01
{
    public interface IApplicationActions
    {
        void Started();
        void Stopping();
        void Stopped();
    }

    public class ApplicationActions : IApplicationActions
    {
        private readonly ILogger _logger;

        public ApplicationActions(ILoggerFactory loggerFactory)
        {
            var prefix = "ApplicationActions() [INSTANCE CTOR] - ";
            _logger = loggerFactory.CreateLogger<ApplicationActions>();
            _logger.LogDebug(prefix + "Entering");
        }

        ~ApplicationActions()
        {
            var prefix = "~ApplicationActions() [INSTANCE DTOR] - ";
            _logger.LogDebug(prefix + "Entering");
        }

        public void Started()
        {
            var prefix = "Started() - ";
            _logger.LogWarning(prefix + "Application started.");

            // Do things on app start
        }

        public void Stopping()
        {
            var prefix = "Stopping() - ";
            _logger.LogInformation(prefix + "Application stopping (finishing pending Html requests)...");

        }

        public void Stopped()
        {
            var prefix = "Stopped() - ";
            _logger.LogWarning(prefix + "Application stopped.");

            _logger.LogInformation(prefix + "Application stopped.");
        }
    }
}
