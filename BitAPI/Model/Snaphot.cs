using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI.Model
{
    public class Quote
    {
        public decimal Size;
        public decimal Price;

        public Quote()
        {
            Size = 0;
            Price = 0;
        }

        public Quote(decimal aSize, decimal aPrice)
        {
            Size = aSize;
            Price = aPrice;
        }
    }

    public class Snaphot
    {
        public string Symbol;
        public DateTime TimeStamp;
        public List<Quote> Bids = new List<Quote>();
        public List<Quote> Asks = new List<Quote>();

        public void clear()
        {
            Bids.Clear();
            Asks.Clear();
        }

        public void addBid(Quote aQuote)
        {
            Bids.Add(aQuote);
        }

        public void addAsk(Quote aQuote)
        {
            Asks.Add(aQuote);
        }

        public Quote BestBid
        {
            get
            {
                if (Bids.Count > 0)
                {
                    return Bids[0];
                }
                return null;
            }
        }

        public Quote BestAsk
        {
            get
            {
                if (Asks.Count > 0)
                {
                    return Asks[0];
                }
                return null;
            }
        }

        public decimal TwapPrice
        {
            get 
            {
                if (BestAsk != null && BestAsk != null)
                {
                    return (BestAsk.Price * BestAsk.Size + BestBid.Price * BestBid.Size) / (BestBid.Size + BestAsk.Size);
                }
                else if (BestAsk != null)
                {
                    return BestAsk.Price;
                }
                else
                {
                    return BestBid.Price;
                }
            }
        }

        public Decimal MidPrice
        {
            get
            {
                if (BestAsk != null && BestAsk != null)
                {
                    return (BestAsk.Price + BestBid.Price) / 2;
                }
                else 
                {
                    return decimal.Zero;
                }                
            }
        }
    }
}
