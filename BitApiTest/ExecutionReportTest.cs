using BitAPI.Bitmex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BitApiTest
{
    
    
    /// <summary>
    ///Это класс теста для ExecutionReportTest, в котором должны
    ///находиться все модульные тесты ExecutionReportTest
    ///</summary>
    [TestClass()]
    public class ExecutionReportTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Тест для ReadFromJObject
        ///</summary>
        [TestMethod()]
        public void ERBuyLimitNewTest()
        {
            string jsonMessage = "{\"orderID\":\"d69a6918-e8f6-915d-06c3-3efeb0d9355c\",\"clOrdID\":\"order1\",\"clOrdLinkID\":\"\",\"account\":13493,\"symbol\":\"XBTUSD\",\"side\":\"Buy\",\"simpleOrderQty\":null,\"orderQty\":1,\"price\":7000,\"displayQty\":null,\"stopPx\":null,\"pegOffsetValue\":null,\"pegPriceType\":\"\",\"currency\":\"USD\",\"settlCurrency\":\"XBt\",\"ordType\":\"Limit\",\"timeInForce\":\"GoodTillCancel\",\"execInst\":\"\",\"contingencyType\":\"\",\"exDestination\":\"XBME\",\"ordStatus\":\"New\",\"triggered\":\"\",\"workingIndicator\":true,\"ordRejReason\":\"\",\"simpleLeavesQty\":0.0001,\"leavesQty\":1,\"simpleCumQty\":0,\"cumQty\":0,\"avgPx\":null,\"multiLegReportingType\":\"SingleSecurity\",\"text\":\"Submitted via API.\",\"transactTime\":\"2017-11-17T23:40:48.166Z\",\"timestamp\":\"2017-11-17T23:40:48.166Z\"}";
            JToken json = JObject.Parse(jsonMessage);
            ExecutionReport expected = new ExecutionReport() 
            {
                OrderId = "d69a6918-e8f6-915d-06c3-3efeb0d9355c",
                Symbol = "XBTUSD",
                ClOrdId = "order1",
                Account = 13493,
                Side = BitAPI.Common.SideT.BUY,
                OrderQty = 1,
                Price = 7000,
                SettlCurrency = "XBt",
                OrdType = BitAPI.Common.OrdTypeT.LIMIT,
                TimeInForce = BitAPI.Common.TimeInForceT.GTC,
                OrdStatus = BitAPI.Common.OrdStatusT.NEW,
                LeavesQty = 1,
                Currency = "USD",
                TransactTime = new DateTime(2017, 11, 18, 23,40,48,166)
            };
            ExecutionReport actual = ExecutionReport.ReadFromJObject(json);
            AreEqual(expected, actual);            
        }

        [TestMethod()]
        public void ERSellMarketFilledTest()
        {
            string jsonMessage = "{\"orderID\":\"36a8e290-bd8e-860c-4377-b1d424bea9d3\",\"clOrdID\":\"20171119190856-3\",\"clOrdLinkID\":\"\",\"account\":13493,\"symbol\":\"XBTUSD\",\"side\":\"Sell\",\"simpleOrderQty\":null,\"orderQty\":1,\"price\":7744.5,\"displayQty\":null,\"stopPx\":null,\"pegOffsetValue\":null,\"pegPriceType\":\"\",\"currency\":\"USD\",\"settlCurrency\":\"XBt\",\"ordType\":\"Market\",\"timeInForce\":\"ImmediateOrCancel\",\"execInst\":\"\",\"contingencyType\":\"\",\"exDestination\":\"XBME\",\"ordStatus\":\"Filled\",\"triggered\":\"\",\"workingIndicator\":false,\"ordRejReason\":\"\",\"simpleLeavesQty\":0,\"leavesQty\":0,\"simpleCumQty\":0.00012943,\"cumQty\":1,\"avgPx\":7745,\"multiLegReportingType\":\"SingleSecurity\",\"text\":\"Submitted via API.\",\"transactTime\":\"2017-11-19T16:06:00.241Z\",\"timestamp\":\"2017-11-19T16:06:00.241Z\"}";
            JToken json = JObject.Parse(jsonMessage);
            ExecutionReport expected = new ExecutionReport()
            {
                OrderId = "36a8e290-bd8e-860c-4377-b1d424bea9d3",
                Symbol = "XBTUSD",
                ClOrdId = "20171119190856-3",
                Account = 13493,
                Side = BitAPI.Common.SideT.SELL,
                OrderQty = 1,
                Price = 7744.5,
                AvgPx = 7745,
                SettlCurrency = "XBt",
                OrdType = BitAPI.Common.OrdTypeT.MARKET,
                TimeInForce = BitAPI.Common.TimeInForceT.IOC,
                OrdStatus = BitAPI.Common.OrdStatusT.FILLED,
                CumQty = 1,
                Currency = "USD",
                TransactTime = new DateTime(2017, 11, 18, 23, 40, 48, 166)
            };
            ExecutionReport actual = ExecutionReport.ReadFromJObject(json);
            AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ExecutionReportAfterOrderCancelTest()
        {
            string jsonMessage = "[{\"orderID\":\"b5a66750-a8ce-2df1-49b2-cf6892e5f3f4\",\"clOrdID\":\"20171126233602-1\",\"clOrdLinkID\":\"\",\"account\":13493,\"symbol\":\"XBTUSD\",\"side\":\"Buy\",\"simpleOrderQty\":null,\"orderQty\":1,\"price\":10000,\"displayQty\":null,\"stopPx\":null,\"pegOffsetValue\":null,\"pegPriceType\":\"\",\"currency\":\"USD\",\"settlCurrency\":\"XBt\",\"ordType\":\"Limit\",\"timeInForce\":\"GoodTillCancel\",\"execInst\":\"\",\"contingencyType\":\"\",\"exDestination\":\"XBME\",\"ordStatus\":\"Filled\",\"triggered\":\"\",\"workingIndicator\":false,\"ordRejReason\":\"\",\"simpleLeavesQty\":0,\"leavesQty\":0,\"simpleCumQty\":0.00010752,\"cumQty\":1,\"avgPx\":9300.5,\"multiLegReportingType\":\"SingleSecurity\",\"text\":\"Submitted via API.\",\"transactTime\":\"2017-11-26T20:32:59.626Z\",\"timestamp\":\"2017-11-26T20:32:59.626Z\",\"error\":\"Unable to cancel order due to existing state: Filled\"}]";
            JArray arr = JArray.Parse(jsonMessage);            
            ExecutionReport expected = new ExecutionReport()
            {
                OrderId = "b5a66750-a8ce-2df1-49b2-cf6892e5f3f4",
                Symbol = "XBTUSD",
                ClOrdId = "20171126233602-1",
                Account = 13493,
                Side = BitAPI.Common.SideT.BUY,
                OrderQty = 1,
                Price = 10000,
                AvgPx = 9300.5,
                SettlCurrency = "XBt",
                OrdType = BitAPI.Common.OrdTypeT.LIMIT,
                TimeInForce = BitAPI.Common.TimeInForceT.GTC,
                OrdStatus = BitAPI.Common.OrdStatusT.FILLED,
                CumQty = 1,
                Currency = "USD",
                TransactTime = new DateTime(2017, 11, 18, 23, 40, 48, 166),
                Error = "Unable to cancel order due to existing state: Filled"
            };
            List<ExecutionReport> actual = ExecutionReport.ReadFromJArray(arr);
            AreEqual(expected, actual[0]);
        }
        //canceled "[{\"orderID\":\"3b9247ce-60da-16f6-29a6-62c3ca5e03ce\",\"clOrdID\":\"20171127002232-1\",\"clOrdLinkID\":\"\",\"account\":13493,\"symbol\":\"XBTUSD\",\"side\":\"Sell\",\"simpleOrderQty\":null,\"orderQty\":1,\"price\":10000,\"displayQty\":null,\"stopPx\":null,\"pegOffsetValue\":null,\"pegPriceType\":\"\",\"currency\":\"USD\",\"settlCurrency\":\"XBt\",\"ordType\":\"Limit\",\"timeInForce\":\"GoodTillCancel\",\"execInst\":\"\",\"contingencyType\":\"\",\"exDestination\":\"XBME\",\"ordStatus\":\"Canceled\",\"triggered\":\"\",\"workingIndicator\":false,\"ordRejReason\":\"\",\"simpleLeavesQty\":0,\"leavesQty\":0,\"simpleCumQty\":0,\"cumQty\":0,\"avgPx\":null,\"multiLegReportingType\":\"SingleSecurity\",\"text\":\"Canceled: cancel order by id\\nSubmitted via API.\",\"transactTime\":\"2017-11-26T21:19:24.645Z\",\"timestamp\":\"2017-11-26T21:20:37.546Z\"}]"

        private void AreEqual(ExecutionReport aLeft, ExecutionReport aRight)
        {
            Assert.AreEqual(aLeft.TimeInForce, aRight.TimeInForce);
            Assert.AreEqual(aLeft.ClOrdId, aRight.ClOrdId);
            Assert.AreEqual(aLeft.ExecId, aRight.ExecId);
            Assert.AreEqual(aLeft.Currency, aRight.Currency);
            Assert.AreEqual(aLeft.OrderId, aRight.OrderId);
            Assert.AreEqual(aLeft.Price, aRight.Price);
            Assert.AreEqual(aLeft.Error, aRight.Error);
            //Assert.AreEqual(aLeft.Commission, aRight.Commission);
            Assert.AreEqual(aLeft.ExecCost, aRight.ExecCost);
            Assert.AreEqual(aLeft.ExecCom, aRight.ExecCom);
            //Assert.AreEqual(aLeft.SimpleCumQty, aRight.SimpleCumQty);
            Assert.AreEqual(aLeft.OrderQty , aRight.OrderQty);
            Assert.AreEqual(aLeft.Side , aRight.Side);
            Assert.AreEqual(aLeft.CumQty , aRight.CumQty);
            Assert.AreEqual(aLeft.Symbol , aRight.Symbol);
            Assert.AreEqual(aLeft.ClOrdId , aRight.ClOrdId);
            Assert.AreEqual(aLeft.Account , aRight.Account);
            Assert.AreEqual(aLeft.OrdStatus , aRight.OrdStatus);
            Assert.AreEqual(aLeft.SettlCurrency , aRight.SettlCurrency);
            Assert.AreEqual(aLeft.LastQty, aRight.LastQty);
            Assert.AreEqual(aLeft.LeavesQty, aRight.LeavesQty);
            Assert.AreEqual(aLeft.LastPx, aRight.LastPx);           
        }
        
        [TestMethod()]
        public void ShouldParseExecutionReportFilledTest()
        {
            //{"table":"execution","action":"insert","data":[{"execID":"2723c53c-84cf-353b-e7fa-053b2ecb7326","orderID":"7e7ec9f4-ec40-fdef-7ef4-da4834edcb08","clOrdID":"20171129002338-4","clOrdLinkID":"","account":13493,"symbol":"XBTZ17","side":"Sell","lastQty":1,"lastPx":10082,"underlyingLastPx":null,"lastMkt":"XBME","lastLiquidityInd":"RemovedLiquidity","simpleOrderQty":null,"orderQty":1,"price":10082,"displayQty":null,"stopPx":null,"pegOffsetValue":null,"pegPriceType":"","currency":"USD","settlCurrency":"XBt","execType":"Trade","ordType":"Market","timeInForce":"ImmediateOrCancel","execInst":"","contingencyType":"","exDestination":"XBME","ordStatus":"Filled","triggered":"","workingIndicator":false,"ordRejReason":"","simpleLeavesQty":0,"leavesQty":0,"simpleCumQty":0.00011261,"cumQty":1,"avgPx":10081.5,"commission":0.00075,"tradePublishIndicator":"PublishTrade","multiLegReportingType":"SingleSecurity","text":"Submitted via API.","trdMatchID":"b18bbd6a-8935-7c95-2e57-124c14e1b5a7","execCost":9919,"execComm":7,"homeNotional":-0.00009919,"foreignNotional":1,"transactTime":"2017-11-28T21:20:28.661Z","timestamp":"2017-11-28T21:20:28.661Z"}]}
            string jsonMessage = "[{\"execID\":\"2723c53c-84cf-353b-e7fa-053b2ecb7326\",\"orderID\":\"7e7ec9f4-ec40-fdef-7ef4-da4834edcb08\",\"clOrdID\":\"20171129002338-4\",\"clOrdLinkID\":\"\",\"account\":13493,\"symbol\":\"XBTZ17\",\"side\":\"Sell\",\"lastQty\":1,\"lastPx\":10082,\"underlyingLastPx\":null,\"lastMkt\":\"XBME\",\"lastLiquidityInd\":\"RemovedLiquidity\",\"simpleOrderQty\":null,\"orderQty\":1,\"price\":10082,\"displayQty\":null,\"stopPx\":null,\"pegOffsetValue\":null,\"pegPriceType\":\"\",\"currency\":\"USD\",\"settlCurrency\":\"XBt\",\"execType\":\"Trade\",\"ordType\":\"Market\",\"timeInForce\":\"ImmediateOrCancel\",\"execInst\":\"\",\"contingencyType\":\"\",\"exDestination\":\"XBME\",\"ordStatus\":\"Filled\",\"triggered\":\"\",\"workingIndicator\":false,\"ordRejReason\":\"\",\"simpleLeavesQty\":0,\"leavesQty\":0,\"simpleCumQty\":0.00011261,\"cumQty\":1,\"avgPx\":10081.5,\"commission\":0.00075,\"tradePublishIndicator\":\"PublishTrade\",\"multiLegReportingType\":\"SingleSecurity\",\"text\":\"Submitted via API.\",\"trdMatchID\":\"b18bbd6a-8935-7c95-2e57-124c14e1b5a7\",\"execCost\":9919,\"execComm\":7,\"homeNotional\":-0.00009919,\"foreignNotional\":1,\"transactTime\":\"2017-11-28T21:20:28.661Z\",\"timestamp\":\"2017-11-28T21:20:28.661Z\"}]";
            JArray arr = JArray.Parse(jsonMessage);
            ExecutionReport expected = new ExecutionReport()
            {
                ExecId = "2723c53c-84cf-353b-e7fa-053b2ecb7326",
                OrderId = "7e7ec9f4-ec40-fdef-7ef4-da4834edcb08",
                Symbol = "XBTZ17",
                ClOrdId = "20171129002338-4",
                Account = 13493,
                Side = BitAPI.Common.SideT.SELL,
                OrderQty = 1,
                Price = 10082,
                AvgPx = 10081.5,
                SettlCurrency = "XBt",
                OrdType = BitAPI.Common.OrdTypeT.MARKET,
                TimeInForce = BitAPI.Common.TimeInForceT.IOC,
                OrdStatus = BitAPI.Common.OrdStatusT.FILLED,
                ExecType = BitAPI.Common.ExecTypeT.TRADE,
                CumQty = 1,
                Currency = "USD",
                TransactTime = new DateTime(2017, 11, 18, 23, 40, 48, 166),
                Error = "",
                LastPx = 10082,
                LastQty = 1,
                LeavesQty = 0,
                Commission = 0.00075,
                TrdMatchId = "b18bbd6a-8935-7c95-2e57-124c14e1b5a7",
                Text = "Submitted via API.",
                //HomeNotional = -0.00009919,
                //ForeignNotional = 1,
                ExecCost = 9919,
                ExecCom = 7,
                SimpleCumQty = 0.00011261,
            };
            List<ExecutionReport> actual = ExecutionReport.ReadFromJArray(arr);
            AreEqual(expected, actual[0]);
        }



        [TestMethod()]
        public void ParsePositionTest()
        {
            //{"table":"position","action":"update","data":[{"account":13493,"symbol":"XBTUSD","currency":"XBt","currentTimestamp":"2017-11-27T22:08:55.108Z","currentQty":1,"markPrice":9625.13,"markValue":-10389,"riskValue":53247,"homeNotional":0.00010389,"maintMargin":480,"unrealisedGrossPnl":363,"taxBase":363,"unrealisedPnl":363,"unrealisedPnlPcnt":0.0338,"unrealisedRoePcnt":3.3761,"simpleQty":0.0001,"liquidationPrice":10,"timestamp":"2017-11-27T22:08:55.108Z","lastPrice":9625.13,"lastValue":-10389}]}

            string jsonMessage = "[{\"account\":13493,\"symbol\":\"XBTUSD\",\"currency\":\"XBt\",\"currentTimestamp\":\"2017-11-27T22:08:55.108Z\",\"currentQty\":1,\"markPrice\":9625.13,\"markValue\":-10389,\"riskValue\":53247,\"homeNotional\":0.00010389,\"maintMargin\":480,\"unrealisedGrossPnl\":363,\"taxBase\":363,\"unrealisedPnl\":363,\"unrealisedPnlPcnt\":0.0338,\"unrealisedRoePcnt\":3.3761,\"simpleQty\":0.0001,\"liquidationPrice\":10,\"timestamp\":\"2017-11-27T22:08:55.108Z\",\"lastPrice\":9625.13,\"lastValue\":-10389}]";
            JArray arr = JArray.Parse(jsonMessage);
            Position expected = new Position()
            {                
                Symbol = "XBTUSD",                
                Account = 13493,                
                Currency = "XBt",
                CurrentQty = 1,
                RiskValue = 53247,
                HomeNotional = 0.00010389f,
                MaintMargin = 480,
                TaxBase = 363,
                MarkPrice = 9625.13,
                UnrealisedPnl = 363,
                UnrealizedPnlPcnt = 0.0338f,
                SimpleQty = 0.0001f,
                LastPrice = 9625.13,
                LastValue = -10389,
                Timestamp = new DateTime(2017,11,27,22,8,55,108)
            };
            List<Position> actual = Position.ReadFromJArray(arr);
            AreEqualPositions(expected, actual[0]);
        }

        private void AreEqualPositions(Position aLeft, Position aRight)
        {
            Assert.AreEqual(aLeft.Account, aRight.Account);
            Assert.AreEqual(aLeft.Currency, aRight.Currency);
            Assert.AreEqual(aLeft.Symbol, aRight.Symbol);
            Assert.AreEqual(aLeft.CurrentQty, aRight.CurrentQty);
            Assert.AreEqual(aLeft.LastPrice, aRight.LastPrice);
            Assert.AreEqual(aLeft.LastValue, aRight.LastValue);
            Assert.AreEqual(aLeft.Timestamp, aRight.Timestamp);
            Assert.AreEqual(aLeft.TaxBase, aRight.TaxBase);
            Assert.AreEqual(aLeft.UnrealisedPnl, aRight.UnrealisedPnl);            
        }
    }
}
