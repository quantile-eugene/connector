using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI
{
    public abstract class IConnector
    {
        public abstract void Start();
        public abstract void Stop();

        public delegate void ProcessSnaphot(Model.Snaphot aSnapshot);
        public event ProcessSnaphot onSnapshot;
    }
}
