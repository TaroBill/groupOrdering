using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupOrdering.Boundary;
using Moq;

namespace groupOrdering.Domain.Tests
{
    [TestClass()]
    public class GroupBuyingTests
    {
        private GroupBuying _groupBuying;

        [TestInitialize()]
        public void Initialize()
        {
            var mockGroupBuyingsBoundary = new Mock<IGroupBuyingsBoundary>();
            mockGroupBuyingsBoundary.Setup(p => p.ListAllOrders("test")).Returns(new List<GroupBuying>()
            {
                new GroupBuying(mockGroupBuyingsBoundary.Object, "測試團購一", "test"),
                new GroupBuying(mockGroupBuyingsBoundary.Object, "測試團購二", "test")
            });
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying("1", "test", DateTime.Today, "Tester", "")).Returns(1);
            _groupBuying = new GroupBuying(mockGroupBuyingsBoundary.Object);
        }


        [TestMethod()]
        public void GroupBuyingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ChooseExistStoreTest()
        {
            const string STORE_ID = "1";
            Store store = _groupBuying.GetStore();
            Assert.AreEqual("0", store.StoreID);

            _groupBuying.ChooseExistStore(STORE_ID, "test");
            Assert.AreEqual(STORE_ID, store.StoreID);
        }

        [TestMethod()]
        public void SetEndTimeTest()
        {
            Assert.AreEqual(DateTime.Today, _groupBuying.GetEndTime());

            DateTime endTime = DateTime.Now;
            _groupBuying.SetEndTime(endTime);
            Assert.AreNotEqual(DateTime.Today, _groupBuying.GetEndTime());
            Assert.AreEqual(endTime, _groupBuying.GetEndTime());
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

        [TestMethod()]
        public void PublishGroupBuyingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckEndTimeTest()
        {
            Assert.Fail();
        }
    }
}