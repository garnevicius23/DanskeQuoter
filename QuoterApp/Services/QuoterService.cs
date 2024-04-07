using QuoterApp.DataAccess;
using System;
using System.Linq;

namespace QuoterApp.Services
{
    public class QuoterService : IQuoter
    {
        private readonly IOrdersRepository _ordersRepository;

        public QuoterService(IOrdersRepository ordersRepository) 
        {
            _ordersRepository = ordersRepository;
        }
        public double? GetQuote(string instrumentId, int quantity)
        {
            var orders = _ordersRepository.GetInstrumentOrders(instrumentId)
                .Where(x => x.Quantity == quantity);

            if(orders == null || !orders.Any())
                throw new ArgumentException("No market orders found");

            return orders.Select(x => x.Price).Min();
        }

        public double GetVolumeWeightedAveragePrice(string instrumentId)
        {
            var orders = _ordersRepository.GetInstrumentOrders(instrumentId);

            if (orders == null || !orders.Any())
                throw new ArgumentException("No market orders found");

            double totalValue = 0;
            double totalQuantity = 0;

            foreach (var order in orders)
            {
                totalValue += (order.Price * order.Quantity);
                totalQuantity += order.Quantity;
            }

            return Math.Round(totalValue / totalQuantity, 2);
        }
    }
}
