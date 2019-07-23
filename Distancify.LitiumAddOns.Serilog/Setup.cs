using Litium.Owin.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Distancify.LitiumAddOns.Serilog
{
    public class Setup : IPreSetupTask
    {
        public void PreSetup(IEnumerable<Assembly> assemblies)
        {
            Litium.Owin.Logging.Log.InitializeWith<SerilogLogger>();
        }
    }
}
