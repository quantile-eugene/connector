using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BitAPI.Bitmex
{
    //{"table":"position","keys":["account","symbol","currency"],
    //"types":{"account":"long","symbol":"symbol","currency":"symbol","underlying":"symbol","quoteCurrency":"symbol","commission":"float",
    //"initMarginReq":"float","maintMarginReq":"float","riskLimit":"long","leverage":"float","crossMargin":"boolean",
    //"deleveragePercentile":"float","rebalancedPnl":"long","prevRealisedPnl":"long","prevUnrealisedPnl":"long","prevClosePrice":"float","openingTimestamp":"timestamp","openingQty":"long","openingCost":"long","openingComm":"long","openOrderBuyQty":"long","openOrderBuyCost":"long","openOrderBuyPremium":"long","openOrderSellQty":"long","openOrderSellCost":"long","openOrderSellPremium":"long","execBuyQty":"long","execBuyCost":"long","execSellQty":"long","execSellCost":"long","execQty":"long","execCost":"long","execComm":"long","currentTimestamp":"timestamp","currentQty":"long","currentCost":"long","currentComm":"long","realisedCost":"long","unrealisedCost":"long","grossOpenCost":"long","grossOpenPremium":"long","grossExecCost":"long","isOpen":"boolean","markPrice":"float","markValue":"long","riskValue":"long","homeNotional":"float","foreignNotional":"float","posState":"symbol","posCost":"long","posCost2":"long","posCross":"long","posInit":"long","posComm":"long","posLoss":"long","posMargin":"long","posMaint":"long","posAllowance":"long","taxableMargin":"long","initMargin":"long","maintMargin":"long","sessionMargin":"long","targetExcessMargin":"long","varMargin":"long","realisedGrossPnl":"long","realisedTax":"long","realisedPnl":"long","unrealisedGrossPnl":"long","longBankrupt":"long","shortBankrupt":"long","taxBase":"long","indicativeTaxRate":"float","indicativeTax":"long","unrealisedTax":"long","unrealisedPnl":"long","unrealisedPnlPcnt":"float","unrealisedRoePcnt":"float","simpleQty":"float","simpleCost":"float","simpleValue":"float","simplePnl":"float","simplePnlPcnt":"float","avgCostPrice":"float","avgEntryPrice":"float","breakEvenPrice":"float","marginCallPrice":"float","liquidationPrice":"float","bankruptPrice":"float","timestamp":"timestamp","lastPrice":"float","lastValue":"long"},"foreignKeys":{"symbol":"instrument"},"attributes":{"account":"sorted","symbol":"grouped","currency":"grouped","underlying":"grouped","quoteCurrency":"grouped"},"action":"partial","data":[],"filter":{"account":86865}}
    //"deleveragePercentile":"float","rebalancedPnl":"long","prevRealisedPnl":"long","prevUnrealisedPnl":"long","prevClosePrice":"floa;t",
    //"openingTimestamp":"timestamp","openingQty":"long","openingCost":"long","openingComm":"long","openOrderBuyQty":"long",
    //"openOrderBuyCost":"long","openOrderBuyPremium":"long","openOrderSellQty":"long","openOrderSellCost":"long","openOrderSellPremium":"long",
    //"execBuyQty":"long","execBuyCost":"long","execSellQty":"long","execSellCost":"long","execQty":"long","execCost":"long","execComm":"long",
    //"currentTimestamp":"timestamp","currentQty":"long",
    //"currentCost":"long","currentComm":"long","realisedCost":"long","unrealisedCost":"long","grossOpenCost":"long","grossOpenPremium":"long",
    //"grossExecCost":"long","isOpen":"boolean","markPrice":"float","markValue":"long","riskValue":"long","homeNotional":"float",
    //"foreignNotional":"float","posState":"symbol","posCost":"long","posCost2":"long","posCross":"long","posInit":"long","posComm":"long",
    //"posLoss":"long","posMargin":"long","posMaint":"long","posAllowance":"long",
    //"taxableMargin":"long","initMargin":"long","maintMargin":"long","sessionMargin":"long","targetExcessMargin":"long","varMargin":"long",
    //"realisedGrossPnl":"long","realisedTax":"long","realisedPnl":"long","unrealisedGrossPnl":"long","longBankrupt":"long",
    //"shortBankrupt":"long","taxBase":"long","indicativeTaxRate":"float","indicativeTax":"long","unrealisedTax":"long","unrealisedPnl":"long",
    //"unrealisedPnlPcnt":"float","unrealisedRoePcnt":"float","simpleQty":"float","simpleCost":"float","simpleValue":"float",
    //"simplePnl":"float","simplePnlPcnt":"float","avgCostPrice":"float","avgEntryPrice":"float","breakEvenPrice":"float",
    //"marginCallPrice":"float","liquidationPrice":"float","bankruptPrice":"float","timestamp":"timestamp","lastPrice":"float",
    //"lastValue":"long"},"foreignKeys":{"symbol":"instrument"},"attributes":{"account":"sorted","symbol":"grouped","currency":"grouped","underlying":"grouped","quoteCurrency":"grouped"},"action":"partial","data":[],"filter":{"account":86865}}
    using AccountT = Int32;
    using PriceT = Double;

    public class Position
    {
        public AccountT Account;
        public string Symbol;
        public string Currency;
        public string Underlying;
        public string QuoteCurrency;
        public float Commission;
        public float InitMarginReq;
        public float MainMarginReq;
        public long RiskLimit;
        public float Leverage;
        public bool CrossMargin;        
        public long RebalancedPnl;
        public long PrevRealisedPnl;
        public long PrevUnreliasedPnl;
        public float PrevClosePrice;
        public DateTime OpeningTimestamp;
        public long OpeningQty;
        public long OpeningCost;
        public long OpeningComm;
        public long OpenOrderBuyQty;
        public long OpenOrderBuyCost;
        public long OpenOrderBuyPremium;        
        public long OpenOrderSellQty;
        public long OpenOrderSellCost;
        public long OpenOrderSellPremium;
        public long ExecBuyQty;
        public long ExecBuyCost;
        public long ExecSellQty;
        public long ExecQty;
        public long ExecCost;
        public long ExecComm;
        public DateTime CurrentTimestamp;
        public long CurrentQty;        
        public long CurrentCost;
        public long CurrentComm;
        public long RealisedCost;
        public long UnrealisedCost;
        public long GrossOpenCost;
        public long GrossOpenPremium;
        public long GrossExecCost;
        public bool IsOpen = false;
        public PriceT MarkPrice;
        public long MarkValue;
        public long RiskValue;
        public float HomeNotional;        
        public float ForeignNotional;
        public string PosState;
        public long PosCost;
        public long PosCost2;
        public long PosCross;
        public long PosInit;
        public long PosComm;
        public long PosLoss;
        public long PosMargin;
        public long PosMaint;
        public long PosAllowance;        
        public long TaxableMargin;
        public long InitMargin;
        public long MaintMargin;
        public long SessionMargin;
        public long TargetExcessMargin;
        public long VarMargin;        
        public long RealisedGrossPnl;
        public long RealisedTax;
        public long RealisedPnl;
        public long UnrealisedGrossPnl;
        public long LongBankrupt;
        public long ShortBankrupt;
        public long TaxBase;
        public float IndicativeTaxRate;
        public long InidicativeTax;
        public long UnrealisedTax;
        public long UnrealisedPnl;
        public float UnrealizedPnlPcnt;
        public float UnealisedRoePcnt;
        public float SimpleQty;
        public float SimpleCost;
        public float SimpleValue;
        public float SimplePnl;
        public float SimplePnlPcnt;
        public PriceT AvgCostPrice;
        public PriceT AvgEntryPrice;
        public PriceT BreakEventPrice;
        public PriceT MarginCallPrice;
        public PriceT LiquidationPrice;
        public PriceT BankruptPrice;
        public DateTime Timestamp;
        public long LastValue;
        public PriceT LastPrice; 

        public static Position ReadFromJObject(JToken aValue)
        {
            var r = new Position()
            {
                Symbol = UtilsParser.parseString(aValue, "symbol"),
                Account = (AccountT)UtilsParser.parseInt(aValue, "account"),
                Currency = UtilsParser.parseString(aValue, "currency"),
                Underlying = UtilsParser.parseString(aValue, "underlying"),
                OpeningQty = UtilsParser.parseInt(aValue, "openingQty"),
                CurrentQty = UtilsParser.parseInt(aValue, "currentQty"),
                CurrentComm = UtilsParser.parseInt(aValue, "currentComm"),
                CurrentCost = UtilsParser.parseInt(aValue, "currentCost"),
                OpenOrderBuyQty = UtilsParser.parseInt(aValue, "openOrderBuyQty"),
                OpenOrderBuyCost = UtilsParser.parseInt(aValue, "openOrderBuyCost"),
                OpenOrderBuyPremium = UtilsParser.parseInt(aValue, "openOrderBuyPremium"),
                OpenOrderSellQty = UtilsParser.parseInt(aValue, "openOrderSellQty"),
                OpenOrderSellCost = UtilsParser.parseInt(aValue, "openOrderSellCost"),
                OpenOrderSellPremium = UtilsParser.parseInt(aValue, "openOrderSellPremium"),
                VarMargin = UtilsParser.parseInt(aValue, "varMargin"),
                LastPrice = UtilsParser.parsePrice(aValue, "lastPrice"),
                LastValue = UtilsParser.parseInt(aValue, "lastValue"),
                SimpleQty = UtilsParser.parseFloat(aValue, "simpleQty"),
                SimpleCost = UtilsParser.parseFloat(aValue, "simpleCost"),
                SimpleValue = UtilsParser.parseFloat(aValue, "simpleValue"),
                SimplePnl = UtilsParser.parseFloat(aValue, "simplePnl"),
                IsOpen = UtilsParser.parseInt(aValue, "isOpen") == 1,
                RealisedPnl = UtilsParser.parseInt(aValue, "realisedPnl"),
                UnrealisedPnl = UtilsParser.parseInt(aValue, "unrealisedPnl"),
                RealisedTax = UtilsParser.parseInt(aValue, "realisedTax"),
                UnrealisedTax = UtilsParser.parseInt(aValue, "unrealisedTax"),
                TaxBase = UtilsParser.parseInt(aValue, "taxBase"),
                Timestamp = UtilsParser.parseDate(aValue, "timestamp"),
            };

            return r;
        }

        public static List<Position> ReadFromJArray(JArray aValue)
        {
            List<Position> res = new List<Position>();
            foreach(JObject obj in aValue)
            {
                res.Add(Position.ReadFromJObject(obj));
            }
            return res;
        }

        //public static bool operator==(Position aLeft, Position aRight)
        //{
        //    return (aLeft.Account == aRight.Account);
        //}
    }
}
