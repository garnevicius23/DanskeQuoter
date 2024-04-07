using QuoterApp.DataAccess;
using QuoterApp.Model;
using System;

namespace QuoterApp.EventBus
{
    public class OrderPublisher : IOrderPublisher
    {
        public event EventHandler<MarketOrderEvent> Event;

        public void StartEventPublisher(IMarketOrderSource marketOrderSource)
        {
            while (true)
            {
                Event?.Invoke(this, new MarketOrderEvent(marketOrderSource.GetNextMarketOrder()));
            }
        }
    }
}
