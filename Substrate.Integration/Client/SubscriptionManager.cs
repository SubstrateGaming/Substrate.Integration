using Serilog;
using Substrate.NetApi.Model.Rpc;

namespace Substrate.Integration.Client
{
    /// <summary>
    /// Delegate for subscription updated event
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <param name="storageChangeSet"></param>
    public delegate void SubscriptionOnEvent(string subscriptionId, StorageChangeSet storageChangeSet);

    /// <summary>
    /// Subscription Manager
    /// </summary>
    public class SubscriptionManager
    {
        /// <summary>
        /// Is the subscription active
        /// </summary>
        public bool IsSubscribed { get; set; }

        /// <summary>
        /// Subscription updated event
        /// </summary>
        public event SubscriptionOnEvent SubscrptionEvent;

        /// <summary>
        /// Subscription Manager constructor
        /// </summary>
        public SubscriptionManager()
        {
            SubscrptionEvent += OnSystemEvents;
        }

        /// <summary>
        /// Simple extrinsic tester
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="storageChangeSet"></param>
        public void ActionSubscrptionEvent(string subscriptionId, StorageChangeSet storageChangeSet)
        {
            IsSubscribed = false;

            Log.Information("System.Events: {0}", storageChangeSet);

            SubscrptionEvent?.Invoke(subscriptionId, storageChangeSet);
        }

        /// <summary>
        /// On extrinsic updated
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="storageChangeSet"></param>
        private void OnSystemEvents(string subscriptionId, StorageChangeSet storageChangeSet)
        {
            Log.Debug("OnExtrinsicUpdated[{id}] updated {state}",
                subscriptionId,
                storageChangeSet);
        }
    }
}