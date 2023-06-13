using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using groupOrdering.Boundary;
using Discord;

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
        public void AddStoreItemTest()
        {
            int count = _store.GetStoreItemCount();
            Assert.AreEqual(0, count);
            _store.AddStoreItem("item1", 20);
            _store.AddStoreItem("item2", 30);
            _store.AddStoreItem("item3", 40);
            count = _store.GetStoreItemCount();
            Assert.AreEqual(3, count);
        }

        [TestMethod()]
        public void GetStoreItem()
        {
            StoreItem item1 = new StoreItem("1", "item1", 10);
            StoreItem item2 = new StoreItem("2", "item2", 20);
            StoreItem item3 = new StoreItem("3", "item3", 30);
            _store.AddStoreItem(item1);
            _store.AddStoreItem(item2);
            _store.AddStoreItem(item3);
            var item = _store.GetStoreItem("1");
            Assert.AreEqual("item1", item.StoreitemName);
            item = _store.GetStoreItem("3");
            Assert.AreEqual("item3", item.StoreitemName);
        }

        [TestMethod()]
        public void DeleteStoreItem()
        {
            StoreItem item1 = new StoreItem("1", "item1", 10);
            StoreItem item2 = new StoreItem("2", "item2", 20);
            StoreItem item3 = new StoreItem("3", "item3", 30);
            _store.AddStoreItem(item1);
            _store.AddStoreItem(item2);
            _store.AddStoreItem(item3);
            var item = _store.GetStoreItem("2");
            Assert.AreEqual("item2", item.StoreitemName);
            _store.DeleteStoreItem("item2");
            item = _store.GetStoreItem("2");
            Assert.AreEqual("", item.StoreitemName);
        }

        [TestMethod()]
        public void DeleteStoreItem2()
        {
            StoreItem item1 = new StoreItem("1", "item1", 10);
            StoreItem item2 = new StoreItem("2", "item2", 20);
            StoreItem item3 = new StoreItem("3", "item3", 30);
            _store.AddStoreItem(item1);
            _store.AddStoreItem(item2);
            _store.AddStoreItem(item3);
            var item = _store.GetStoreItem("1");
            Assert.AreEqual("item1", item.StoreitemName);

            _store.DeleteStoreItemAt(0);
            item = _store.GetStoreItem("1");
            Assert.AreEqual("", item.StoreitemName);
        }

        [TestMethod()]
        public void IsInStoreItemList()
        {
            StoreItem item1 = new StoreItem("1", "item1", 10);
            StoreItem item2 = new StoreItem("2", "item2", 20);
            StoreItem item3 = new StoreItem("3", "item3", 30);
            bool isIn = _store.IsInStoreItemList(0);
            Assert.IsFalse(isIn);
            _store.AddStoreItem(item1);

            isIn = _store.IsInStoreItemList(0);
            Assert.IsTrue(isIn);

            _store.AddStoreItem(item2);
            _store.AddStoreItem(item3);
            isIn = _store.IsInStoreItemList(2);
            Assert.IsTrue(isIn);

            isIn = _store.IsInStoreItemList(3);
            Assert.IsFalse(isIn);
        }
    }
}