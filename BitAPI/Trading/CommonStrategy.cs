using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAPI.Trading
{
    public abstract class CommonStrategy
    {
        public abstract void OnPositionChanged(Dictionary<string, double> aPosition);
        public abstract void OnOrderChanged(List<Bitmex.ExecutionReport> aExecutionReports);
        public abstract void OnOrderBookChanged(Model.Snaphot aSnapshot);
        public abstract void Start();
        public abstract void Stop();
    }
}
