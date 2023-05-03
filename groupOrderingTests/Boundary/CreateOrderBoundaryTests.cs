using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupOrdering.Domain;
using groupOrdering.Technical;

namespace groupOrdering.Boundary.Tests
{
    [TestClass()]
    public class CreateOrderBoundaryTests
    {
        private CreateOrderBoundary _createOrderBoundary;

        [TestInitialize()]
        public void Initialize()
        {
            _createOrderBoundary = new CreateOrderBoundary();
        }

        [TestMethod()]
        public void GetStoreTest()
        {
            Store store = _createOrderBoundary.GetStore("1", "test");
            Assert.AreEqual("1", store.StoreID);
            Assert.AreEqual("7-11", store.StoreName);
            Assert.AreEqual("台北市", store.StoreAddress);
            Assert.AreEqual("0909000000", store.StorePhoneNumber);
        }

        [TestMethod()]
        public void ListStoresTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PublishGroupBuyingTest()
        {
            Assert.Fail();
        }
    }
}