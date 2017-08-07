using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace NZ01
{
    public static class NLogExtensions
    {
        public static int CountCalls_AddNLog { get; set; } = 0;

        public static ILoggerFactory AddNLog(this ILoggerFactory factory)
        {
            ++CountCalls_AddNLog;
            factory.AddProvider(new NLogProvider("App"));
            return factory;
        }
    }
}
