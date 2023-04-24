using Microsoft.VisualStudio.TestTools.UnitTesting;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupOrdering.Domain;


namespace groupOrdering.Technical.Tests
{
    [TestClass()]
    public class DAOTests
    {
        [TestMethod()]
        public void GetDataTest()
        {
            DAO dao = new DAO();
            List<Store> store = dao.GetData<Store>("SELECT * FROM groupordering.store WHERE serverID='test';");
            Assert.AreEqual(1, store[0].StoreID);
            Assert.AreEqual("7-11", store[0].StoreName);
            Assert.AreEqual("台北市", store[0].StoreAddress);
            Assert.AreEqual("0909000000", store[0].StorePhoneNumber);
        }
    }
}