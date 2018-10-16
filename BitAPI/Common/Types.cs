using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI.Common
{
    using PriceT = Double;
      
    public enum TimeInForceT { IOC=2, FOK=3, GTC=1, DAY=0, UNDEFINED = -1 };
    public enum OrdTypeT {LIMIT=1, MARKET=2, STOP=3, STOPLIMIT=4, UNDEFINED = 0};    
    public enum SideT { BUY=1, SELL=-1, UNDEFINED=0};   
    public enum OrdStatusT { NEW, FILLED, PARTIAL_FILLED, REPLACED, CANCELLED, REJECTED, UNDEFINED=0 };
    public enum ExecTypeT { NEW, CANCELED, FILLED, TRADE, UNDEFINED = 0  };
    public enum OrdRejReasonT { UNDEFINED = 0 };

    public enum OrderRequestTypeT { NEW_ORDER, ORDER_CANCEL, ORDER_REPLACE, UNDEFINED=0 };
    /*public class PriceT : Double 
    {
        public override string ToString()
        {
            return this.ToString().Replace(",", ".");
        }
    };*/

    /*
    public class QtyT : double 
    {
        public override string ToString()
        {
            return this.ToString().Replace(",", ".");
        }
    };*/

    public class Types
    {      

    }
}
