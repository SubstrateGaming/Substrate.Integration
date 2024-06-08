using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.frame_system;
using Substrate.Polkadot.NET.NetApiExt.Generated.Model.sp_core.crypto;
using System.Numerics;

namespace Substrate.Integration.Test
{
    public class Integration
    {
        private readonly string _nodeUrl = "wss://rpc-polkadot.luckyfriday.io";

        private SubstrateNetwork _client;

        [SetUp]
        public void Setup()
        {
            // create client
            _client = new SubstrateNetwork(null, _nodeUrl);
        }

        [TearDown]
        public void TearDown()
        {
            // dispose client
            _client.SubstrateClient.Dispose();
        }

        [Test]
        public async Task ConnectionTestAsync()
        {
            Assert.That(_client, Is.Not.Null);
            Assert.That(_client.IsConnected, Is.False);

            Assert.That(await _client.ConnectAsync(true, true, CancellationToken.None), Is.True);
            Assert.That(_client.IsConnected, Is.True);

            Assert.That(await _client.DisconnectAsync(), Is.True);
            Assert.That(_client.IsConnected, Is.False);
        }

        [Test]
        public async Task GetBlockNumberTestAsync()
        {
            Assert.That(_client, Is.Not.Null);
            Assert.That(_client.IsConnected, Is.False);

            Assert.That(await _client.ConnectAsync(true, true, CancellationToken.None), Is.True);
            Assert.That(_client.IsConnected, Is.True);

            var blockNumber = await _client.GetBlocknumberAsync(CancellationToken.None);
            Assert.That(blockNumber, Is.Not.Null);
            Assert.That(blockNumber.HasValue, Is.True);
            Assert.That(blockNumber.Value, Is.GreaterThan(21000000));

            Assert.That(await _client.DisconnectAsync(), Is.True);
            Assert.That(_client.IsConnected, Is.False);
        }

    }
}