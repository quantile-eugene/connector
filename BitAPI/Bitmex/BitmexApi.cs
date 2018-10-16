using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.Collections;
using System;
using BitAPI.Common;


namespace BitAPI.Bitmex
{
    public class BitMexApi : IConnector
    {
        System.IO.StreamWriter mLog = new System.IO.StreamWriter(string.Format("bitmex_{0:yyyyMMdd}.log", DateTime.Now), true);
        BitmexMarketManager mMarketManager;
        BitmexOrderManager mOrderManager;
        GeneratorId mGenerator = new GeneratorId();
                
        string mApiKey = "";
        string mApiSecret = "";        
        Dictionary<string, OrderRequest> mOrderRequests = new Dictionary<string, OrderRequest>();

        public BitMexApi(string aApiKey, string aApiSecret, bool aIsDemo = false)
        {
            string url = "";
            mApiKey = aApiKey;
            mApiSecret = aApiSecret;            
            if (aIsDemo)
            {
                url = "testnet.bitmex.com";
            }
            else
            {
                url = "www.bitmex.com/realtime";
            }
            
            mMarketManager = new BitmexMarketManager(mLog, mApiKey, mApiSecret, url);
            mMarketManager.onSnapshot += new BitmexMarketManager.ProcessSnaphot(OnSnapshot);
            mOrderManager = new BitmexOrderManager(mLog, mApiKey, mApiSecret, url);
            
        }

        public override void Start()
        {
            mMarketManager.connect();
            mOrderManager.connect();
        }

        public override void Stop()
        {
            mMarketManager.disconnect();
            mOrderManager.disconnect();
        }

        private void OnSnapshot(Model.Snaphot aSnapshot)
        {
            if (onSnapshot != null)
            {
                onSnapshot(aSnapshot);
            }
        }

        public void SubscribeBySymbol(string aSymbol)
        {
            subscribeQuote(aSymbol);
            subscribeTrade(aSymbol);
            subscribeInstrument(aSymbol);
            subscribeOrderBook(aSymbol);
        }

        private void subscribeQuote(string aSymbol)
        {
            mMarketManager.subscribeQuote(aSymbol);
        }

        private void subscribeTrade(string aSymbol)
        {
            mMarketManager.subscribeTrade(aSymbol);
        }

        private void subscribeInstrument(string aSymbol)
        {
            mMarketManager.subscribeInstrument(aSymbol);
        }

        private void subscribeOrderBook(string aSymbol)
        {
            mMarketManager.subscribeOrderBook(aSymbol);
        }

        public void subscribePosition()
        {
            mMarketManager.subscribePosition("");
        }

        public void subscribeExecution()        
        {
            mMarketManager.subscribeExecution("");
        }

        public void subscribeOrder()
        {
            mMarketManager.subscribeOrder("");
        }

        public void subscribeMargin()
        {
            mMarketManager.subscribeMargin("");
        }

        public void sendOrder(OrderRequest aRequest)
        {
            if (aRequest.Id == "" || aRequest.Id == null)
            {
                aRequest.Id = mGenerator.GetNext();
            }

            mOrderRequests[aRequest.Id] = aRequest;
            if (mOrderManager != null)
            {
                mOrderManager.sendOrder(aRequest);
            }
        }

        public List<Position> getPositions()
        {
            return mOrderManager.getPositions();
        }

        public delegate void ProcessSnaphot(Model.Snaphot aSnapshot);
        public event ProcessSnaphot onSnapshot;
    }
}
