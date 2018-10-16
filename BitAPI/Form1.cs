using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BitAPI.Bitmex;
using BitAPI.Common;
using BitAPI.Trading;

namespace BitAPI
{
    public partial class Form1 : Form
    {        
        
        BitMexApi mBitmexApi = new BitMexApi("apikey", "secretkey", false);//bitmex
        
        System.IO.StreamWriter mLog = new System.IO.StreamWriter(string.Format("spread_{0:yyyMMdd}.log", DateTime.Now));
        Dictionary<string, Model.Snaphot> mSnapshot = new Dictionary<string, Model.Snaphot>();
        Strategy mStrategy = new Strategy("XBTUSD", "XBTM18");
        Model.Quote btcAsk = null;
        Model.Quote btcBid = null;
        Model.Quote xbtAsk = null;
        Model.Quote xbtBid = null;

        public Form1()
        {               
            InitializeComponent();
            mBitmexApi.onSnapshot += new BitMexApi.ProcessSnaphot(OnSnapshot);
            mStrategy.onRequest += new Strategy.OnRequest(OnRequest);
        }

        private void OnRequest(OrderRequest aOrderRequest)
        {
            mBitmexApi.sendOrder(aOrderRequest);

            mLog.WriteLine("NewOrder:{0}", aOrderRequest);
        }

        private void OnSnapshot(Model.Snaphot aSnapshot)
        {
            mSnapshot[aSnapshot.Symbol] = aSnapshot;
            string value = "";
            foreach (Model.Snaphot sn in mSnapshot.Values)
            {
                value = value + (value.Length == 0 ? "" : ";") + string.Format("{0};{1};{2};{3:MM/dd/yyyy H:mm:ss.ffff}", sn.Symbol, sn.BestBid.Price, sn.BestAsk.Price, sn.TimeStamp);
            }
            mLog.WriteLine(value);
            mStrategy.OnSnapshot(aSnapshot);
        }

        private void show()
        {
            /*lblBTC.Text = "0 / 0";
            lblXBT.Text = "0 / 0";
            lblSPREAD.Text = "0";

            if (btcAsk != null && btcBid != null)
            {
                lblBTC.Text = btcBid.Price + " / " + btcAsk.Price;
            }
            if (xbtAsk != null && xbtBid != null)
            {
                lblXBT.Text = xbtBid.Price + " / " + xbtAsk.Price;
            }
            if (btcAsk != null && btcBid != null && xbtAsk != null && xbtBid != null)
            {
                lblSPREAD.Text = string.Format("{0}", (btcBid.Price + btcAsk.Price)/2 - (xbtBid.Price + xbtAsk.Price)/2);
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {            
        }





        private void timer1_Tick(object sender, EventArgs e)
        {
            button1_Click(null, null);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mBitmexApi.Start();
            mBitmexApi.SubscribeBySymbol("XBTUSD");
            mBitmexApi.SubscribeBySymbol("XBTM18");           
            mBitmexApi.subscribeExecution();
            mBitmexApi.subscribePosition();
            mBitmexApi.subscribeOrder();
            mBitmexApi.subscribeMargin();            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrderRequest aRequest = new OrderRequest();
            //aRequest.Id = "order1";
            aRequest.OrderType = OrdTypeT.MARKET;
            aRequest.Side = SideT.SELL;
            aRequest.Size = 1m;
            aRequest.Symbol = "XBTUSD";
            aRequest.TimeInForce = TimeInForceT.GTC;
            aRequest.Price = 20000;
            aRequest.Type = OrderRequestTypeT.NEW_ORDER;

            mBitmexApi.sendOrder(aRequest);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OrderRequest aRequest = new OrderRequest();
            //aRequest.Id = "order1";
            aRequest.ClOrdId = "20171127003244-1";
            aRequest.Type = OrderRequestTypeT.ORDER_CANCEL;

            mBitmexApi.sendOrder(aRequest);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var p = mBitmexApi.getPositions();
        }

        private void Start()
        {
            BitMexApi mApi = new BitMexApi("", "", true);
            //Engine engine = new Engine(mApi);
            //engine.AddStrategy(new MarketMakingStrategy("BTCUSD", 3, 0.01));
            //engine.Start();
        }
    }
}


