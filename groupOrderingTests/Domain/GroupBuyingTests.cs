using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupOrdering.Boundary;
using Moq;
using Discord;
using System.Reflection.Metadata.Ecma335;

namespace groupOrdering.Domain.Tests
{
    [TestClass()]
    public class GroupBuyingTests
    {
        private GroupBuying _groupBuying;

        private const string USER_ID = "Tester";
        private const string STORE_ID = "1";
        private const string SERVER_ID = "test";
        private User _user;

        [TestInitialize()]
        public void Initialize()
        {
            var mockGroupBuyingsBoundary = new Mock<IGroupBuyingsBoundary>();
            var mockStoresBoundary = new Mock<IStoresBoundary>();
            StoreItem storeItem1 = new StoreItem("1", "item1", 10);
            StoreItem storeItem2 = new StoreItem("2", "item2", 20);
            mockStoresBoundary.Setup(p => p.ListItemsOfStore("1")).Returns(new List<StoreItem> { storeItem1, storeItem2 });
            _user = new User(USER_ID);

            var store = new Store("1", "testStore", "address", "090000000");
            store.SetStoresBoundary(mockStoresBoundary.Object);
            store.AddStoreItem(storeItem1);
            store.AddStoreItem(storeItem2);
            mockGroupBuyingsBoundary.Setup(p => p.GetStoreByGroupbuyingID("1")).Returns(store);
            mockGroupBuyingsBoundary.Setup(p => p.GetStoreByGroupbuyingID("2")).Returns(store);
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying(STORE_ID, SERVER_ID, DateTime.Today, USER_ID, "")).Returns(1);

            GroupBuying groupBuying1 = new GroupBuying(mockGroupBuyingsBoundary.Object, "1", "測試團購一", SERVER_ID);
            GroupBuying groupBuying2 = new GroupBuying(mockGroupBuyingsBoundary.Object, "2", "測試團購二", SERVER_ID);
            mockGroupBuyingsBoundary.Setup(p => p.ListAllOrders(SERVER_ID)).Returns(new List<GroupBuying>()
            {
                groupBuying1,
                groupBuying2
            });
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying(STORE_ID, SERVER_ID, DateTime.Today, USER_ID, "")).Returns(1);
            mockGroupBuyingsBoundary.Setup(p => p.GetGroupBuyingByGroupID("1")).Returns(groupBuying1);
            _groupBuying = new GroupBuying(mockGroupBuyingsBoundary.Object, "1");
            _groupBuying.SetStore(store);
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
        public void JoinOrderTest()
        {
            _groupBuying.GetMemberOrder(_user);
            MemberOrder memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual("", memberOrder.UserID);

            _groupBuying.JoinOrder(_user);
            _groupBuying.GetMemberOrder(_user);
            memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual(USER_ID, memberOrder.UserID);
        }

        [TestMethod()]
        public void AddItemTest()
        {
            _groupBuying.JoinOrder(_user);
            _groupBuying.AddItem(_user, "1", 5);
            MemberOrder memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 5\n", memberOrder.OrderToString());
        }

        [TestMethod()]
        public void EditItemTest()
        {
            _groupBuying.JoinOrder(_user);
            _groupBuying.AddItem(_user, "1", 5);
            MemberOrder memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 5\n", memberOrder.OrderToString());

            _groupBuying.EditItem(_user, "1", 6);
            memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 6\n", memberOrder.OrderToString());
        }

        [TestMethod()]
        public void DeleteItemTest()
        {
            _groupBuying.JoinOrder(_user);
            _groupBuying.AddItem(_user, "1", 5);
            MemberOrder memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 5\n", memberOrder.OrderToString());

            _groupBuying.DeleteItem(_user, "1");
            memberOrder = _groupBuying.GetMemberOrder(_user);
            Assert.AreEqual($"{USER_ID}\n", memberOrder.OrderToString());
        }

        [TestMethod()]
        public void GetTotalPriceTest()
        {
            _groupBuying.JoinOrder(_user);
            _groupBuying.AddItem(_user, "1", 5);
            Assert.AreEqual(50, _groupBuying.GetTotalPrice(_user));
        }
    }
}