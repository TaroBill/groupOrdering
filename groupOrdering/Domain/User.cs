using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        private int _debt;

        public User(string userID, string userName = "")
        {
            UserID = userID;
            UserName = userName;
            _debt = 0;
        }

        public void LoadUserData()
        {
            throw new NotImplementedException();
        }

    }
}
