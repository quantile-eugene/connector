using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAPI.Trading;

namespace BitAPI
{
    public class Engine
    {
        IConnector mConnector;
        List<CommonStrategy> mStrategies = new List<CommonStrategy>();

        public Engine(IConnector aConnector)
        {
            mConnector = aConnector;
            //mConnector.OnSnapshot += OnSnapshot;
        }

        public void AddStrategy(CommonStrategy aStrategy)
        {
            mStrategies.Add(aStrategy);
        }

        public void Start()
        {
            mConnector.Start();
            foreach(CommonStrategy strategy in mStrategies)
            {
                strategy.Start();
            }
        }

        // private
        private void OnSnapshot(Model.Snaphot aSnapshot)
        {
            foreach (CommonStrategy strategy in mStrategies)
            {
                strategy.OnOrderBookChanged(aSnapshot);
            }
        }

    }
}
