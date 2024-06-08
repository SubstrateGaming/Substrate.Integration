using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_referenda.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Referendum Info C# Wrapper
    /// </summary>
    public class ReferendumInfoSharp
    {
        /// <summary>
        /// Referendum Info Constructor
        /// </summary>
        /// <param name="enumReferendumInfo"></param>
        public ReferendumInfoSharp(EnumReferendumInfo enumReferendumInfo)
        {
            ReferendumInfo = enumReferendumInfo.Value;
            switch (enumReferendumInfo.Value)
            {
                case ReferendumInfo.Ongoing:
                    ReferendumStatus = new ReferendumStatusSharp((ReferendumStatus)enumReferendumInfo.Value2);
                    break;

                case ReferendumInfo.Killed:
                    Moment = (U32)enumReferendumInfo.Value2;
                    break;
                case ReferendumInfo.Approved:
                case ReferendumInfo.Rejected:
                case ReferendumInfo.Cancelled:
                case ReferendumInfo.TimedOut:
                    BaseTuple<U32, BaseOpt<Deposit>, BaseOpt<Deposit>> refInfo = (BaseTuple<U32, BaseOpt<Deposit>, BaseOpt<Deposit>>)enumReferendumInfo.Value2;
                    Moment = (U32)refInfo.Value[0];
                    var depositOpt1 = (BaseOpt<Deposit>)refInfo.Value[1];
                    SubmissionDeposit = depositOpt1.OptionFlag ? new DepositSharp(depositOpt1.Value) : null;
                    var depositOpt2 = (BaseOpt<Deposit>)refInfo.Value[2];
                    DecisionDeposit = depositOpt2.OptionFlag ? new DepositSharp(depositOpt2.Value) : null;
                    break;
            }
        }

        /// <summary>
        /// Referendum Info
        /// </summary>
        public ReferendumInfo ReferendumInfo { get; }

        /// <summary>
        /// Referendum Status
        /// </summary>
        public ReferendumStatusSharp? ReferendumStatus { get; }

        /// <summary>
        /// Moment
        /// </summary>
        public U32? Moment { get; }

        /// <summary>
        /// Deposit 1
        /// </summary>
        public DepositSharp? SubmissionDeposit { get; }

        /// <summary>
        /// Deposit 2
        /// </summary>
        public DepositSharp? DecisionDeposit { get; }
    }
}
