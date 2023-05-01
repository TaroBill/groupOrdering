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
        private DAO _dao;

        [TestInitialize()]
        public void Initialize()
        {
            _dao = new DAO();
        }

        [TestMethod()]
        public void GetDataTest()
        {
            List<Store> store = _dao.GetData<Store>($"SELECT * FROM groupordering.store WHERE serverID='test';");
            Assert.IsTrue(store.Count > 0);
        }

        [TestMethod()]
        public void SetDataTest()
        {
            int result = _dao.SetData("INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID) VALUES (1,0,'0','2023-02-28','Tester');");
            Assert.AreEqual(1, result);
        }
    }
}