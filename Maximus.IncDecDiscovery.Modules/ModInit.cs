using Maximus.Library.Helpers;
using Maximus.Library.ManagedModuleBase;
using Microsoft.EnterpriseManagement.HealthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maximus.IncDecDiscovery.Modules
{
  internal static class ModInit
  {
    internal static char[] Separators = { ',', ';', '|' };

    const string LogSourceName = "Maximus Incremental and Decremental Discovery Module";
    const int LogBaseEventId = 2720;

    static private LoggingHelper _Logger;
    static internal LoggingHelper Logger => _Logger ?? (_Logger = new LoggingHelper(LogSourceName, LogBaseEventId, EventLoggingLevel.Informational));

    internal const int evtId_VMSelfRemoveDiscoveryPA = 0;

    internal static void ModuleErrorSignalReceiver(ModuleErrorSeverity severity, ModuleErrorCriticality criticality, Exception e, string message, object callerInstance)
    {
      Logger.WriteException($"Internal module exception or error.\r\nMessage: {message}\r\nError Severity: {severity}\r\nError Criticality: {criticality}", e ?? new Exception("Unknown exception"), callerInstance);
    }
  }
}
