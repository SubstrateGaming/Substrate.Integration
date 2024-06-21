using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_referenda.types;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Deciding Status C# Wrapper
    /// </summary>
    public class DecidingStatusSharp
    {
        /// <summary>
        /// Deciding Status Constructor
        /// </summary>
        /// <param name="value"></param>
        public DecidingStatusSharp(DecidingStatus decidingStatus)
        {
            Since = decidingStatus.Since.Value;
            Confirming = decidingStatus.Confirming.OptionFlag ? decidingStatus.Confirming.Value.Value : (uint?)null;
        }

        /// <summary>
        /// Since
        /// </summary>
        public uint Since { get; }
        
        /// <summary>
        /// Confirming
        /// </summary>
        public uint? Confirming { get; }
    }
}