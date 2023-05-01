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
    public class CreateOrderHandlerTests
    {
        private CreateOrderHandler _createOrderHandler;
        private const string USER_ID = "Tester";
        const string STORE_ID = "1";
        private const string SERVER_ID = "test";

        [TestInitialize()]
        public void Initialize()
        {
            _createOrderHandler = new CreateOrderHandler();
        }

        [TestMethod()]
        public void CreateGroupBuyingTest()
        {
            _createOrderHandler.CreateGroupBuying(USER_ID);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNotNull(groupBuying);
        }

        [TestMethod()]
        public void ListStoreTest()
        {
            string storeList = _createOrderHandler.ListStore(SERVER_ID);
            Assert.AreNotEqual("", storeList);
        }

        [TestMethod()]
        public void ChooseExistStoreTest()
        {
            _createOrderHandler.ChooseExistStore(USER_ID, STORE_ID, SERVER_ID);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(USER_ID);
            _createOrderHandler.ChooseExistStore(USER_ID, STORE_ID, SERVER_ID);
            groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNotNull(groupBuying);
            Store store = groupBuying.GetStore();
            Assert.AreEqual(STORE_ID, store.StoreID);
        }

        [TestMethod()]
        public void SetEndTimeTest()
        {
            DateTime time = DateTime.Parse("2022/02/22");
            _createOrderHandler.SetEndTime(USER_ID, time);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(USER_ID);
            _createOrderHandler.SetEndTime(USER_ID, time);
            groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNotNull(groupBuying);
            Assert.AreEqual(time, groupBuying.GetEndTime());
        }

        [TestMethod()]
        public void EndEditTest()
        {
            _createOrderHandler.EndEdit(USER_ID);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(USER_ID);
            groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNotNull(groupBuying);

            _createOrderHandler.ChooseExistStore(USER_ID, STORE_ID, SERVER_ID);
            _createOrderHandler.EndEdit(USER_ID);
            groupBuying = _createOrderHandler.GetGroupBuying(USER_ID);
            Assert.IsNull(groupBuying);
        }
    }
}