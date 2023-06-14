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
    public class MemberOrderTests
    {
        private MemberOrder _order;
        private const string USER_ID = "Tester";
        private User _user;

        [TestInitialize()]
        public void Initialize()
        {
            _order = new MemberOrder();
            _order.UserID = USER_ID;
            _user = new User(USER_ID);
        }

        [TestMethod()]
        public void GetTotalTest()
        {
            _order.AddItem(new StoreItem("1", "item1", 100), 5);
            Assert.AreEqual(500, _order.GetTotal());
        }

        [TestMethod()]
        public void AddItemTest()
        {
            _order.AddItem(new StoreItem("1", "item1", 100), 5);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 5\n", _order.OrderToString());
        }

        [TestMethod()]
        public void EditItemTest()
        {
            StoreItem storeItem = new StoreItem("1", "item1", 100);
            _order.AddItem(storeItem, 5);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 5\n", _order.OrderToString());

            _order.EditItem(storeItem, 6);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 6\n", _order.OrderToString());
        }

        [TestMethod()]
        public void DeleteItemTest()
        {
            StoreItem storeItem = new StoreItem("1", "item1", 100);
            _order.AddItem(storeItem, 5);
            Assert.AreEqual($"{USER_ID}\n\titem1 X 5\n", _order.OrderToString());

            _order.DeleteItem(storeItem);
            Assert.AreEqual($"{USER_ID}\n", _order.OrderToString());
        }
    }
}