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
    public class StoreTests
    {
        private Store _store;

        [TestInitialize()]
        public void Initialize() 
        {
            _store = new Store();
        }


        [TestMethod()]
        public void SetStoreTest()
        {
            const int STORE_ID = 1;
            _store.SetStore(STORE_ID);
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