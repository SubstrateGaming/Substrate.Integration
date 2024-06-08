using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.frame_support.traits.schedule;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Dispatch Time C# Wrapper
    /// </summary>
    public class DispatchTimeSharp
    {
        /// <summary>
        /// Dispatch Time Constructor
        /// </summary>
        /// <param name="enactment"></param>
        public DispatchTimeSharp(EnumDispatchTime dispatchTime)
        {
            DispatchTime = dispatchTime.Value;
            switch (dispatchTime.Value)
            {
                case DispatchTime.At:
                    BlockNumber = ((U32)dispatchTime.Value2).Value;
                    break;
                case DispatchTime.After:
                    BlockNumber = ((U32)dispatchTime.Value2).Value;
                    break;
            }
        }

        /// <summary>
        /// Dispatch Time
        /// </summary>
        public DispatchTime DispatchTime { get; }

        /// <summary>
        /// Block Number
        /// </summary>
        public uint? BlockNumber { get; }
    }
}