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
using Substrate.Unique.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.Unique.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.Unique.NET.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Substrate.Unique.NET.NetApiExt.Generated.Model.unique_runtime;
using Substrate.Unique.NET.NetApiExt.Generated.Model.up_data_structs;
using Substrate.Unique.NET.NetApiExt.Generated.Storage;
using System.Numerics;
using System.Security.Principal;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

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
                // and can also deep dive into each of the extrinsic events that happend
                Log.Information("Event {Event}", e.Event.Value);
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

            var enumPalletCall = new Unique.NET.NetApiExt.Generated.Model.pallet_balances.pallet.EnumCall();
            enumPalletCall.Create(Unique.NET.NetApiExt.Generated.Model.pallet_balances.pallet.Call.transfer_keep_alive, baseTubleParams);

            var enumCall = new EnumRuntimeCall();
            enumCall.Create(RuntimeCall.Balances, enumPalletCall);

            return enumCall;
        }
    }
}