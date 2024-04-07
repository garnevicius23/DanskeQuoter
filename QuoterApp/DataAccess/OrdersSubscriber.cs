using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuoterApp.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuoterApp.DataAccess
{
    public class OrdersSubscriber : IHostedService
    {
        private readonly IMarketOrderSource _marketOrderSource;
        private readonly IOrderPublisher _subscriber;
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger _logger;
        private CancellationTokenSource _cancellationTokenSource;

        public OrdersSubscriber(IMarketOrderSource marketOrderSource,
                                IOrderPublisher publisher,
                                IOrdersRepository repository,
                                ILogger logger) 
        {
            _marketOrderSource = marketOrderSource;
            _subscriber = publisher;
            _ordersRepository = repository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => SubscribeOrders(), _cancellationTokenSource.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource?.Cancel();

            await Task.CompletedTask;
        }

        public void SubscribeOrders()
        {
            _subscriber.Event += (sender, e) => {
                _logger.LogDebug(e.InstrumentId);

                var marketOrder = new MarketOrder()
                {
                    InstrumentId = e.InstrumentId,
                    Quantity = e.Quantity,
                    Price = e.Price
                };

                _ordersRepository.StoreMarketOrder(marketOrder);
            };

            _subscriber.StartEventListener(_marketOrderSource);
        }
    }
}
