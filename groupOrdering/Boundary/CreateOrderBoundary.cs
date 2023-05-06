using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public class CreateOrderBoundary
    {
        private DAO _dao;
        public CreateOrderBoundary() 
        {
            _dao = new DAO();
        }


        public int PublishGroupBuying(string storeID, string serverID, DateTime endTime, User user)
        {
            return _dao.SetData($"INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID) VALUES ('{storeID}',{1},'{serverID}','{endTime.ToString("yyyy-MM-dd")}','{user.UserID}');");
        }
    }
}
