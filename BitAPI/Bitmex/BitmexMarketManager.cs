using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;
using WebSocket4Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Security.Cryptography;

namespace BitAPI.Bitmex
{
    public class BitmexMarketManager
    {
        string mUrl = "";
        JsonSerializer mSerializer = new JsonSerializer();
        RequstSequenceNumber mReqSeqNum = new RequstSequenceNumber();
        StreamWriter mLogger;
        WebSocket mSocket;
        HMACSHA256 mHashMaker;
        string mApiKey = "";
        string mApiSecret = "";
        bool mAutoConnect = true;

        public BitmexMarketManager(StreamWriter aLogger, string aApiKey, string aApiSecret, string aUrl)
        {
            mApiKey = aApiKey;
            mApiSecret = aApiSecret;
            mLogger = aLogger;
            mHashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(aApiSecret));
            mUrl = "wss://" + aUrl + "/realtime";
        }

        public void connect()
        {
            if (mSocket == null)
            {
                mSocket = new WebSocket(mUrl);
                mSocket.Opened += new System.EventHandler(OnOpened);
                mSocket.Closed += new System.EventHandler(OnClosed);
                mSocket.Error += new System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(OnError);
                mSocket.MessageReceived += new System.EventHandler<MessageReceivedEventArgs>(OnMessageReceived);
                mSocket.AutoSendPingInterval = 3;
                mSocket.EnableAutoSendPing = true;                
                mSocket.Open();
            }
            
            System.Threading.Thread.Sleep(3000);
        }

        public void disconnect()
        {
            if (mSocket != null)
            {
                mSocket.Close();
                mSocket = null;
            }
        }

        public void subscribe(string aTable, string aPair = "")
        {
            if (mSocket != null && mSocket.State == WebSocketState.Open)
            {
                //mSocket.Send("{\"op\": \"subscribe\", args: [\"orderBookL2:XBTUSD\"]}");
                //mSocket.Send("{\"op\": \"subscribe\", \"args\": [\"trade:XBTUSD\",\"instrument:XBTUSD\"]}");
                string command = "{\"op\": \"subscribe\", \"args\": [\"" + aTable + (aPair.Length > 0 ? (":" + aPair) : "") + "\"]}";
                mSocket.Send(command);
            }
        }

        public void unsubcribe(string aTable, string aPair)
        {
            if (mSocket != null && mSocket.State == WebSocketState.Open)
            {
                string command = "{\"op\": \"unsubscribe\", \"args\": [\"" + aTable + ":" + aPair + "\"]}";
                mSocket.Send(command);
            }
        }

        public void subscribePosition(string aPair)
        {
            subscribe("position");
        }

        public void subscribeExecution(string aPair)
        {
            subscribe("execution");
        }

        public void subscribeQuote(string aPair)
        {
            subscribe("quote", aPair);
        }

        public void subscribeInstrument(string aPair)
        {
            subscribe("instrument", aPair);
        }

        public void subscribeTrade(string aPair)
        {
            subscribe("trade", aPair);
        }

        public void subscribeOrderBook(string aPair)
        {
            subscribe("orderBookL2", aPair);
        }

        public void subscribeOrder(string aPair)
        {
            subscribe("order", aPair);
        }

        public void subscribeMargin(string aPair)
        {
            subscribe("margin", aPair);
        }

        private void sendAuth()
        {
            if (mSocket != null && mSocket.State == WebSocketState.Open)
            {
                long nonce = UtilsCrypto.GetNonce();
                string signature = UtilsCrypto.GetSignature(mApiSecret, string.Format("GET/realtime{0}", nonce));
                string command = "{\"op\": \"authKey\", \"args\": [\"" + mApiKey + "\", " + nonce + ", \"" + signature + "\"]}";
                mSocket.Send(command);
            }
        }

        private void OnError(object aObject, SuperSocket.ClientEngine.ErrorEventArgs aArgs)
        {
            System.Console.WriteLine("ERROR:" + aArgs.Exception.Message);
            mLogger.WriteLine("ERROR:" + aArgs.Exception.Message);
            mLogger.Flush();
        }

