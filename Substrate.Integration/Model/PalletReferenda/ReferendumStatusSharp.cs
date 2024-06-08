using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_conviction_voting.types;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_referenda.types;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_treasury;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.pallet_xcm.pallet;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Referendum Status C# Wrapper
    /// </summary>
    public class ReferendumStatusSharp
    {
        /// <summary>
        /// Referendum Status Constructor
        /// </summary>
        /// <param name="referendumStatus"></param>
        public ReferendumStatusSharp(ReferendumStatus referendumStatus)
        {
            Track = referendumStatus.Track.Value;
            OriginSharp = new OriginSharp(referendumStatus.Origin);
            Proposal = new BoundedSharp(referendumStatus.Proposal);
            Enactment = new DispatchTimeSharp(referendumStatus.Enactment);
            Submitted = referendumStatus.Submitted.Value;
            SubmissionDeposit = new DepositSharp(referendumStatus.SubmissionDeposit);
            DecisionDeposit = referendumStatus.DecisionDeposit.OptionFlag ? new DepositSharp(referendumStatus.DecisionDeposit.Value) : null;
            Deciding = referendumStatus.Deciding.OptionFlag ? new DecidingStatusSharp(referendumStatus.Deciding.Value) : null;
            Tally = new TallySharp(referendumStatus.Tally);
            InQueue = referendumStatus.InQueue.Value;
        }

        /// <summary>
        /// Track
        /// </summary>
        public ushort Track { get; }

        /// <summary>
        /// Origin Sharp
        /// </summary>
        public OriginSharp OriginSharp { get; }

        /// <summary>
        /// Proposal
        /// </summary>
        public BoundedSharp Proposal { get; }

        /// <summary>
        /// Enactment
        /// </summary>
        public DispatchTimeSharp Enactment { get; }

        /// <summary>
        /// Submitted
        /// </summary>
        public uint Submitted { get; }

        /// <summary>
        /// Submission Deposit
        /// </summary>
        public DepositSharp SubmissionDeposit { get; }

        /// <summary>
        /// Decision Deposit
        /// </summary>
        public DepositSharp? DecisionDeposit { get; }

        /// <summary>
        /// Deciding
        /// </summary>
        public DecidingStatusSharp? Deciding { get; }

        /// <summary>
        /// Tally
        /// </summary>
        public TallySharp Tally { get; }

        /// <summary>
        /// In Queue
        /// </summary>
        public bool InQueue { get; }
    }
}