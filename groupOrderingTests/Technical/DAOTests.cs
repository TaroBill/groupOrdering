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
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetDataTest()
        {
            DAO dao = new DAO();
            int result = dao.SetData("INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime) VALUES (1,0,'0','2023-02-28');");
            Assert.AreEqual(1, result);
        }
    }
}