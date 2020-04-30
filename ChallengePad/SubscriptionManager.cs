using ChallengePad.Channel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChallengePad
{
    public class SubscriptionManager
    {
        private readonly IOptions<ChallengePadSettings> Settings;
        public SubscriptionManager(IOptions<ChallengePadSettings> settings)
        {
            Settings = settings;
        }

        public async Task Subscribe(CancellationToken token, Action<long> callback)
        {
            var operationIdEnumerable = OperationsChannel.Subscribe(token, Settings.Value.RedisConfiguration);
            await foreach (var operationId in operationIdEnumerable)
            {                                           // An operation has changed in the backend!
                var scopedId = operationId;             // foreach loop counters are by ref, not val
                callback(scopedId);
            }
        }
    }
}
