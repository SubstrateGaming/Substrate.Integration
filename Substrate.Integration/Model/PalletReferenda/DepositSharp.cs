using Substrate.Integration.Helper;
using Substrate.NetApi.Model.Types;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_referenda.types;
using System.Numerics;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Deposit C# Wrapper
    /// </summary>
    public class DepositSharp
    {
        /// <summary>
        /// Deposit Constructor
        /// </summary>
        /// <param name="deposit"></param>
        public DepositSharp(Deposit deposit)
        {
            Who = deposit.Who.ToAddress(0);
            Amount = deposit.Amount.Value;
        }

        /// <summary>
        /// Who
        /// </summary>
        public string Who { get; }

        /// <summary>
        /// Amount
        /// </summary>
        public BigInteger Amount { get; }
    }
}