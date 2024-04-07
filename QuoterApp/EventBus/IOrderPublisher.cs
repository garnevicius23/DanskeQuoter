using QuoterApp.DataAccess;
using QuoterApp.Model;
using System;

namespace QuoterApp.EventBus
{
    public interface IOrderPublisher
    {
        event EventHandler<MarketOrderEvent> Event;
        void StartEventPublisher(IMarketOrderSource marketOrderSource);
    }
}
