using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BitAPI.Bitmex
{
    public class Quote
    {
        public DateTime TimeStamp { get; private set; }
        public string Symbol { get; private set; }
        public decimal BidSize { get; private set; }
        public decimal BidPrice { get; private set; }
        public decimal OfferSize { get; private set; }
        public decimal OfferPrice { get; private set; }

        public static Quote ReadFromJObject(JArray o)
        {
            if (o == null)
                return null;

            if (o.Count == 1)
            {
                JToken value = o[0];  

                var r = new Quote()
                {
                    TimeStamp = UtilsParser.parseDate(value, "timestamp"),
                    Symbol = UtilsParser.parseString(value,"symbol"),
                    BidSize = UtilsParser.parseInt(value, "bidSize"),
                    BidPrice = (decimal)UtilsParser.parsePrice(value, "bidPrice"),
                    OfferPrice = (decimal)UtilsParser.parsePrice(value, "askPrice"),
                    OfferSize = UtilsParser.parseInt(value, "askSize"),
                };

                return r;
            }

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2}/{3};{4}/{5}", TimeStamp,Symbol, BidPrice,BidSize,OfferPrice,OfferSize);
        }
    }
}
