using DefinedCaller.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DefinedCaller
{

    public class OrderBackgroundService : IOrderBackgroundService
    {
        public const string OrderIdPropertyKey = "OrderId";

        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentQueue<BackgroundMessage> _queue = 
            new ConcurrentQueue<BackgroundMessage>();

        public OrderBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task BackgroundOrderProcessingAsync(string orderId)
        {
            _queue.Enqueue(new BackgroundMessage
            {
                Type = BackgroundMessageType.ProcessMessage,
                Properties = new Dictionary<string, string> {
                    { OrderIdPropertyKey, orderId }
                }
            });

            await Task.CompletedTask;
        }

        public async Task RunBackgroundWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_queue.TryDequeue(out var message))
                {
                    continue;
                }

                using (var scopeProvider = _serviceProvider.CreateScope())
                {

                    switch (message.Type)
                    {
                        case BackgroundMessageType.ProcessMessage:
                            var service = ActivatorUtilities.GetServiceOrCreateInstance<OrderService>(scopeProvider.ServiceProvider);
                            var order = await service.GetOrderByIdAsync(message.Properties[OrderIdPropertyKey]);
                            await service.ProcessOrderAsync(order);
                            break;
                        default:
                            break;

                    }
                }
            }
        }
    }
}
