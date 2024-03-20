using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using Serilog;
using Substrate.Integration;
using Substrate.Integration.Client;
using Substrate.Integration.Helper;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Opal.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.Opal.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.Opal.NET.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Substrate.Opal.NET.NetApiExt.Generated.Model.opal_runtime;
using Substrate.Opal.NET.NetApiExt.Generated.Model.up_data_structs;
using Substrate.Opal.NET.NetApiExt.Generated.Storage;
using System.Numerics;
using System.Security.Principal;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using Substrate.Opal.NET.NetApiExt.Generated.Model.primitive_types;

namespace Substrate.Console
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // configure serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo
                .Console()
                .CreateLogger();

            var config = new ConfigurationBuilder()
                // this will be used more later on
                .SetBasePath(AppContext.BaseDirectory)
                // I chose using YML files for my config data as I am familiar with them
                .AddYamlFile("config.yml")
                .Build();

            // Add this to your C# console app's Main method to give yourself
            // a CancellationToken that is canceled when the user hits Ctrl+C.
            var cts = new CancellationTokenSource();
            System.Console.CancelKeyPress += (s, e) =>
            {
                System.Console.WriteLine("Canceling...");
                cts.Cancel();
                e.Cancel = true;
            };

            try
            {
                Log.Information("Press Ctrl+C to end.");
                await MainAsync(config, cts.Token);
            }
            catch (OperationCanceledException)
            {
                // This is the normal way we close.
            }

            // Finally, once just before the application exits...
            await Log.CloseAndFlushAsync();
        }

        private static async Task MainAsync(IConfigurationRoot config, CancellationToken token)
        {
            string nodeUrl = config["node:url"];

            string mnemonic = config["account:mnemonic"];

            var account = Mnemonic.GetAccountFromMnemonic(mnemonic, "", KeyType.Sr25519);

            Log.Information("Account {Address}", account.ToString());

            var client = new SubstrateNetwork(account, NetworkType.Live, nodeUrl);

            await client.ConnectAsync(true, true, token);

            var block = await client.SubstrateClient.Chain.GetBlockAsync();

            Log.Information("Connected to node {NodeUrl} with block {Block}", nodeUrl, block.Block.Header.Number);

            // we are ready to execute an extrinsic
            // since we want to know what exactly happens, we will subscribe to the extrinsic
            // and we will have to wait till we get finalized or at least bestinblock
            var tcs = new TaskCompletionSource<(bool IsSuccess, ExtrinsicInfo ExtrinsicInfo)>();
            // also we only want to listen to the subscription id from the transaction
            string? mySubscriptionId = null;
            client.ExtrinsicManager.ExtrinsicUpdated += (string subscriptionId, ExtrinsicInfo extrinsicInfo) =>
            {
                if (mySubscriptionId == subscriptionId)
                {
                    Log.Information("SubscriptionId {subscriptionId} - {ExtrinsicStatus}", subscriptionId, extrinsicInfo.TransactionEvent);
                    if (extrinsicInfo.TransactionEvent == NetApi.Model.Rpc.TransactionEvent.Finalized)
                    {
                        tcs.SetResult((true, extrinsicInfo));
                    }
                }
            };

            string remark = "Hello, World!";

            Log.Information("Sending a remark with the content '{remark}'", remark);
            // now we build our method which is the content of the extrinsic each pallet has a Calls
            var method = SystemCalls.RemarkWithEvent(new BaseVec<U8>(remark.ToU8Array()));

            mySubscriptionId = await client.GenericExtrinsicAsync(account, "SystemCalls.RemarkWithEvent", method, 1, token);
            
            // task could not be handled from the node something must have been wrong!
            if (mySubscriptionId == null)
            {
                Log.Warning("SystemCalls.RemarkWithEvent couldn't execute.");
                await client.DisconnectAsync();
                return;
            }

            // let's wait till the extrinsic is finalized, or we time out after one minute
            var taskFinished = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromMinutes(1)));
            if (taskFinished != tcs.Task)
            {
                Log.Warning("Task timed out!");
                await client.DisconnectAsync();
                return;
            }

             var result = ((Task<(bool IsSuccess, ExtrinsicInfo ExtrinsicInfo)>)taskFinished).Result;

            // now we have the transaction finalized let's find out if the extrinsic was also successful
            bool flag = result.ExtrinsicInfo.SystemExtrinsicEvent(out var systemExtrinsicEvent, out var errorMsg);
            Log.Information("SystemExtrinsicEvent {systemExtrinsicEvent} - ErrorMsg {errorMsg}", systemExtrinsicEvent, errorMsg);

            // we can also check all existing events of the transactions
            Log.Information("The Extrinsic had {count} events", result.ExtrinsicInfo.EventRecords.Count);
            result.ExtrinsicInfo.EventRecords.ForEach(e =>
            {
                Log.Information("Event {Event}", e.Event.Value);

                switch (e.Event.Value2)
                {
                    case Opal.NET.NetApiExt.Generated.Model.pallet_balances.pallet.EnumEvent typedEvent:
                        Log.Information("+- Event Value2 {fullname}", typedEvent.Value);
                        switch(typedEvent.Value)
                        {
                            case Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_balances.pallet.Event.Transfer:
                                {
                                    var typedValues = (BaseTuple<AccountId32, AccountId32, U128>)typedEvent.Value2;
                                    Log.Information("   +- Transfer {from} -> {to} - {value}", ((AccountId32)typedValues.Value[0]).ToAddress(), ((AccountId32)typedValues.Value[1]).ToAddress(), ((U128)typedValues.Value[2]).Value.ToString());
                                    break;
                                }
                            case Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_balances.pallet.Event.Withdraw:
                                {
                                    var typedValues = (BaseTuple<AccountId32, U128>)typedEvent.Value2;
                                    Log.Information("   +- Withdraw {from} - {value}", ((AccountId32)typedValues.Value[0]).ToAddress(), ((U128)typedValues.Value[1]).Value.ToString());
                                }
                                break;
                            case Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_balances.pallet.Event.Deposit:
                                {
                                    var typedValues = (BaseTuple<AccountId32, U128>)typedEvent.Value2;
                                    Log.Information("   +- Deposit {from} - {value}", ((AccountId32)typedValues.Value[0]).ToAddress(), ((U128)typedValues.Value[1]).Value.ToString());
                                }
                                break;
                        }
                        break;
                    case Opal.NET.NetApiExt.Generated.Model.frame_system.pallet.EnumEvent typedEvent:
                        Log.Information("+- Event Value2 {fullname}", typedEvent.Value);
                        switch (typedEvent.Value)
                        {
                            case Substrate.Opal.NET.NetApiExt.Generated.Model.frame_system.pallet.Event.Remarked:
                                {
                                    var typedValues = (BaseTuple<AccountId32, H256>)typedEvent.Value2;
                                    Log.Information("   +- Remarked {from} - {hash}", ((AccountId32)typedValues.Value[0]).ToAddress(), ((H256)typedValues.Value[1]).ToHexString());
                                }
                                break;
                        }
                        break;
                    case Opal.NET.NetApiExt.Generated.Model.pallet_treasury.pallet.EnumEvent typedEvent:
                        Log.Information("+- Event Value2 {fullname}", typedEvent.Value);
                        switch (typedEvent.Value)
                        {
                            case Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_treasury.pallet.Event.Deposit:
                                {
                                    var typedValues = (U128)typedEvent.Value2;
                                    Log.Information("   +- Deposit {value}", typedValues.Value.ToString());
                                }
                                break;
                        }
                        break;

                    // add unhandled cases ...
                    default:
                        Log.Information("Unhandled event type: {Type}", e.Event.Value2.GetType().Name);
                        var value2 = e.Event.Value2;
                        Type type = value2.GetType();
                        foreach (var property in type.GetProperties())
                        {
                            var propertyName = property.Name;
                            var propertyValue = property.GetValue(value2, null);
                            Log.Information("   +- {PropertyName}: {PropertyValue}", propertyName, propertyValue);
                        }
                        break;
                }
            });

            await client.DisconnectAsync();
        }

        /// <summary>
        /// This is a method that creates an enumeration runtime call, and allows you to use it
        /// for example as a parameter of an extrinsic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public EnumRuntimeCall BalancesTransferKeepAlive(AccountId32 target, BigInteger amount)
        {
            var baseU128 = new BaseCom<U128>();
            baseU128.Create(amount);

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, target);

            var baseTubleParams = new BaseTuple<EnumMultiAddress, BaseCom<U128>>();
            baseTubleParams.Create(multiAddress, baseU128);

            var enumPalletCall = new Opal.NET.NetApiExt.Generated.Model.pallet_balances.pallet.EnumCall();
            enumPalletCall.Create(Opal.NET.NetApiExt.Generated.Model.pallet_balances.pallet.Call.transfer_keep_alive, baseTubleParams);

            var enumCall = new EnumRuntimeCall();
            enumCall.Create(RuntimeCall.Balances, enumPalletCall);

            return enumCall;
        }
    }
}