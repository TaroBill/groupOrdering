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

        private const string USER_ID = "Tester";
        private const string STORE_ID = "1";
        private const string SERVER_ID = "test";

        [TestInitialize()]
        public void Initialize()
        {
            var mockGroupBuyingsBoundary = new Mock<IGroupBuyingsBoundary>();
            GroupBuying groupBuying1 = new GroupBuying(mockGroupBuyingsBoundary.Object, "1", "測試團購一", SERVER_ID);
            GroupBuying groupBuying2 = new GroupBuying(mockGroupBuyingsBoundary.Object, "2", "測試團購二", SERVER_ID);
            Store store1 = new Store("1", "7-11", "台北市", "0909000000");
            Store store2 = new Store("2", "全家", "我家對面", "0911111111");
            groupBuying1.SetStore(store1);
            groupBuying2.SetStore(store2);
            mockGroupBuyingsBoundary.Setup(p => p.ListAllOrders(SERVER_ID)).Returns(new List<GroupBuying>()
            {
                groupBuying1,
                groupBuying2
            });
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying(STORE_ID, SERVER_ID, DateTime.Today, USER_ID, "")).Returns(1);
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