using Maximus.Library.ManagedModuleBase;
using Microsoft.EnterpriseManagement.HealthService;
using Microsoft.EnterpriseManagement.Modules.DataItems.Discovery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Maximus.IncDecDiscovery.Modules
{
  [MonitoringModule(ModuleType.ReadAction)]
  [ModuleOutput(true)]
  class VMSelfRemoveDiscoveryPA : ModuleBaseSimpleAction<DiscoveryDataItem>
  {
    // configuration
    private Guid sourceId, managedEntityId;
    private string VMId, VCId;

    public VMSelfRemoveDiscoveryPA(ModuleHost<DiscoveryDataItem> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
    {
    }

    protected override void PreInitialize(ModuleHost<DiscoveryDataItem> moduleHost, XmlReader configuration, byte[] previousState)
    {
      ModInit.Logger.AddLoggingSource(GetType(), ModInit.evtId_VMSelfRemoveDiscoveryPA);
      base.PreInitialize(moduleHost, configuration, previousState);
    }

    protected override DiscoveryDataItem[] GetOutputData(DataItemBase[] inputDataItems)
    {
      try
      {
        string[] allVMIds = File.ReadAllLines($"C:\\Temp\\vmEmul_{VCId}.txt");
        if (!allVMIds.Contains(VMId))
        {
          ModInit.Logger.WriteInformation($"Deleting {VMId}", this);
          return new DiscoveryDataItem[]
          {
            new DiscoveryDataItem(DateTime.UtcNow, DiscoveryType.Remove, DiscoverySourceType.Rule, sourceId, managedEntityId,
            new List<ClassInstance>
            {
              new ClassInstance(Ids.VirtualMachineEmulatorClassId, new List<Property>
              {
                NewProperty(Ids.vCenterEmulatorClassProperties.ObjectKeyPropertyId, VCId), // host
                NewProperty(Ids.VirtualMachineEmulatorClassProperties.VMKeyPropertyId, VMId) // key
              }.AsReadOnly())
            }.AsReadOnly(),
            new List<RelationshipInstance>().AsReadOnly())
          };
        }
        else
          return null;
      }
      catch
      {
        return null;
      }
    }

    protected override void LoadConfiguration(XmlDocument cfgDoc)
    {
      try
      {
        /*
         * <xsd:element minOccurs="1" name="sourceId" type="xsd:string" />
         * <xsd:element minOccurs="1" name="managedEntityId" type="xsd:string" />
         * <xsd:element minOccurs="1" name="VCId" type="xsd:string" />
         * <xsd:element minOccurs="1" name="VMId" type="xsd:string" />
         */
        // load
        LoadConfigurationElement(cfgDoc, "sourceId", out sourceId);
        LoadConfigurationElement(cfgDoc, "managedEntityId", out managedEntityId);
        LoadConfigurationElement(cfgDoc, "VCId", out VCId);
        LoadConfigurationElement(cfgDoc, "VMId", out VMId);
      }
      catch (Exception xe)
      {
        throw new ModuleException("Error parsing configuration XML", xe);
      }
    }
  }
}
