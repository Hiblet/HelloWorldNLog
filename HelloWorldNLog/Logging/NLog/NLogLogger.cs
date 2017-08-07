using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging; // ILogger
using System.Text;

namespace NZ01
{
    public class NLogLogger : ILogger
    {
        private string _name;

        public static int CountCalls_InstanceCtor { get; set; } = 0;
        public static int CountCalls_BeginScope { get; set; } = 0;
        public static int CountCalls_IsEnabled { get; set; } = 0;
        public static int CountCalls_Log { get; set; } = 0;

        public NLogLogger(string name)
        {
            ++CountCalls_InstanceCtor;
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state) { ++CountCalls_BeginScope; return null; }

        public bool IsEnabled(LogLevel logLevel)
        {
            ++CountCalls_IsEnabled;
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return NLogAsyncLog.IsFatalEnabled;
                case LogLevel.Trace:
                    return NLogAsyncLog.IsTraceEnabled;
                case LogLevel.Debug:
                    return NLogAsyncLog.IsDebugEnabled;
                case LogLevel.Error:
                    return NLogAsyncLog.IsErrorEnabled;
                case LogLevel.Information:
                    return NLogAsyncLog.IsInfoEnabled;
                case LogLevel.Warning:
                    return NLogAsyncLog.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public static string MyFormatter<TState, Exception>(TState t, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(t?.ToString());
            sb.Append(" ");
            if (ex != null)
            {
                sb.Append(ex.ToString());
            }

            return sb.ToString();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ++CountCalls_Log;

            if (!IsEnabled(logLevel)) { return; }

            if (formatter == null) { throw new ArgumentNullException(nameof(formatter)); }

            if (state != null || exception != null)
            {
                NLogAsyncLog.Enqueue(logLevel, eventId, (object)state, exception, formatter, state.GetType(), _name);
            }
        }
    }
}

