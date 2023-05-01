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
    public class GroupBuyingTests
    {
        private GroupBuying _groupBuying;

        [TestInitialize()]
        public void Initialize()
        {
            _groupBuying = new GroupBuying();
        }


        [TestMethod()]
        public void GroupBuyingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ChooseExistStoreTest()
        {
            const int STORE_ID = 1;
            Store store = _groupBuying.GetStore();
            Assert.AreEqual(0, store.StoreID);

            _groupBuying.ChooseExistStore(STORE_ID);
            Assert.AreEqual(STORE_ID, store.StoreID);
        }

        [TestMethod()]
        public void SetEndTimeTest()
        {
            Assert.AreEqual(DateTime.Today, _groupBuying.EndTime);

            DateTime endTime = DateTime.Now;
            _groupBuying.SetEndTime(endTime);
            Assert.AreNotEqual(DateTime.Today, _groupBuying.EndTime);
            Assert.AreEqual(endTime, _groupBuying.EndTime);
        }

        [TestMethod()]
        public void SetGroupBuyingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateMemberOrderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EndGroupBuyingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListItemsOfStoreTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddItemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTotalPriceTest()
        {
            Assert.Fail();
        }
    }
}