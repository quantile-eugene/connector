using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAPI.Common;

namespace BitAPI
{
    public enum OrderStatus { ACTIVE=1, CANCLED=2, CLOSED=3, UNKNOWN=0};
    public class Order
    {
        public Order()
        {
            status = OrderStatus.UNKNOWN;
        }


        public OrderStatus status
        {
            set;
            get;
        }

        public long orderId
        {
            set;
            get;
        }

        public double price
        {
            set;
            get;
        }

        public double amount
        {
            set;
            get;
        }

        public SideT type
        {
            set;
            get;
        }

        public DateTime date
        {
            set;
            get;
        }
    }
}
