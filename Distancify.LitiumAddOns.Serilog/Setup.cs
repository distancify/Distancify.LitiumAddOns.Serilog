using Litium.Owin.Lifecycle;
using System.Collections.Generic;
using System.Reflection;

namespace Distancify.LitiumAddOns.Serilog
{
    public class Setup : IPreSetupTask
    {
        public void PreSetup(IEnumerable<Assembly> assemblies)
        {
            Litium.Owin.Logging.Log.Factory = (name) =>
            {
                var logger = new SerilogLogger();
                logger.InitializeFor(name);

                return logger;
            };
        }
    }
}
