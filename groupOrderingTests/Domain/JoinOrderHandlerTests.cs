using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using groupOrdering.Boundary;
using Moq;

namespace groupOrdering.Domain.Tests
{
    [TestClass()]
    public class JoinOrderHandlerTests
    {
        private JoinOrderHandler _joinOrderHandler;
        private User _user;

        private const string USER_ID = "Tester";
        private const string STORE_ID = "1";
        private const string SERVER_ID = "test";

        [TestInitialize()]
        public void Initialize()
        {
            _user = new User(USER_ID);
            _joinOrderHandler = new JoinOrderHandler();

            var mockGroupBuyingsBoundary = new Mock<IGroupBuyingsBoundary>();

            GroupBuying groupBuying1 = new GroupBuying(mockGroupBuyingsBoundary.Object, "1", "測試團購一", SERVER_ID);
            GroupBuying groupBuying2 = new GroupBuying(mockGroupBuyingsBoundary.Object, "2", "測試團購二", SERVER_ID);
            Store store1 = new Store("1", "7-11", "台北市", "0909000000");
            Store store2 = new Store("2", "全家", "我家對面", "0911111111");
            store1.AddStoreItem("御飯糰", 30);
            store1.AddStoreItem("茶葉蛋", 11);

            store2.AddStoreItem("香蕉", 30);
            store2.AddStoreItem("御茶園", 25);
            store2.AddStoreItem("脆迪酥", 100);
            groupBuying1.SetStore(store1);
            groupBuying2.SetStore(store2);
            mockGroupBuyingsBoundary.Setup(p => p.ListAllOrders(SERVER_ID)).Returns(new List<GroupBuying>()
            {
                groupBuying1,
                groupBuying2
            });
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying(STORE_ID, SERVER_ID, DateTime.Today, USER_ID)).Returns(1);
            _joinOrderHandler.SetGroupBuyingsBoundary(mockGroupBuyingsBoundary.Object);
        }

        [TestMethod()]
        public void ListAllOrderTest()
        {
            string orderList = _joinOrderHandler.ListAllOrder(SERVER_ID);
            Assert.AreEqual("1  測試團購一\n2  測試團購二", orderList);
        }

        [TestMethod()]
        public void JoinOrderTest()
        {
            bool isSuccess = _joinOrderHandler.JoinOrder(SERVER_ID, "3", _user);
            GroupBuying? groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNull(groupBuying);
            Assert.IsFalse(isSuccess);

            isSuccess = _joinOrderHandler.JoinOrder(SERVER_ID, "2", _user);
            Assert.IsTrue(isSuccess);
            groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNotNull(groupBuying);
        }

        [TestMethod()]
        public void ListItemsOfStoreTest()
        {
            string itemsList = _joinOrderHandler.ListItemsOfStore("3", SERVER_ID);
            Assert.AreEqual("", itemsList);

            itemsList = _joinOrderHandler.ListItemsOfStore("1", SERVER_ID);
            Assert.AreEqual("1  御飯糰  30元\n2  茶葉蛋  11元", itemsList);

            itemsList = _joinOrderHandler.ListItemsOfStore("2", SERVER_ID);
            Assert.AreEqual("1  香蕉  30元\n2  御茶園  25元\n3  脆迪酥  100元", itemsList);
        }

        [TestMethod()]
        public void AddItemTest()
        {
            bool isSuccess = _joinOrderHandler.AddItem(_user, "1", 1);
            Assert.IsFalse(isSuccess); //尚未加入團購

            _joinOrderHandler.JoinOrder(SERVER_ID, "1", _user);
            isSuccess = _joinOrderHandler.AddItem(_user, "2", 1);
            Assert.IsTrue(isSuccess);

            isSuccess = _joinOrderHandler.AddItem(_user, "3", 1);
            Assert.IsFalse(isSuccess); //選擇商店沒有的物品
        }

        [TestMethod()]
        public void SubmitOrderTest()
        {
            bool isSuccess = _joinOrderHandler.SubmitOrder(_user);
            Assert.IsFalse(isSuccess); //尚未加入團購

            _joinOrderHandler.JoinOrder(SERVER_ID, "1", _user);
            GroupBuying?  groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNotNull(groupBuying);
            isSuccess = _joinOrderHandler.SubmitOrder(_user);
            Assert.IsTrue(isSuccess);

            groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNull(groupBuying);
        }
    }
}
