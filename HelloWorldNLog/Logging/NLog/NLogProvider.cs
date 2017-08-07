using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging; // ILoggerProvider 
using System.Xml; // XmlElement
using System.IO; // File
using System.Collections.Concurrent;


namespace NZ01
{
    public class NLogProvider : ILoggerProvider
    {
        private Dictionary<string, NLogLogger> _registry = new Dictionary<string, NLogLogger>();
        private bool _bRunning = true;

        // DIAGS
        public static int CountCalls_InstanceCtor { get; set; } = 0;
        public static int CountCalls_CreateLogger { get; set; } = 0;
        public static int CountCalls_Dispose { get; set; } = 0;


        // Ctor
        public NLogProvider(string name)
        {
            ++CountCalls_InstanceCtor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            ++CountCalls_CreateLogger;

            if (!_bRunning)
                return null;

            NLogLogger logger = null;
            if (!_registry.TryGetValue(categoryName, out logger))
                logger = new NLogLogger(categoryName);

            return logger;
        }

        public void Dispose()
        {
            ++CountCalls_Dispose;
            _bRunning = false;
            _registry.Clear();
        }
    }    
}