        private void OnOpened(object aObject, System.EventArgs aArgs)
        {            
            sendAuth();
        }

        private void OnClosed(object aObject, System.EventArgs aArgs)
        {
            System.Console.WriteLine("CLOSED");
            mLogger.Flush();
            mReqSeqNum.save();
            if (mAutoConnect)
            {
                System.Console.WriteLine("RECONNECT");
                mSocket = null;
                connect();
            }
        }

        private void OnMessageReceived(object aObject, MessageReceivedEventArgs aArgs)
        {
            mLogger.WriteLine(string.Format("{0}:{1}", DateTime.Now, aArgs.Message));
            System.Console.WriteLine(aArgs.Message);

            JToken result = JObject.Parse(aArgs.Message);

            JToken table = result.SelectToken("table");
            if (table != null)
            {
                processTable((string)((JValue)table).Value, result);
            }
        
            //var json = Encoding.ASCII.GetString(aArgs.Message);

            //var data = new OrderBookData();
            //var havelockOrderBook = JsonConvert.DeserializeObject<HavelockOrderBookJson>(aArgs.Message);
        }

        private void processTable(string aTable, JToken aToken)
        {
            JArray data = (JArray)aToken.SelectToken("data");
            if (aTable == "trade")
            {
                Trade trade = Trade.ReadFromJObject(data);
                if (onTrade != null)
                {
                    onTrade(trade);
                }
            }
            else if (aTable == "order")
            {
                ExecutionReport report = ExecutionReport.ReadFromJObject(data);
                if (onExecutionReport != null)
                {
                    onExecutionReport(report);
                }
            }
            else if (aTable == "execution")
            {
                ExecutionReport report = ExecutionReport.ReadFromJObject(data);
                if (onExecutionReport != null)
                {
                    onExecutionReport(report);
                }
            }
            else if (aTable == "position")
            {
                List<Position> positions = Position.ReadFromJArray(data);
                if (onPositionChanged != null)
                {
                    onPositionChanged(positions);
                }
            }
            else if (aTable == "margin")
            {

            }
            else if (aTable == "instrument")
            {

            }
            else if (aTable == "orderBookL2")
            {
                OrderBook orderBook = new OrderBook();
                orderBook.Items = OrderBook.ReadFromJObject(data);
                orderBook.Action = OrderBook.ReadFromJObject(aToken);
                if (orderBook.Items.Count > 0)
                {
                    //onOrderBook(orderBook);
                }
            }
            else if (aTable == "quote")
            {
                Quote quote = Quote.ReadFromJObject(data);
                if (quote != null)
                {
                    Model.Snaphot snapshot = getSnapshot(quote);
                    if (onSnapshot != null)
                    {
                        onSnapshot(snapshot);
                    }
                }
            }
            else if (aTable == "auth")
            {

            }
        }

        private Model.Snaphot getSnapshot(Quote aQuote)
        {
            Model.Snaphot temp;
            try
            {
                temp = mSnaphots[aQuote.Symbol];
            }
            catch
            {
                temp = new Model.Snaphot();
                temp.Symbol = aQuote.Symbol;
                mSnaphots[aQuote.Symbol] = temp;
            }
            temp.clear();
            temp.TimeStamp = aQuote.TimeStamp;
            temp.addBid(new Model.Quote(aQuote.BidSize, aQuote.BidPrice));
            temp.addAsk(new Model.Quote(aQuote.OfferSize, aQuote.OfferPrice));
            return temp;
        }

        public delegate void ProcessSnaphot(Model.Snaphot aSnapshot);
        public event ProcessSnaphot onSnapshot;

        public delegate void ProcessExecutionReport(ExecutionReport aExecutionReport);
        public event ProcessExecutionReport onExecutionReport;

        public delegate void ProcessTrade(Trade aTrade);
        public event ProcessTrade onTrade;

        public delegate void ProcessPosition(List<Position> aTrade);
        public event ProcessPosition onPositionChanged;

        Dictionary<string, Model.Snaphot> mSnaphots = new Dictionary<string, Model.Snaphot>();
    }
}
