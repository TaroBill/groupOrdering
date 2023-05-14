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
    public class CreateOrderHandlerTests
    {
        private CreateOrderHandler _createOrderHandler;
        private const string USER_ID = "Tester";
        private const string STORE_ID = "1";
        private const string SERVER_ID = "test";

        private User _user;

        [TestInitialize()]
        public void Initialize()
        {
            _user = new User(USER_ID);
            _createOrderHandler = new CreateOrderHandler();

            var mockGroupBuyingsBoundary = new Mock<IGroupBuyingsBoundary>();
            mockGroupBuyingsBoundary.Setup(p => p.ListAllOrders(SERVER_ID)).Returns(new List<GroupBuying>()
            {
                new GroupBuying(mockGroupBuyingsBoundary.Object, "1", "測試團購一", SERVER_ID),
                new GroupBuying(mockGroupBuyingsBoundary.Object, "2", "測試團購二", SERVER_ID)
            });
            mockGroupBuyingsBoundary.Setup(p => p.PublishGroupBuying(STORE_ID, SERVER_ID, DateTime.Today, USER_ID)).Returns(1);
            _createOrderHandler.SetGroupBuyingsBoundary(mockGroupBuyingsBoundary.Object);
        }


        [TestMethod()]
        public void CreateGroupBuyingTest()
        {

            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(_user);
            groupBuying = _createOrderHandler.GetGroupBuying(_user);
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
            _createOrderHandler.ChooseExistStore(_user, STORE_ID, SERVER_ID);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(_user);
            _createOrderHandler.ChooseExistStore(_user, STORE_ID, SERVER_ID);
            groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNotNull(groupBuying);
            Store store = groupBuying.GetStore();
            Assert.AreEqual(STORE_ID, store.StoreID);
        }

        [TestMethod()]
        public void SetEndTimeTest()
        {
            DateTime time = DateTime.Parse("2022/02/22");
            _createOrderHandler.SetEndTime(_user, time);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(_user);
            _createOrderHandler.SetEndTime(_user, time);
            groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNotNull(groupBuying);
            Assert.AreEqual(time, groupBuying.GetEndTime());
        }

        [TestMethod()]
        public void EndEditTest()
        {
            _createOrderHandler.EndEdit(_user);
            GroupBuying? groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNull(groupBuying);

            _createOrderHandler.CreateGroupBuying(_user);
            groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNotNull(groupBuying);

            _createOrderHandler.ChooseExistStore(_user, STORE_ID, SERVER_ID);
            _createOrderHandler.EndEdit(_user);
            groupBuying = _createOrderHandler.GetGroupBuying(_user);
            Assert.IsNull(groupBuying);
        }

        [TestMethod()]
        public void CheckStartOrderTest()
        {
            bool startedOrder = _createOrderHandler.CheckStartOrder(_user);
            Assert.IsFalse(startedOrder);

            _createOrderHandler.CreateGroupBuying(_user);
            startedOrder = _createOrderHandler.CheckStartOrder(_user);
            Assert.IsTrue(startedOrder);
        }

        [TestMethod()]
        public void CheckChooseStoreTest()
        {
            bool chosenStore = _createOrderHandler.CheckChooseStore(_user);
            Assert.IsFalse(chosenStore);

            _createOrderHandler.CreateGroupBuying(_user);
            chosenStore = _createOrderHandler.CheckChooseStore(_user);
            Assert.IsFalse(chosenStore);

            _createOrderHandler.ChooseExistStore(_user, STORE_ID, SERVER_ID);
            chosenStore = _createOrderHandler.CheckChooseStore(_user);
            Assert.IsTrue(chosenStore);

        }

        [TestMethod()]
        public void CheckEndTimeTest()
        {
            DateTime dateTime = new DateTime(2000, 12, 10);
            bool checkEndTime = _createOrderHandler.CheckEndTime(dateTime);
            Assert.IsFalse(checkEndTime);

            dateTime = DateTime.Now;
            checkEndTime = _createOrderHandler.CheckEndTime(dateTime);
            Assert.IsFalse(checkEndTime);

            dateTime.AddDays(10);
            checkEndTime = _createOrderHandler.CheckEndTime(dateTime);
            Assert.IsTrue(checkEndTime);
        }

        [TestMethod()]
        public void CheckEndTimeValidTest()
        {
            bool result = _createOrderHandler.CheckEndTimeValid(_user);
            Assert.IsFalse(result);

            _createOrderHandler.CreateGroupBuying(_user);
            result = _createOrderHandler.CheckEndTimeValid(_user);
            Assert.IsFalse(result);

            _createOrderHandler.ChooseExistStore(_user, STORE_ID, SERVER_ID);
            _createOrderHandler.SetEndTime(_user, DateTime.Now.AddDays(10));
            Assert.IsTrue(result);
        }
    }
}