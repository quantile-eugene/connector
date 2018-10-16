using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;


namespace BitAPI.Common
{
    public class DataSaver
    {        
        public class Item
        {
            public DateTime Time;
            public decimal Value;
        };

        TimeSpan mTimeFrame;
        Timer mTimer;
        decimal mCurrent = -1;
        StreamWriter mFile;
        List<Item> mData = new List<Item>();

        public delegate void OnData(Item aItem);
        public event OnData onData;

        public DataSaver(TimeSpan aTimeFrame)
        {
            string fileName = string.Format("data.txt");
            load(fileName);
            mFile = new StreamWriter(fileName, true);
            mTimeFrame = aTimeFrame;
            mTimer = new Timer(OnTimer, null, 0, (int)mTimeFrame.TotalMilliseconds);            
        }

        private void load(string mFileName)
        {
            StreamReader read = new StreamReader(mFileName);
            if (read != null)
            {
                string str;
                while ((str = read.ReadLine()) != null)
                {
                    string[] values = str.Split(';');
                    Item item = new Item() { Time = DateTime.Parse(values[0]), Value = decimal.Parse(values[1]) };
                    mData.Add(item);
                }
                read.Close();
            }
        }

        public void SetCurrent(decimal aCurrent)
        {
            mCurrent = aCurrent;
        }

        public void OnTimer(Object aObject)
        {
            if (mCurrent > 0)
            {
                Item item = new Item() { Time = DateTime.Now, Value = mCurrent };
                mData.Add(item);
                onData(item);

                mFile.WriteLine(string.Format("{0};{1}", item.Time, item.Value));
                mFile.Flush();
            }
        }

        public List<Item> getData(int aCount)
        {
            if (mData.Count < aCount)
            {
                return mData;
            }
            else
            {
                int index = mData.Count - aCount;
                return mData.GetRange(index, aCount);
            }
        }
    }
}
