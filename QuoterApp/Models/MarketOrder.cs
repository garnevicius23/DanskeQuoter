using System;

namespace QuoterApp.Model
{
    public class MarketOrder
    {
        /// <summary>
        /// Id of the Instrument
        /// </summary>
        public string InstrumentId { get; set; }

        /// <summary>
        /// Price of this Instrument available
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Quantity of how many units available at given Price
        /// </summary>
        public int Quantity { get; set; }
    }

    public class MarketOrderEvent : EventArgs
    {
        public string InstrumentId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public MarketOrderEvent(MarketOrder marketOrder)
        {
            InstrumentId = marketOrder.InstrumentId;
            Price = marketOrder.Price;
            Quantity = marketOrder.Quantity;
        }

    }
}
