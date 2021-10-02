using Maximus.Library.ManagedModuleBase;
using Maximus.Library.SCOMId;
using Microsoft.EnterpriseManagement.HealthService;
using Microsoft.EnterpriseManagement.Modules.DataItems.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Maximus.IncDecDiscovery.Modules
{
  [MonitoringModule(ModuleType.ReadAction)]
  [ModuleOutput(true)]
  class vCenterSnapshotDiscoveryPA : ModuleBaseSimpleAction<DiscoveryDataItem>
  {
    // configuration
    private Guid sourceId, managedEntityId;

    public vCenterSnapshotDiscoveryPA(ModuleHost<DiscoveryDataItem> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
    {
    }

    protected override void PreInitialize(ModuleHost<DiscoveryDataItem> moduleHost, XmlReader configuration, byte[] previousState)
    {
      base.PreInitialize(moduleHost, configuration, previousState);
    }

    protected override DiscoveryDataItem[] GetOutputData(DataItemBase[] inputDataItems)
    {
      try
      {
        return new DiscoveryDataItem[]
        {
          new DiscoveryDataItem(DateTime.UtcNow, DiscoveryType.Snapshot, DiscoverySourceType.Rule, sourceId, managedEntityId,
          new List<ClassInstance>
          {
            new ClassInstance(Ids.vCenterEmulatorClassId, new List<Property>
            {
              NewProperty(Ids.vCenterEmulatorClassProperties.ObjectKeyPropertyId, "vc-1"),
              NewProperty(SystemId.EntityClassProperties.DisplayNamePropertyId, "vCenter 01")
            }.AsReadOnly()),
            new ClassInstance(Ids.vCenterEmulatorClassId, new List<Property>
            {
              NewProperty(Ids.vCenterEmulatorClassProperties.ObjectKeyPropertyId, "vc-2"),
              NewProperty(SystemId.EntityClassProperties.DisplayNamePropertyId, "vCenter 02")
            }.AsReadOnly())
          }.AsReadOnly(),
          new List<RelationshipInstance>().AsReadOnly())
        };
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
         */
        // load
        LoadConfigurationElement(cfgDoc, "sourceId", out sourceId);
        LoadConfigurationElement(cfgDoc, "managedEntityId", out managedEntityId);
      }
      catch(Exception xe)
      {
        throw new ModuleException("Error parsing configuration XML", xe);
      }
    }
  }
}
