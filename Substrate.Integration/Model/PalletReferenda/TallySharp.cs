using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_conviction_voting.types;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.sp_npos_elections;
using System.Numerics;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Tally C# Wrapper
    /// </summary>
    public class TallySharp
    {
        /// <summary>
        /// Tally Constructor
        /// </summary>
        /// <param name="tally"></param>
        public TallySharp(Tally tally)
        {
            Ayes = tally.Ayes.Value;
            Nays = tally.Nays.Value;
            Support = tally.Support.Value;
        }

        /// <summary>
        /// Ayes
        /// </summary>
        public BigInteger Ayes { get; }

        /// <summary>
        /// Nays
        /// </summary>
        public BigInteger Nays { get; }

        /// <summary>
        /// Support
        /// </summary>
        public BigInteger Support { get; }
    }
}