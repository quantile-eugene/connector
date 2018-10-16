using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BitAPI.Common;

namespace BitAPI.Bitmex
{

    using PriceT = Double;
    public class UtilsParser
    {        
        public static string parseString(JToken aToken, string aName)
        {
            JValue find = (JValue)aToken.SelectToken(aName);
            if (find != null)
            {
                if (find.Type == JTokenType.String)
                {
                    return (string)find.Value;
                }
            }
            return "";
        }

        public static SideT parseSide(JToken aToken, string aName)
        {
            JValue find = (JValue)aToken.SelectToken(aName);
            if (find != null)
            {
                if (find.Type == JTokenType.String)
                {
                    return ConvertToEnum((string)find.Value);
                }
            }
            return SideT.UNDEFINED;
        }
                
        public static SideT ConvertToEnum(string aValue)
        {
            if(aValue == "Buy")
                return SideT.BUY;
            else if (aValue == "Sell")
                return SideT.SELL;
            else
                return SideT.UNDEFINED;
        }

        /*public static OrdTypeT ConvertToEnum(string aValue)
        {
            return OrdTypeT.UNDEFINED;
        }
        */
        public static PriceT parsePrice(JToken aToken, string aName)
        {
            JValue find = (JValue)aToken.SelectToken(aName);
            if (find != null)
            {
                object value = find.Value;
                if (value != null)
                {
                    Type t = value.GetType();

                    if (t == typeof(Double))
                    {
                        return (PriceT)(((double)find.Value));
                    }
                    else if (t == typeof(long))
                    {
                        return (PriceT)(double)((long)find.Value);
                    }
                }
            }
            return 0;
        }

        public static float parseFloat(JToken aToken, string aName)
        {
            JValue find = (JValue)aToken.SelectToken(aName);
            if (find != null)
            {
                if (find.Type == JTokenType.Float)
                {
                    return (float)find;
                }
                else if (find.Type == JTokenType.Integer)
                {
                    return (float)(long)find.Value;
                }
            }
            return 0;
        }

        public static long parseInt(JToken aToken, string aName)
        {
            JValue find = (JValue)aToken.SelectToken(aName);
            if (find != null)
            {
                if (find.Type == JTokenType.Integer)
                {
                    return (long)find.Value;
                }
            }
            return 0;
        }

        public static DateTime parseDate(JToken aToken, string aName)
        {
            JValue find = (JValue)aToken.SelectToken(aName);
            if (find != null)
            {
                if (find.Type == JTokenType.Date)
                {
                    return (DateTime)find;
                }
            }
            return new DateTime();
        }

        public static SideT parseSide(string aSide)
        {
            aSide = aSide.ToLower();
            if (aSide == "sell")
                return SideT.SELL;
            else if (aSide == "buy")
                return SideT.BUY;
            else
                return SideT.UNDEFINED;
        }

        public static OrdStatusT parseOrdStatus(string aStatus)
        {
            aStatus = aStatus.ToLower();
            if (aStatus == "new")
                return OrdStatusT.NEW;
            else if (aStatus == "filled")
                return OrdStatusT.FILLED;
            else if (aStatus == "canceled")
                return OrdStatusT.CANCELLED;
            else if (aStatus == "partial_filled")
                return OrdStatusT.PARTIAL_FILLED;
            else if (aStatus == "rejected")
                return OrdStatusT.REJECTED;
            else if (aStatus == "replaced")
                return OrdStatusT.REPLACED;
            else
                return OrdStatusT.UNDEFINED;
        }

        public static ExecTypeT parseExecType(string aStatus)
        {
            aStatus = aStatus.ToLower();
            if (aStatus == "new")
                return ExecTypeT.NEW;
            else if (aStatus == "trade")
                return ExecTypeT.TRADE;
            else if (aStatus == "canceled")
                return ExecTypeT.CANCELED;
            else if (aStatus == "trade")
                return ExecTypeT.TRADE;
            else
                return ExecTypeT.UNDEFINED;
        }

        public static OrdRejReasonT parseOrdRejectReason(string aRejReason)
        {
            aRejReason = aRejReason.ToLower();
            /*if (aRejReason == "new")
                return OrdRejReasonT.NEW;
            else if (aRejReason == "filled")
                return OrdRejReasonT.FILLED;            
            else
             */
                return OrdRejReasonT.UNDEFINED;
        }

        public static TimeInForceT parseTimeInForce(string aType)
        {
            aType = aType.ToLower();
            if (aType == "goodtillcancel")
            {
                return TimeInForceT.GTC;
            }
            else if (aType == "immediateorcancel")
            {
                return TimeInForceT.IOC;                
            }
            else if (aType == "fillorkill")
            {
                return TimeInForceT.FOK;
            }
            else
            {
                return TimeInForceT.UNDEFINED;
            }
       }

        public static OrdTypeT parseOrdType(string aType)
        {
            aType = aType.ToLower();
            if (aType == "market")
            {
                return OrdTypeT.MARKET;
            }
            else if (aType == "limit")
            {
                return OrdTypeT.LIMIT;
            }
            else if (aType == "stop")
            {
                return OrdTypeT.STOP;
            }
            else if (aType == "stop_limit")
            {
                return OrdTypeT.STOPLIMIT;
            }
            else
            {
                return OrdTypeT.UNDEFINED;
            }
       }
        
        public static string ToString(TimeInForceT aTimeInForce)
        {
            switch (aTimeInForce)
            {
                case TimeInForceT.FOK:
                    return "FillOrKill";
                case TimeInForceT.IOC:
                    return "ImmediateOrCancel";
                case TimeInForceT.GTC:
                    return "GoodTillCancel";
                case TimeInForceT.DAY:
                    return "Day";
                default:
                    return "";
            }
        }

        public static string ToString(OrdTypeT aOrderType)
        {
            switch (aOrderType)
            {
                case OrdTypeT.MARKET:
                    return "Market";
                case OrdTypeT.LIMIT:
                    return "Limit";
                default:
                    return "";
            }
        }

        public static string ToString(SideT aSide)
        {
            switch (aSide)
            {
                case SideT.BUY:
                    return "Buy";
                case SideT.SELL:
                    return "Sell";
                default:
                    return "UNKNOWN";
            }
        }

        public static string ToString(PriceT aValue)
        {
            return aValue.ToString().Replace(",", ".");
        }

        public static JArray ParseData(string value)
        {
            JToken data = JObject.Parse(value);
            return (JArray)data.SelectToken("data");
        }

    }
}
