using QuoterApp.Model;
using System.Collections.Generic;

namespace QuoterApp.DataAccess
{
    public interface IOrdersRepository
    {
        List<MarketOrder> GetInstrumentOrders(string instrumentId);
        void StoreMarketOrder(MarketOrder marketOrder);
    }
}
