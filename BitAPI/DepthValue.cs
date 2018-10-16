using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI
{
    public class DepthValue
    {
        public DepthValue(double priceValue, double amountValue)
        {
            price = priceValue;
            amount = amountValue;
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
    }
}
