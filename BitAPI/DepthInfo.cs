using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI
{
    public class DepthInfo
    {
        List<DepthValue> asks = new List<DepthValue>();
        List<DepthValue> bids = new List<DepthValue>();

        public DepthValue bestBuy
        {
            set;
            get;
        }

        public DepthValue bestSell
        {
            set;
            get;
        }

        public void addAskValue(DepthValue value)
        {
            if (bestSell == null)
                bestSell = value;

            asks.Add(value);
        }

        public void addBidValue(DepthValue value)
        {
            if (bestBuy == null)
                bestBuy = value;

            bids.Add(value);
        }

        public void clear()
        {
            asks.Clear();
            bids.Clear();
        }
    }
}
