using Substrate.Polkadot.NET.NetApiExt.Generated.Model.frame_support.dispatch;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.polkadot_runtime;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Origin C# Wrapper
    /// </summary>
    public class OriginSharp
    {
        /// <summary>
        /// Origin Caller Constructor
        /// </summary>
        /// <param name="originCaller"></param>
        public OriginSharp(EnumOriginCaller originCaller)
        {
            OriginCaller = originCaller.Value;
            switch (originCaller.Value)
            {
                case OriginCaller.system:
                    // TODO
                    break;
                case OriginCaller.Origins:
                    // TODO
                    break;
                case OriginCaller.ParachainsOrigin:
                    // TODO
                    break;
                case OriginCaller.XcmPallet:
                    // TODO
                    break;
                case OriginCaller.Void:
                    // TODO
                    break;
            }
        }

        public OriginCaller OriginCaller { get; }
    }
}