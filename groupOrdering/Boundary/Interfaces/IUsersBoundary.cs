using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary.Interfaces
{
    public interface IUsersBoundary
    {
        public void AddNewDebt(string recieveUserID, string spendUserID, int money);
    }
}
