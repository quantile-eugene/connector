using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using BitAPI.Common;

namespace BitAPI.Bitmex
{    
    public class Trade
    {
        public decimal Price { get; private set; }
        public decimal Size { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public SideT Side { get; private set; }
        public string Symbol { get; private set; }
        public string TradeId { get; private set; }
        public decimal GrossValue { get; private set; }
        public decimal HomeNotional { get; private set; }
        public decimal ForeignNotional { get; private set; }
        
        public static Trade ReadFromJObject(JArray o)
        {
            if (o == null)
                return null;

            var r = new Trade()
            {
                //Price = o.Value<decimal>(0),
                //Amount = o.Value<decimal>(1),
            };

            return r;
        }
    }

}
