//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_xcm.pallet
{
    
    
    /// <summary>
    /// >> VersionMigrationStage
    /// </summary>
    public enum VersionMigrationStage
    {
        
        /// <summary>
        /// >> MigrateSupportedVersion
        /// </summary>
        MigrateSupportedVersion = 0,
        
        /// <summary>
        /// >> MigrateVersionNotifiers
        /// </summary>
        MigrateVersionNotifiers = 1,
        
        /// <summary>
        /// >> NotifyCurrentTargets
        /// </summary>
        NotifyCurrentTargets = 2,
        
        /// <summary>
        /// >> MigrateAndNotifyOldTargets
        /// </summary>
        MigrateAndNotifyOldTargets = 3,
    }
    
    /// <summary>
    /// >> 565 - Variant[pallet_xcm.pallet.VersionMigrationStage]
    /// </summary>
    public sealed class EnumVersionMigrationStage : BaseEnumExt<VersionMigrationStage, BaseVoid, BaseVoid, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>, BaseVoid>
    {
    }
}
