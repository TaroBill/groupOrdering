using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using groupOrdering.Boundary;

namespace groupOrdering.Domain.Tests
{
    [TestClass()]
    public class StoreTests
    {
        private Store _store;

        [TestInitialize()]
        public void Initialize()
        {
            var mockStoresBoundary = new Mock<IStoresBoundary>();
            mockStoresBoundary.Setup(p => p.ListStores("test")).Returns(new List<Store>()
            {
                new Store("1", "7-11", "台北市", "0909000000"),
                new Store("2", "全家", "我家對面", "0911111111")
            });
            mockStoresBoundary.Setup(p => p.GetStore("1", "test")).Returns(new Store("1", "7-11", "台北市", "0909000000"));
           
            _store = new Store();
            _store.SetStoresBoundary(mockStoresBoundary.Object);
        }


        [TestMethod()]
        public void SetStoreTest()
        {
            const string STORE_ID = "1";
            const string SERVER_ID = "test";
            _store.SetStore(STORE_ID, SERVER_ID);
            Assert.AreEqual(STORE_ID, _store.StoreID);
            Assert.AreEqual("7-11", _store.StoreName);
            Assert.AreEqual("台北市", _store.StoreAddress);
            Assert.AreEqual("0909000000", _store.StorePhoneNumber);
        }

        [TestMethod()]
        public void CreateMenuTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddStoreItemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EndBuildStoreTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListItemsOfStoreTest()
        {
            Assert.Fail();
        }
    }
}