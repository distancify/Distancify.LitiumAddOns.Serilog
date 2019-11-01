using Litium.Owin.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace Distancify.LitiumAddOns.Serilog
{
    public class SerilogLogger : LoggerBase, ILog
    {
        private static readonly ISet<string> VerboseSources = new HashSet<string>
        {
            "System.Web.Http.Tracing.ITraceWriter",
            "Microsoft.EntityFrameworkCore.ChangeTracking",
            "Microsoft.EntityFrameworkCore.Database.Command",
            "Microsoft.EntityFrameworkCore.Database.Connection",
            "Microsoft.EntityFrameworkCore.Database.Transaction",
            "Microsoft.EntityFrameworkCore.Infrastructure",
            "Microsoft.EntityFrameworkCore.Model",
            "Microsoft.EntityFrameworkCore.Model.Validation",
            "Microsoft.EntityFrameworkCore.Query",
            "Microsoft.EntityFrameworkCore.Update",
            "Litium.Web.Media.Storage.StorageRequestContext"
        };

        public override bool IsFatalEnabled => global::Serilog.Log.Logger.IsEnabled(global::Serilog.Events.LogEventLevel.Fatal);

        public override bool IsErrorEnabled => global::Serilog.Log.Logger.IsEnabled(global::Serilog.Events.LogEventLevel.Error);

        public override bool IsWarnEnabled => global::Serilog.Log.Logger.IsEnabled(global::Serilog.Events.LogEventLevel.Warning);

        public override bool IsInfoEnabled => global::Serilog.Log.Logger.IsEnabled(global::Serilog.Events.LogEventLevel.Information);

        public override bool IsDebugEnabled => global::Serilog.Log.Logger.IsEnabled(global::Serilog.Events.LogEventLevel.Debug);

        public override bool IsTraceEnabled => global::Serilog.Log.Logger.IsEnabled(global::Serilog.Events.LogEventLevel.Verbose);

        private ILogger For(string loggerName)
        {
            return global::Serilog.Log.Logger
                .ForContext("FromLitium", true)
                .ForContext("SourceContext", loggerName);
        }

        protected override void Log(Level level, string title, Func<string> message, Exception exception)
        {
            var renderedMessage = message();

            if (renderedMessage.Contains("FromLitium"))
            {
                return;
            }

            level = PostProcessLevel(level, title, renderedMessage);

            switch (level)
            {
                case Level.Trace:
                    For(title).Verbose(exception, renderedMessage);
                    break;
                case Level.Debug:
                    For(title).Debug(exception, renderedMessage);
                    break;
                case Level.Info:
                    For(title).Information(exception, renderedMessage);
                    break;
                case Level.Warn:
                    For(title).Warning(exception, renderedMessage);
                    break;
                case Level.Error:
                    For(title).Error(exception, renderedMessage);
                    break;
                case Level.Fatal:
                    For(title).Fatal(exception, renderedMessage);
                    break;
            }
        }

        protected override void Log(Level level, string title, string message, params object[] formatting)
        {
            if (message.Contains("FromLitium"))
            {
                return;
            }

            level = PostProcessLevel(level, title, message);

            switch (level)
            {
                case Level.Trace:
                    For(title).Verbose(message, formatting);
                    break;
                case Level.Debug:
                    For(title).Debug(message, formatting);
                    break;
                case Level.Info:
                    For(title).Information(message, formatting);
                    break;
                case Level.Warn:
                    For(title).Warning(message, formatting);
                    break;
                case Level.Error:
                    For(title).Error(message, formatting);
                    break;
                case Level.Fatal:
                    For(title).Fatal(message, formatting);
                    break;
            }
        }

        private Level PostProcessLevel(Level level, string title, string message)
        {
            if (VerboseSources.Contains(title))
                return Level.Trace;

            if (message.Contains("A potentially dangerous Request.Path value was detected from the client"))
                return Level.Warn;

            return level;
        }
    }
}
