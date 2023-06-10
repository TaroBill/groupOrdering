using groupOrdering.Boundary;
using groupOrdering.Boundary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public static class Users
    {
        private static IUsersBoundary _usersBoundary = new UsersBoundary();

        public static void AddNewDebt(string recieveUserID, string spendUserID, int money)
        {
            _usersBoundary.AddNewDebt(recieveUserID, spendUserID, money);
        }
    }
}
