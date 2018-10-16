using BitAPI.Bitmex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BitApiTest
{
    
    
    /// <summary>
    ///Это класс теста для UtilsCryptoTest, в котором должны
    ///находиться все модульные тесты UtilsCryptoTest
    ///</summary>
    [TestClass()]
    public class UtilsCryptoTest
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
        ///Тест для GetSignature
        ///</summary>
        [TestMethod()]
        public void GetSignatureTest()
        {
            string aApiSecret = "chNOOS4KvNXR_Xq4k4c9qsfoKWvnDecLATCRlcBwyKDYnWgO";
            string aMessage = "POST/api/v1/order1429631577995{\"symbol\":\"XBTM15\",\"price\":219.0,\"clOrdID\":\"mm_bitmex_1a/oemUeQ4CAJZgP3fjHsA\",\"orderQty\":98}";
            string expected = "93912e048daa5387759505a76c28d6e92c6a0d782504fc9980f4fb8adfc13e25";
            string actual;
            actual = UtilsCrypto.GetSignature(aApiSecret, aMessage);
            Assert.AreEqual(expected, actual);            
        }

        /// <summary>
        ///Тест для GetNonce
        ///</summary>
        [TestMethod()]
        public void GetNonceTest()
        {
            long expected = 0; // TODO: инициализация подходящего значения
            long actual;
            actual = UtilsCrypto.GetNonce();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для GetHmacsha256
        ///</summary>
        [TestMethod()]
        public void GetHmacsha256Test()
        {
            byte[] keyByte = null; // TODO: инициализация подходящего значения
            byte[] messageBytes = null; // TODO: инициализация подходящего значения
            byte[] expected = null; // TODO: инициализация подходящего значения
            byte[] actual;
            actual = UtilsCrypto.GetHmacsha256(keyByte, messageBytes);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }

        /// <summary>
        ///Тест для ByteArrayToString
        ///</summary>
        [TestMethod()]
        public void ByteArrayToStringTest()
        {
            byte[] ba = null; // TODO: инициализация подходящего значения
            string expected = string.Empty; // TODO: инициализация подходящего значения
            string actual;
            actual = UtilsCrypto.ByteArrayToString(ba);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
    }
}
