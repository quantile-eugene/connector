using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAPI.Common;

namespace BitAPI.Trading
{
    public class Strategy
    {
        public enum State { BUY, SELL, CASH };
        GeneratorId mGenerator = new GeneratorId();
        private string mSymbolA;
        private string mSymbolB;
        private Model.Snaphot mSnapshotA = null;
        private Model.Snaphot mSnapshotB = null;
        DataSaver mSaver = null;
        int mCount = 20;
        double mLevelOpen = 2;
        double mLevelClose = 0.5;
        double mMean = 0;
        double mStd = 0;
        State mState = State.CASH;

        public delegate void OnRequest(OrderRequest aOrderRequest);
        public event OnRequest onRequest;

        public Strategy(string aSymbolA, string aSymbolB)
        {
            mSymbolA = aSymbolA;
            mSymbolB = aSymbolB;
            mSaver = new DataSaver(new TimeSpan(0, 1, 0));
            mSaver.onData +=new DataSaver.OnData(OnData);
        }

        public void OnSnapshot(Model.Snaphot aSnapshot)
        {
            if (aSnapshot.Symbol == mSymbolA)
            {
                mSnapshotA = aSnapshot;
            }
            else
            {
                mSnapshotB = aSnapshot;
            }
            Process();
        }

        private void OnData(DataSaver.Item aItem)
        {
            double mean = 0;
            double sum = 0;

            List<DataSaver.Item> items = mSaver.getData(mCount);

            foreach (DataSaver.Item item in items)
            {
                mean += (double)item.Value;
            }
            mean /= items.Count;
            mMean = mean;

            foreach (DataSaver.Item item in items)
            {
                sum += Math.Pow((double)(mean - (double)item.Value), 2);
            }
            sum /= items.Count - 1;
            sum = Math.Sqrt(sum);
            mStd = sum;
        }

        private void Process()
        {            
            if (mSnapshotA != null && mSnapshotB != null && mSnapshotB.MidPrice != 0)
            {
                double spread = (double)(mSnapshotA.MidPrice / mSnapshotB.MidPrice);
                mSaver.SetCurrent((decimal)spread);
                if (mState == State.CASH)
                {
                    if (spread > mMean + mLevelOpen * mStd)
                    {
                        mState = State.SELL;
                        OrderRequest ordA = new OrderRequest() 
                        {
                            Symbol = mSymbolA,
                            Side = SideT.SELL,
                            Size = 1,
                            OrderType = OrdTypeT.MARKET,
                            Id = mGenerator.GetNext(), 
                            Type = OrderRequestTypeT.NEW_ORDER
                        };                       

                        OrderRequest ordB = new OrderRequest()
                        {
                            Symbol = mSymbolB,
                            Side = SideT.BUY,
                            Size = 1,
                            OrderType = OrdTypeT.MARKET,
                            Id = mGenerator.GetNext(),
                            Type = OrderRequestTypeT.NEW_ORDER
                        };

                        if (onRequest != null)
                        {
                            onRequest(ordA);
                            onRequest(ordB);
                        }
                        //sell A
                        //buy B
                    }
                    else if (spread < mMean - mLevelOpen * mStd)
                    {
                        mState = State.BUY;
                        //buy A
                        //sell B
                        OrderRequest ordA = new OrderRequest()
                        {
                            Symbol = mSymbolA,
                            Side = SideT.BUY,
                            Size = 1,
                            OrderType = OrdTypeT.MARKET,
                            Id = mGenerator.GetNext(),
                            Type = OrderRequestTypeT.NEW_ORDER
                        };

                        OrderRequest ordB = new OrderRequest()
                        {
                            Symbol = mSymbolB,
                            Side = SideT.SELL,
                            Size = 1,
                            OrderType = OrdTypeT.MARKET,
                            Id = mGenerator.GetNext(),
                            Type = OrderRequestTypeT.NEW_ORDER
                        };

                        if (onRequest != null)
                        {
                            onRequest(ordA);
                            onRequest(ordB);
                        }
                    }
                }
                else
                {
                    if (spread < mMean + mLevelClose * mStd && spread > mMean - mLevelOpen * mStd)
                    {
                        SideT dir = (mState == State.BUY) ? SideT.SELL : SideT.BUY;
                        OrderRequest ordA = new OrderRequest()
                        {
                            Symbol = mSymbolA,
                            Side = dir,
                            Size = 1,
                            OrderType = OrdTypeT.MARKET,
                            Id = mGenerator.GetNext(),
                            Type = OrderRequestTypeT.NEW_ORDER
                        };

                        OrderRequest ordB = new OrderRequest()
                        {
                            Symbol = mSymbolB,
                            Side = dir == SideT.BUY?SideT.SELL:SideT.BUY,
                            Size = 1,
                            OrderType = OrdTypeT.MARKET,
                            Id = mGenerator.GetNext(),
                            Type = OrderRequestTypeT.NEW_ORDER
                        };

                        if (onRequest != null)
                        {
                            onRequest(ordA);
                            onRequest(ordB);
                        }

                        mState = State.CASH;
                    }
                }                
            }
        }
    }
}
