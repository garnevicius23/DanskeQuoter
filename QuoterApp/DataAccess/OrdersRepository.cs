using QuoterApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace QuoterApp.DataAccess
{
    public class OrdersRepository : IOrdersRepository
    {
        private Dictionary<string, List<MarketOrder>> _orders;

        public OrdersRepository()
        {
            _orders = new Dictionary<string, List<MarketOrder>>();
        }

        public List<MarketOrder> GetInstrumentOrders(string instrumentId)
        {
            var orders = _orders;

            if (orders == null)
                return null;

            return orders
                .Where(x => x.Key.Equals(instrumentId))
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public void StoreMarketOrder(MarketOrder marketOrder)
        {
            if(_orders.ContainsKey(marketOrder.InstrumentId))
                _orders[marketOrder.InstrumentId].Add(marketOrder);
            else
                _orders[marketOrder.InstrumentId] = new List<MarketOrder>() { marketOrder };
        }
    }
}
