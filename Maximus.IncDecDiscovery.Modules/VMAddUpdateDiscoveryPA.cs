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
  class VMAddUpdateDiscoveryPA : ModuleBaseSimpleAction<DiscoveryDataItem>
  {
    // configuration
    private Guid sourceId, managedEntityId;
    private string VCId;

    private int maxBatchSize = 3;
    private Queue<string> currentSnapshot = new Queue<string>();

    public VMAddUpdateDiscoveryPA(ModuleHost<DiscoveryDataItem> moduleHost, XmlReader configuration, byte[] previousState) : base(moduleHost, configuration, previousState)
    {
    }

    protected override DiscoveryDataItem[] GetOutputData(DataItemBase[] inputDataItems)
    {
      try
      {
        if (currentSnapshot.Count == 0)
        {
          foreach (string item in File.ReadAllLines($"C:\\Temp\\vmEmul_{VCId}.txt"))
            currentSnapshot.Enqueue(item);
        }
        List<ClassInstance> discoveredVMs = new List<ClassInstance>(maxBatchSize);
        for (int batchCounter = maxBatchSize; batchCounter > 0; batchCounter--)
        {
          if (currentSnapshot.Count > 0)
          {
            string vmId = currentSnapshot.Dequeue();
            discoveredVMs.Add(new ClassInstance(Ids.VirtualMachineEmulatorClassId, new List<Property>
            {
              NewProperty(Ids.vCenterEmulatorClassProperties.ObjectKeyPropertyId, VCId), // host
              NewProperty(Ids.VirtualMachineEmulatorClassProperties.VMKeyPropertyId, vmId) // key
            }.AsReadOnly()));
          }
          else
            break;
        }
        if (discoveredVMs.Count > 0)
          return new DiscoveryDataItem[]
          {
            new DiscoveryDataItem(DateTime.UtcNow, DiscoveryType.AddUpdate, DiscoverySourceType.Rule, sourceId, managedEntityId, 
            discoveredVMs.AsReadOnly(), 
            new List<RelationshipInstance>().AsReadOnly())
          };
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
         */
        // load
        LoadConfigurationElement(cfgDoc, "sourceId", out sourceId);
        LoadConfigurationElement(cfgDoc, "managedEntityId", out managedEntityId);
        LoadConfigurationElement(cfgDoc, "VCId", out VCId);
      }
      catch (Exception xe)
      {
        throw new ModuleException("Error parsing configuration XML", xe);
      }
    }
  }
}
