using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI.Bitmex
{
    public class RequstSequenceNumber
    {
        long mNumber = 0;
        static string path = "request_seq.num";
        public RequstSequenceNumber()
        {
            load();
        }

        public long getNext()
        {
            return ++mNumber;
        }

        public void save()
        {
            System.IO.StreamWriter mFile = new System.IO.StreamWriter(path);
            mFile.WriteLine(string.Format("{0}", path));
            mFile.Close();
        }

        public void load()
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.StreamReader mFile = new System.IO.StreamReader(path);
                string value = mFile.ReadLine();
                if (value.Length > 0 && long.TryParse(value, out mNumber))
                {
                    return;
                }
            }
            mNumber = 1;
        }
    }
}
