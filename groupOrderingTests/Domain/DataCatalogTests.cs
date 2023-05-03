using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain.Tests
{
    [TestClass()]
    public class DataCatalogTests
    {
        private DataCatalog _dataCatalog;
        private const string SERVER_ID = "test";

        [TestInitialize()]
        public void Initialize()
        {
            _dataCatalog = new DataCatalog();
        }


        [TestMethod()]
        public void ListStoresTest()
        {
            List<Store> storeList = _dataCatalog.ListStores(SERVER_ID);
            Assert.AreNotEqual(0, storeList.Count);
        }

        [TestMethod()]
        public void ListAllOrdersTest()
        {
            List<GroupBuying> groupBuyingList = _dataCatalog.ListAllOrders(SERVER_ID);
            Assert.AreNotEqual(0, groupBuyingList.Count);
        }
    }
}