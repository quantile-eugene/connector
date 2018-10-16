using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI.Common
{
    public enum OrderRequestStatus { NEW, ACTIVE, FILLED, CANCELED };
    public class OrderRequest
    {
        public string Id;
        public OrderRequestTypeT Type = OrderRequestTypeT.UNDEFINED;
        public DateTime TimeStamp = DateTime.Now;
        public string Symbol;
        public SideT Side = SideT.UNDEFINED;
        public decimal Price;
        public decimal Size;
        public OrdTypeT OrderType = OrdTypeT.UNDEFINED;
        public TimeInForceT TimeInForce = TimeInForceT.UNDEFINED;
        public OrderRequestStatus mOrderRequestStatus = OrderRequestStatus.NEW;
        public string ClOrdId;
        public string OrderId;
        
        public override string ToString()
        {
            return string.Format("OrderRequest: id:{0}; type:{1}; symbol:{2}", Id, Type, Symbol);
        }

        public List<Bitmex.ExecutionReport> mReports = new List<Bitmex.ExecutionReport>();


        public void addReport(Bitmex.ExecutionReport aExecutionReport)
        {
            if (Id == aExecutionReport.ClOrdId)
            {
                mReports.Add(aExecutionReport);
                switch(aExecutionReport.ExecType)
                {
                    case ExecTypeT.FILLED:
                    {
                        mOrderRequestStatus = OrderRequestStatus.FILLED;
                        break;
                    }
                    case ExecTypeT.CANCELED:
                    {
                        mOrderRequestStatus = OrderRequestStatus.CANCELED;
                        break;
                    }
                    default:
                    {
                        //TODO unknown type
                        break;
                    }
                }
            }
        }
    }
}
