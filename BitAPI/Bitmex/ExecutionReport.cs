using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using BitAPI.Common;


namespace BitAPI.Bitmex
{
    //{"table":"execution","keys":["execID"],"types":{"execID":"guid","orderID":"guid","clOrdID":"symbol","clOrdLinkID":"symbol","account":"long","symbol":"symbol","side":"symbol","lastQty":"long","lastPx":"float","underlyingLastPx":"float","lastMkt":"symbol","lastLiquidityInd":"symbol","simpleOrderQty":"float","orderQty":"long","price":"float","displayQty":"long","stopPx":"float","pegOffsetValue":"float","pegPriceType":"symbol","currency":"symbol","settlCurrency":"symbol","execType":"symbol","ordType":"symbol","timeInForce":"symbol","execInst":"symbol","contingencyType":"symbol","exDestination":"symbol","ordStatus":"symbol","triggered":"symbol","workingIndicator":"boolean","ordRejReason":"symbol","simpleLeavesQty":"float","leavesQty":"long","simpleCumQty":"float","cumQty":"long","avgPx":"float","commission":"float","tradePublishIndicator":"symbol","multiLegReportingType":"symbol","text":"symbol","trdMatchID":"guid","execCost":"long","execComm":"long","homeNotional":"float","foreignNotional":"float","transactTime":"timestamp","timestamp":"timestamp"},"foreignKeys":{"symbol":"instrument","side":"side","ordStatus":"ordStatus"},"attributes":{"execID":"grouped","account":"grouped","execType":"grouped","transactTime":"sorted"},"action":"partial","data":[],"filter":{"account":86865}}
    using PriceT = Double;
    using QtyT = Double;
    using AccountT = Int32;
    public class ExecutionReport
    {
        public string Symbol;
        public QtyT OrderQty;
        public PriceT Price;
        public string Currency;
        public SideT Side = SideT.UNDEFINED;
        public string OrderId;
        public string ClOrdId;

        public PriceT LastPx;
        public QtyT LastQty;
        public QtyT LeavesQty;
        public QtyT CumQty;
        public QtyT ExecCost;
        public QtyT ExecCom;
        public PriceT AvgPx;
        public OrdTypeT OrdType = OrdTypeT.UNDEFINED;
        public DateTime TransactTime;
        public ExecTypeT ExecType = ExecTypeT.UNDEFINED;
        public OrdRejReasonT OrdRejReason = OrdRejReasonT.UNDEFINED;
        public OrdStatusT OrdStatus = OrdStatusT.UNDEFINED;
        public TimeInForceT TimeInForce = TimeInForceT.UNDEFINED;
        public AccountT Account = 0;
        public string SettlCurrency = "";
        public string Text = "";
        public string TrdMatchId = "";
        public PriceT Commission;
        public string ExecInst = "";
        public string ClOrdLinkId = "";
        public string ExecId = "";
        public string Error = "";
        public QtyT SimpleCumQty = 0;

        public static ExecutionReport ReadFromJObject(JToken aValue)
        {
            var r = new ExecutionReport()
            {
                OrderId = UtilsParser.parseString(aValue, "orderID"),
                ClOrdId = UtilsParser.parseString(aValue, "clOrdID"),
                ExecId = UtilsParser.parseString(aValue, "execID"),
                Symbol = UtilsParser.parseString(aValue, "symbol"),
                Currency = UtilsParser.parseString(aValue, "currency"),
                Text = UtilsParser.parseString(aValue, "text"),
                Side = UtilsParser.parseSide(aValue, "side"),
                Price = UtilsParser.parsePrice(aValue, "price"),
                LastPx = UtilsParser.parsePrice(aValue, "lastPx"),
                AvgPx = UtilsParser.parsePrice(aValue, "avgPx"),
                OrderQty = UtilsParser.parseFloat(aValue, "orderQty"),
                LastQty = UtilsParser.parseFloat(aValue, "lastQty"),
                LeavesQty = UtilsParser.parseFloat(aValue, "leavesQty"),
                CumQty = UtilsParser.parseFloat(aValue, "cumQty"),
                Commission = UtilsParser.parseFloat(aValue, "commission"),
                OrdStatus = UtilsParser.parseOrdStatus(UtilsParser.parseString(aValue, "ordStatus")),
                Account = (AccountT)UtilsParser.parseInt(aValue, "account"),
                SettlCurrency = UtilsParser.parseString(aValue, "settlCurrency"),
                OrdType = UtilsParser.parseOrdType(UtilsParser.parseString(aValue, "ordType")),
                TimeInForce = UtilsParser.parseTimeInForce(UtilsParser.parseString(aValue, "timeInForce")),
                OrdRejReason = UtilsParser.parseOrdRejectReason(UtilsParser.parseString(aValue, "ordRejReason")),
                Error = UtilsParser.parseString(aValue, "error"),
                ExecCost = UtilsParser.parseFloat(aValue, "execCost"),
                ExecCom = UtilsParser.parseFloat(aValue, "execComm"),
                SimpleCumQty = UtilsParser.parseFloat(aValue, "simpleCumQty"),
            };    

            return r;
        }

        public static List<ExecutionReport> ReadFromJArray(JArray aValue)
        {
            List<ExecutionReport> result = new List<ExecutionReport>();

            foreach(JToken elem in aValue)
            {
                ExecutionReport r = ExecutionReport.ReadFromJObject(elem);
                result.Add(r);
            }
            return result;
        }
    }
}
