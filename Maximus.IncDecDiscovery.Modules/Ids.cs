using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maximus.IncDecDiscovery.Modules
{
  public class Ids
  {
    #region Maximus.IncDecDiscovery.vCenterEmulator
    /// <summary>
    ///  (Maximus.IncDecDiscovery.vCenterEmulator)
    /// </summary>
    public static Guid vCenterEmulatorClassId { get; } = new Guid("8a799a9c-47f7-3d9c-f400-0fc395f317d7");
    public static class vCenterEmulatorClassProperties
    {
      /// <summary>
      /// Maximus.IncDecDiscovery.vCenterEmulator/ObjectKey (Key)
      /// </summary>
      public static Guid ObjectKeyPropertyId { get; } = new Guid("d246dd31-eaf8-7eb6-261f-242195476796");
    }
    #endregion

    #region Maximus.IncDecDiscovery.VirtualMachineEmulator
    /// <summary>
    ///  (Maximus.IncDecDiscovery.VirtualMachineEmulator)
    /// </summary>
    public static Guid VirtualMachineEmulatorClassId { get; } = new Guid("1d351877-5ffd-215f-6831-8c1611de3a8e");
    public static class VirtualMachineEmulatorClassProperties
    {
      /// <summary>
      /// Maximus.IncDecDiscovery.VirtualMachineEmulator/VMKey (Key)
      /// </summary>
      public static Guid VMKeyPropertyId { get; } = new Guid("74d7be99-c637-e4a6-89ca-743acd97ce5b");
    }
    #endregion
  }
}
