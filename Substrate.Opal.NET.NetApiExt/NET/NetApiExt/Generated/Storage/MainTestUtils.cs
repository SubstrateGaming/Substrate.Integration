//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Substrate.Opal.NET.NetApiExt.Generated.Storage
{
    
    
    /// <summary>
    /// >> TestUtilsStorage
    /// </summary>
    public sealed class TestUtilsStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> TestUtilsStorage Constructor
        /// </summary>
        public TestUtilsStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("TestUtils", "Enabled"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.Bool)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("TestUtils", "TestValue"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
        }
        
        /// <summary>
        /// >> EnabledParams
        /// </summary>
        public static string EnabledParams()
        {
            return RequestGenerator.GetStorage("TestUtils", "Enabled", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> EnabledDefault
        /// Default value as hex string
        /// </summary>
        public static string EnabledDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Enabled
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.Bool> Enabled(string blockhash, CancellationToken token)
        {
            string parameters = TestUtilsStorage.EnabledParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.Bool>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> TestValueParams
        /// </summary>
        public static string TestValueParams()
        {
            return RequestGenerator.GetStorage("TestUtils", "TestValue", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> TestValueDefault
        /// Default value as hex string
        /// </summary>
        public static string TestValueDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> TestValue
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> TestValue(string blockhash, CancellationToken token)
        {
            string parameters = TestUtilsStorage.TestValueParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> TestUtilsCalls
    /// </summary>
    public sealed class TestUtilsCalls
    {
        
        /// <summary>
        /// >> enable
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Enable()
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            return new Method(255, "TestUtils", 0, "enable", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_test_value
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetTestValue(Substrate.NetApi.Model.Types.Primitive.U32 value)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(value.Encode());
            return new Method(255, "TestUtils", 1, "set_test_value", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_test_value_and_rollback
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method SetTestValueAndRollback(Substrate.NetApi.Model.Types.Primitive.U32 value)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(value.Encode());
            return new Method(255, "TestUtils", 2, "set_test_value_and_rollback", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> inc_test_value
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method IncTestValue()
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            return new Method(255, "TestUtils", 3, "inc_test_value", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> just_take_fee
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method JustTakeFee()
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            return new Method(255, "TestUtils", 4, "just_take_fee", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> batch_all
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method BatchAll(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.Opal.NET.NetApiExt.Generated.Model.opal_runtime.EnumRuntimeCall> calls)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(calls.Encode());
            return new Method(255, "TestUtils", 5, "batch_all", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> TestUtilsConstants
    /// </summary>
    public sealed class TestUtilsConstants
    {
    }
    
    /// <summary>
    /// >> TestUtilsErrors
    /// </summary>
    public enum TestUtilsErrors
    {
        
        /// <summary>
        /// >> TestPalletDisabled
        /// </summary>
        TestPalletDisabled,
        
        /// <summary>
        /// >> TriggerRollback
        /// </summary>
        TriggerRollback,
    }
}