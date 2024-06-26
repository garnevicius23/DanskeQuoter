﻿using QuoterApp.Model;
using System;

namespace QuoterApp.DataAccess
{
    public class OrderPublisher : IOrderPublisher
    {
        public event EventHandler<MarketOrderEvent> Event;

        public void StartEventListener(IMarketOrderSource marketOrderSource)
        {
            while (true)
            {
                Event?.Invoke(this, new MarketOrderEvent(marketOrderSource.GetNextMarketOrder()));
            }
        }
    }
}
