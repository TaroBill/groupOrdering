using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupOrdering.Boundary;
using Moq;
using static System.Formats.Asn1.AsnWriter;

namespace groupOrdering.Domain.Tests
{
    [TestClass()]
    public class CreateStoreHandlerTests
    {
        CreateStoreHandler _createStoreHandler;
        private const string USER_ID = "Tester";
        private const string SERVER_ID = "test";
        private User _user;


        [TestInitialize()]
        public void Initialize()
        {
            _user = new User(USER_ID);
            _createStoreHandler = new CreateStoreHandler();
        }

        [TestMethod()]
        public void CreateStoreTest()
        {
            var store = _createStoreHandler.GetStore(_user);
            Assert.AreEqual("", store.StoreName);
            _createStoreHandler.CreateStore(_user, SERVER_ID, "test", "address", "0900000000");
            store = _createStoreHandler.GetStore(_user);
            Assert.AreEqual("test", store.StoreName);
            Assert.AreEqual(SERVER_ID, store.ServerID);
            Assert.AreEqual("address", store.StoreAddress);
            Assert.AreEqual("0900000000", store.StorePhoneNumber);
        }

        [TestMethod()]
        public void AddStoreItemTest()
        {
            _createStoreHandler.CreateStore(_user, SERVER_ID, "test", "address", "0900000000");
            int count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(0, count);
            _createStoreHandler.AddStoreItem(_user, "item1", 10);
            count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(1, count);
            _createStoreHandler.AddStoreItem(_user, "item2", 20);
            _createStoreHandler.AddStoreItem(_user, "item3", 30);
            count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(3, count);
        }

        [TestMethod()]
        public void DeleteStoreItemTest()
        {
            _createStoreHandler.CreateStore(_user, SERVER_ID, "test", "address", "0900000000");
            _createStoreHandler.AddStoreItem(_user, "item1", 10);
            _createStoreHandler.AddStoreItem(_user, "item2", 20);
            _createStoreHandler.AddStoreItem(_user, "item3", 30);
            int count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(3, count);
            _createStoreHandler.DeleteStoreItem(_user, "item2");
            count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(2, count);
            var store = _createStoreHandler.GetStore(_user);
            var storeItems = store.GetStoreItems();
            Assert.AreEqual(2, storeItems.Count);
            Assert.AreEqual("item1", storeItems[0].StoreitemName);
            Assert.AreEqual("item3", storeItems[1].StoreitemName);
        }

        [TestMethod()]
        public void EditStoreNameTest()
        {
            _createStoreHandler.CreateStore(_user, SERVER_ID, "test", "address", "0900000000");
            var store = _createStoreHandler.GetStore(_user);
            Assert.AreEqual("test", store.StoreName);
            _createStoreHandler.EditStoreName(_user, "name");
            Assert.AreEqual("name", store.StoreName);
        }

        [TestMethod()]
        public void GetTotalStoreItemCountTest()
        {
            int count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(0, count);
            _createStoreHandler.CreateStore(_user, SERVER_ID, "test", "address", "0900000000");
            _createStoreHandler.AddStoreItem(_user, "item1", 10);
            _createStoreHandler.AddStoreItem(_user, "item2", 20);
            _createStoreHandler.AddStoreItem(_user, "item3", 30);
            count = _createStoreHandler.GetTotalStoreItemCount(_user);
            Assert.AreEqual(3, count);
        }
    }
}