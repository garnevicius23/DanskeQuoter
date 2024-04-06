using QuoterApp.Model;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace QuoterApp.DataAccess
{
    public interface IOrdersRepository
    {
        List<MarketOrder> GetInstrumentOrders(string instrumentId);
        Task SubscribeOrders();
    }
}
