using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ChallengePad.Channel
{
    public class OperationsChannel
    {
        public static readonly string OPERATIONS_CHANNEL = "operations";

        public static async IAsyncEnumerable<long> Subscribe([EnumeratorCancellation] CancellationToken cancelToken, string configuration)
        {
            ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync(configuration);
            var sub = redis.GetSubscriber();
            var queue = sub.Subscribe(OPERATIONS_CHANNEL);
            while (!cancelToken.IsCancellationRequested)
            {
                yield return long.Parse((await queue.ReadAsync(cancelToken)).Message);
            }
        }

        public static async Task Publish(long operationId, string configuration)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration);
            var sub = redis.GetSubscriber();
            await sub.PublishAsync(OPERATIONS_CHANNEL, operationId);
        }
    }
}
