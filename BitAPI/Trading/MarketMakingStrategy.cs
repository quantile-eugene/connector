using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;


namespace BitAPI.Trading
{
    public class MarketMakingStrategy : CommonStrategy
    {
        Bitmex.BitMexApi mApi = null;
        string mSymbol;
        double mQuotedVolume;
        double mSpread;
        double mSellOrders;
        double mBuyOrders;
        Model.Snaphot mSnapshot = null;
        Timer mTimer;
        Dictionary<string, Common.OrderRequest> mOrders = new Dictionary<string, Common.OrderRequest>();

        public MarketMakingStrategy(string aSymbol, double aQuotedVolume, double aSpread, string aApiKey, string aApiSecret)
        {
            mSpread = aSpread;
            mSymbol = aSymbol;
            mQuotedVolume = aQuotedVolume;

            mTimer = new Timer(30000);
            mTimer.Elapsed += OnTimer;

            mApi = new Bitmex.BitMexApi(aApiKey, aApiSecret, true);
            mApi.onSnapshot += OnSnapshot;
        }

        private void OnSnapshot(Model.Snaphot aSnapshot)
        {
            mSnapshot = aSnapshot;
        }

        public override void Start()
        {            
            mTimer.Start();
            mApi.Start();
            mApi.SubscribeBySymbol(mSymbol);
        }

        public override void Stop()
        {
            mTimer.Stop();
            CancelAllOrders();
            mOrders.Clear();
        }

        public void OnTimer(object sender, ElapsedEventArgs e)
        {
            Process();
        }

        public override void OnPositionChanged(Dictionary<string, double> aPosition)
        {
            Process();
        }

        public override void OnOrderBookChanged(Model.Snaphot aSnapshot)
        {
            mSnapshot = aSnapshot;
        }

        public override void OnOrderChanged(List<Bitmex.ExecutionReport> aReports)
        {
        }

        // private

        private void Process()
        {
            CancelAllOrders();

            if (mSnapshot != null && mSnapshot.MidPrice != decimal.Zero)
            {
                double delta = getDelta(mSymbol);
                decimal midPrice = mSnapshot.MidPrice;

                mSellOrders = -mQuotedVolume - delta;
                mBuyOrders = mQuotedVolume - delta;

                SendOrder(mSellOrders, midPrice);
                SendOrder(mBuyOrders, midPrice);
            }
        }

        private double getDelta(string aSymbol)
        {
            Bitmex.Position position = mApi.getPositions().Find(s => s.Symbol == aSymbol);
            if (position != null)
            {
                return mQuotedVolume - position.CurrentQty;
            }
            else
            {
                return mQuotedVolume;
            }
        }

        private void CancelAllOrders()
        {
            Common.OrderRequest order = new Common.OrderRequest()
            {
                Symbol = mSymbol,
                Type = Common.OrderRequestTypeT.ORDER_CANCEL
            };
            mApi.sendOrder(order);
            mOrders[order.ClOrdId] = order;
        }

        private void SendOrder(double aVolume, decimal aMidPrice)
        {
            if (aVolume == 0)
                return;

            Common.SideT side = (aVolume > 0) ? Common.SideT.BUY : Common.SideT.SELL;
            decimal price = (aMidPrice * (decimal)(1.0 + (side == Common.SideT.BUY ? 1 : -1) * mSpread));

            Common.OrderRequest order = new Common.OrderRequest()
            {                
                Symbol = mSymbol,
                Side = side,
                OrderType = Common.OrdTypeT.LIMIT,
                TimeInForce = Common.TimeInForceT.GTC,
                Price = price
            };

            mApi.sendOrder(order);
            mOrders[order.ClOrdId] = order;
        }
    }
}
