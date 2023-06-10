using groupOrdering.Boundary.Interfaces;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public class UsersBoundary : IUsersBoundary
    {
        private DAO _dao;

        public UsersBoundary()
        {
            _dao = new DAO();
        }

        public void AddNewDebt(string recieveUserID, string spendUserID, int money)
        {
            _dao.SetData(@$"INSERT INTO groupordering.debt(price, borrowerID, debtorID) 
                            VALUES ({money}, '{spendUserID}', '{recieveUserID}');");
        }
    }
}
