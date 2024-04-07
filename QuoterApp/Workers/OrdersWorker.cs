using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuoterApp.DataAccess;
using QuoterApp.EventBus;
using QuoterApp.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuoterApp.Workers
{
    public class OrdersWorker : IHostedService
    {
        private readonly IMarketOrderSource _marketOrderSource;
        private readonly IOrderPublisher _subscriber;
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrdersWorker> _logger;

        public OrdersWorker(IMarketOrderSource marketOrderSource,
                                IOrderPublisher publisher,
                                IOrdersRepository repository,
                                ILogger<OrdersWorker> logger)
        {
            _marketOrderSource = marketOrderSource;
            _subscriber = publisher;
            _ordersRepository = repository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => SubscribeOrders(), cancellationToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            UnsubscriberOrders();
            await Task.CompletedTask;
        }

        public void SubscribeOrders()
        {
            _logger.LogInformation("Subscribing Market orders...");
            _subscriber.Event += HandleNewMarketOrder;

            _subscriber.StartEventPublisher(_marketOrderSource);
        }

        public void UnsubscriberOrders()
        {
            _logger.LogInformation("Unsubscribing Market orders...");
            _subscriber.Event -= HandleNewMarketOrder;
        }

        public void HandleNewMarketOrder(object sender, MarketOrderEvent e)
        {
            _logger.LogDebug(e.InstrumentId);

            var marketOrder = new MarketOrder()
            {
                InstrumentId = e.InstrumentId,
                Quantity = e.Quantity,
                Price = e.Price
            };

            _ordersRepository.StoreMarketOrder(marketOrder);
        }
    }
}
