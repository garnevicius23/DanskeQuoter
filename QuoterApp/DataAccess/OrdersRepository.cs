using Microsoft.Extensions.Caching.Memory;
using QuoterApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoterApp.DataAccess
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOrderPublisher _publisher;
        private readonly IMarketOrderSource _marketOrderSource;

        public OrdersRepository(IMemoryCache memoryCache, IOrderPublisher orderPublisher, IMarketOrderSource marketOrderSource)
        {
            _memoryCache = memoryCache;
            _publisher = orderPublisher;
            _marketOrderSource = marketOrderSource;
        }

        public List<MarketOrder> GetInstrumentOrders(string instrumentId)
        {
            var orders = _orders;

            if (orders == null)
                return null;

            return orders
                .Where(x => x.Key.Equals(instrumentId))
                .Select(x => x.Value)
                .ToList();
        }

        public async Task SubscribeOrders()
        {
            _publisher.Event += (sender, e) => {
                Console.WriteLine(e.InstrumentId);

                var marketOrder = new MarketOrder()
                {
                    InstrumentId = e.InstrumentId,
                    Quantity = e.Quantity,
                    Price = e.Price
                };

                _orders[e.InstrumentId] = marketOrder;
            };

            _publisher.StartEventListener(_marketOrderSource);
        }

        private readonly object OrdersLock = new object();
        private Dictionary<string, MarketOrder> _orders
        {
            get
            {
                var cacheKey = "market_orders";
                var result = _memoryCache.Get<Dictionary<string, MarketOrder>>(cacheKey);

                if (result == null)
                {
                    lock (OrdersLock)
                    {
                        result = _memoryCache.Get<Dictionary<string, MarketOrder>>(cacheKey);

                        if (result == null)
                        {
                            result = new Dictionary<string, MarketOrder>();
                            _memoryCache.Set(cacheKey, result);
                        }
                    }
                }

                return result;
            }
        }
    }
}
