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
        IStoresBoundary _mockStoresBoundary;

        [TestInitialize()]
        public void Initialize()
        {
            _user = new User(USER_ID);
            _joinOrderHandler = new JoinOrderHandler();

            var mockGroupBuyingsBoundary = new Mock<IGroupBuyingsBoundary>();

            GroupBuying groupBuying1 = new GroupBuying(mockGroupBuyingsBoundary.Object, "1", "測試團購1", SERVER_ID);
            GroupBuying groupBuying2 = new GroupBuying(mockGroupBuyingsBoundary.Object, "2", "測試團購2", SERVER_ID);
            Store store1 = new Store("1", "7-11", "台北市", "0909000000");
            Store store2 = new Store("2", "全家", "我家對面", "0911111111");

            StoreItem store1item1 = new StoreItem("1", "御飯糰", 30);
            StoreItem store1item2 = new StoreItem("2", "茶葉蛋", 11);
            StoreItem store2item1 = new StoreItem("1", "香蕉", 30);
            StoreItem store2item2 = new StoreItem("2", "御茶園", 25);
            StoreItem store2item3 = new StoreItem("3", "脆迪酥", 100);
            store1.AddStoreItem(store1item1);
            store1.AddStoreItem(store1item2);
            store2.AddStoreItem(store2item1);
            store2.AddStoreItem(store2item2);
            store2.AddStoreItem(store2item3);

            var mockStoresBoundary = new Mock<IStoresBoundary>();
            mockStoresBoundary.Setup(p => p.ListStores("test")).Returns(new List<Store>()
            {
                store1,
                store2
            });
            mockStoresBoundary.Setup(p => p.GetStore("1", "test")).Returns(store1);
            mockStoresBoundary.Setup(p => p.GetStore("2", "test")).Returns(store2);
            mockStoresBoundary.Setup(p => p.ListItemsOfStore("1")).Returns(new List<StoreItem> { store1item1, store1item2 });
            mockStoresBoundary.Setup(p => p.ListItemsOfStore("2")).Returns(new List<StoreItem> { store2item1, store2item2, store2item3 });
            store1.SetStoresBoundary(mockStoresBoundary.Object);
            store2.SetStoresBoundary(mockStoresBoundary.Object);

            groupBuying1.SetStore(store1);
            groupBuying2.SetStore(store2);
            mockGroupBuyingsBoundary.Setup(p => p.ListAllOrders(SERVER_ID)).Returns(new List<GroupBuying>()
            {
                groupBuying1,
                groupBuying2
            });
            mockGroupBuyingsBoundary.Setup(p => p.GetStoreByGroupbuyingID("1")).Returns(store1);
            mockGroupBuyingsBoundary.Setup(p => p.GetStoreByGroupbuyingID("2")).Returns(store2);
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying(STORE_ID, SERVER_ID, DateTime.Today, USER_ID)).Returns(1);
            mockGroupBuyingsBoundary.Setup(p => p.GetGroupBuyingByGroupID("1")).Returns(groupBuying1);
            mockGroupBuyingsBoundary.Setup(p => p.GetGroupBuyingByGroupID("2")).Returns(groupBuying2);
            _joinOrderHandler.SetGroupBuyingsBoundary(mockGroupBuyingsBoundary.Object);

            GroupBuyings._groupBuyingsBoundary = mockGroupBuyingsBoundary.Object;
            _mockStoresBoundary = mockStoresBoundary.Object;
        }

        [TestMethod()]
        public void ListAllOrderTest()
        {
            List<GroupBuying> orderList = _joinOrderHandler.ListAllOrder(SERVER_ID);
            for(int i = 0; i < orderList.Count; i++)
            {
                var order = orderList[i];
                Assert.AreEqual((i+1).ToString(), order.GroupBuyingID);
                Assert.AreEqual($"測試團購{(i + 1)}", order.GroupbuyingName);
            }
        }

        [TestMethod()]
        public void JoinOrderTest()
        {
            GroupBuying? groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNull(groupBuying);
            bool isSuccess = _joinOrderHandler.JoinOrder(SERVER_ID, "3", _user);
            groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsFalse(isSuccess);
            Assert.IsNull(groupBuying);

            isSuccess = _joinOrderHandler.JoinOrder(SERVER_ID, "2", _user);
            Assert.IsTrue(isSuccess);
            groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNotNull(groupBuying);
        }

        [TestMethod()]
        public void ListItemsOfStoreTest()
        {
            int [] price = new int[2] {30, 11};
            string[] name = new string[2] { "御飯糰", "茶葉蛋" };
            List<StoreItem>  itemsList = _joinOrderHandler.ListItemsOfStore("1", SERVER_ID, _mockStoresBoundary);
            for (int i=0;i<itemsList.Count;i++)
            {
                Assert.AreEqual(name[i], itemsList[i].storeitemName);
                Assert.AreEqual(price[i], itemsList[i].storeitemPrice);
            }

            itemsList = _joinOrderHandler.ListItemsOfStore("2", SERVER_ID, _mockStoresBoundary);
            price = new int[3] { 30, 25, 100 };
            name = new string[3] { "香蕉", "御茶園", "脆迪酥" };
            for (int i = 0; i < itemsList.Count; i++)
            {
                Assert.AreEqual(name[i], itemsList[i].storeitemName);
                Assert.AreEqual(price[i], itemsList[i].storeitemPrice);
            }
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
        public void GetTotalTest()
        {
            _joinOrderHandler.JoinOrder(SERVER_ID, "1", _user);
            GroupBuying? groupBuying = _joinOrderHandler.GetUserGroupBuyingProcess(_user);
            Assert.IsNotNull(groupBuying);
            _joinOrderHandler.AddItem(_user, "1", 5);
            _joinOrderHandler.AddItem(_user, "2", 10);
            int ammount = _joinOrderHandler.GetTotal(_user);
            Assert.AreEqual(260, ammount);
        }
    }
}
