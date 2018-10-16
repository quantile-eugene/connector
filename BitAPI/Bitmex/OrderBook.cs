using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using BitAPI.Common;

namespace BitAPI.Bitmex
{
    public enum OrderBookAction {INSERT=1, DELETE=2, UPDATE=3, UNKNOWN=0};
    public class OrderBook
    {
        public List<OrderBookItem> Items;
        public OrderBookAction Action = OrderBookAction.UNKNOWN;

        public static OrderBookAction ReadFromJObject(JToken aToken)
        {
            string action = UtilsParser.parseString(aToken, "action");
            if (action == "insert")
                return OrderBookAction.INSERT;
            else if (action == "delete")
                return OrderBookAction.DELETE;
            else if (action == "update")
                return OrderBookAction.UPDATE;
            else
                return OrderBookAction.UNKNOWN;

        }

        public static List<OrderBookItem> ReadFromJObject(JArray aArray)
        {
            List<OrderBookItem> items = new List<OrderBookItem>();

            if (aArray == null)
                return null;

            for (int i = 0; i < aArray.Count; i++)
            {
                JToken value = aArray[i];
                OrderBookItem item = new OrderBookItem();
                item.Symbol = UtilsParser.parseString(value, "symbol");
                item.Id = UtilsParser.parseInt(value, "id").ToString();
                item.Side = UtilsParser.parseSide(UtilsParser.parseString(value, "side"));
                item.Size = UtilsParser.parseInt(value, "size");
                item.Price = (decimal)UtilsParser.parsePrice(value, "price");
                items.Add(item);
            }
            return items;
        }
    }

    public class OrderBookItem
    {
        public string Symbol;
        public string Id;
        public SideT Side;
        public long Size;
        public decimal? Price;
    }
}
