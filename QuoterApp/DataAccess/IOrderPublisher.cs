using QuoterApp.Model;
using System;

namespace QuoterApp.DataAccess
{
    public interface IOrderPublisher
    {
        event EventHandler<MarketOrderEvent> Event;
        void StartEventListener(IMarketOrderSource marketOrderSource);
    }
}
