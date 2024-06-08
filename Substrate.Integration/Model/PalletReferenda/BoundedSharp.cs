using Substrate.Integration.Helper;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.frame_support.traits.preimages;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.primitive_types;

namespace Substrate.Integration.Model.PalletReferenda
{
    /// <summary>
    /// Bounded C# Wrapper
    /// </summary>
    public class BoundedSharp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounded"></param>
        public BoundedSharp(EnumBounded bounded)
        {
            Bounded = bounded.Value;
            switch (bounded.Value)
            {
                case Bounded.Legacy:
                    Hash = ((H256)bounded.Value2).ToHexString();
                    break;

                case Bounded.Inline:
                    Bytes = ((BoundedVecT13)bounded.Value2).Value.ToBytes();
                    break;

                case Bounded.Lookup:
                    var tuple = (BaseTuple<H256, U32>)bounded.Value2;
                    Hash = ((H256)tuple.Value[0]).ToHexString();
                    Number = ((U32)tuple.Value[1]).Value;
                    break;
            }
        }

        /// <summary>
        /// Bounded
        /// </summary>
        public Bounded Bounded { get; }

        /// <summary>
        /// Hash
        /// </summary>
        public string? Hash { get; }

        /// <summary>
        /// Number
        /// </summary>
        public uint? Number { get; }

        /// <summary>
        /// Bytes
        /// </summary>
        public byte[]? Bytes { get; }
    }
}