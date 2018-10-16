using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using BitAPI.Common;
using System.Diagnostics;

namespace BitAPI.Bitmex
{
    public class BitmexOrderManager
    {
        string mApiKey;
        string mApiSecret;
        StreamWriter mLogger;
        string mUrl = "";

        public delegate void ProcessExecutionReport(Bitmex.ExecutionReport aExecutionReport);
        public event ProcessExecutionReport onExecutionReport;

        public delegate void ProcessError(string aMessage);
        public event ProcessError onError;

        public BitmexOrderManager(StreamWriter aLogger, string aApiKey, string aApiSecret, string aUrl)
        {
            mApiKey = aApiKey;
            mApiSecret = aApiSecret;
            mLogger = aLogger;
            mUrl = "https://" + aUrl;
        }

        public void connect()
        {
            mLogger.WriteLine("OrderManager: connect");
        }

        public void disconnect()
        {
            mLogger.WriteLine("OrderManager: disconnect");
        }

        public List<Position> getPositions()
        {
            var param = new Dictionary<string, string>();
            string res = Query("GET", "/position", param, true, true);
            JArray arr = JArray.Parse(res);
            List<Position> positions = null;
            /*List<Position> reports = Position.ReadFromJArray(arr);
            if (reports != null && onExecutionReport != null)
            {
                foreach (Position report in reports)
                {
                    onExecutionReport(report);
                }
            }*/
            return positions;
        }

        public bool sendOrder(OrderRequest aOrderRequest)
        {
            ///https://github.com/BitMEX/api-connectors/blob/master/clients/csharp/src/IO.Swagger/Api/OrderApi.cs
            mLogger.WriteLine(string.Format("OrderManager: sendOrder {0}", aOrderRequest));
            var param = new Dictionary<string, string>();
            switch(aOrderRequest.Type)
            {
                case OrderRequestTypeT.NEW_ORDER:
                    {
                        param["symbol"] = aOrderRequest.Symbol;
                        param["side"] = UtilsParser.ToString(aOrderRequest.Side);
                        param["orderQty"] = aOrderRequest.Size.ToString();
                        param["ordType"] = UtilsParser.ToString(aOrderRequest.OrderType);
                        param["timeinforce"] = UtilsParser.ToString(aOrderRequest.TimeInForce);
                        param["clOrdID"] = aOrderRequest.Id;
                        if (aOrderRequest.OrderType != OrdTypeT.MARKET)
                        {
                            param["price"] = aOrderRequest.Price.ToString();
                        }
                        string res = Query("POST", "/order", param, true, true);
                        JToken obj = JObject.Parse(res);
                        ErrorMessage errorMessage = ErrorMessage.ReadFromJObject(obj);
                        if (errorMessage != null)
                        {
                            string msg = "ERROR: NewOrderSingle - " + errorMessage.Message;
                            mLogger.WriteLine(msg);
                            if (onError != null)
                            {
                                onError(msg);
                            }
                        }
                        else
                        {
                            ExecutionReport report = ExecutionReport.ReadFromJObject(obj);
                            if (report != null && onExecutionReport != null)
                            {
                                //Assert(report.ClOrdId == aOrderRequest.Id);
                                onExecutionReport(report);
                            }
                        }
                    }

                    break;

                case OrderRequestTypeT.ORDER_CANCEL:
                    {
                        if (aOrderRequest.OrderId != null && aOrderRequest.OrderId.Length > 0)
                        {
                            param["orderId"] = aOrderRequest.OrderId;
                        }
                        if (aOrderRequest.ClOrdId != null && aOrderRequest.ClOrdId.Length > 0)
                        {
                            param["clOrdID"] = aOrderRequest.ClOrdId;
                        }
                        param["text"] = "cancel order by id";
                        string res = Query("DELETE", "/order", param, true, true);
                        JArray arr = JArray.Parse(res);
                        List<ExecutionReport> reports = ExecutionReport.ReadFromJArray(arr);
                        if (reports != null && onExecutionReport != null)
                        {
                            foreach (ExecutionReport report in reports)
                            {
                                onExecutionReport(report);
                            }
                        }                        
                    }
                    break;

                case OrderRequestTypeT.ORDER_REPLACE:
                    break;

                default:
                    mLogger.WriteLine("Unknown type of OrderRequest");
                    break;
            }

            return true;
        }

        public string GetOrders()
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = "XBTUSD";
            //param["filter"] = "{\"open\":true}";
            //param["columns"] = "";
            //param["count"] = 100.ToString();
            //param["start"] = 0.ToString();
            //param["reverse"] = false.ToString();
            //param["startTime"] = "";
            //param["endTime"] = "";
            string res = Query("GET", "/order", param, true);

            return res;
        }

        private string BuildQueryData(Dictionary<string, string> param)
        {
            if (param == null)
                return "";            
            StringBuilder b = new StringBuilder();
            foreach (var item in param)
                b.Append(string.Format("&{0}={1}", item.Key, WebUtility.HtmlEncode(item.Value)));

            try { return b.ToString().Substring(1); }
            catch (Exception) { return ""; }
        }

        private string BuildJSON(Dictionary<string, string> param)
        {
            if (param == null)
                return "";

            var entries = new List<string>();
            foreach (var item in param)
            {
                entries.Add(string.Format("\"{0}\":\"{1}\"", item.Key, item.Value));
            }

            return "{" + string.Join(",", entries) + "}";
        }

        private string Query(string method, string function, Dictionary<string, string> param = null, bool auth = false, bool json = false)
        {
            string paramData = json ? BuildJSON(param) : BuildQueryData(param);
            string url = "/api/v1" + function + ((method == "GET" && paramData != "") ? "?" + paramData : "");
            string postData = (method != "GET") ? paramData : "";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(mUrl + url);
            webRequest.Method = method;

            //webRequest.Headers.Add("Connection", "keep-alive");

            if (auth)
            {
                string nonce = UtilsCrypto.GetNonce().ToString();
                string message = method + url + nonce + postData;                
                string signatureString = UtilsCrypto.GetSignature(mApiSecret, message);

                webRequest.Headers.Add("api-nonce", nonce);
                webRequest.Headers.Add("api-key", mApiKey);
                webRequest.Headers.Add("api-signature", signatureString);
            }

            try
            {
                if (postData != "")
                {
                    webRequest.ContentType = json ? "application/json" : "application/x-www-form-urlencoded";
                    var data = Encoding.UTF8.GetBytes(postData);
                    using (var stream = webRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                using (WebResponse webResponse = webRequest.GetResponse())
                using (Stream str = webResponse.GetResponseStream())
                using (StreamReader sr = new StreamReader(str))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                using (HttpWebResponse response = (HttpWebResponse)wex.Response)
                {
                    if (response == null)
                        throw;

                    using (Stream str = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(str))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
