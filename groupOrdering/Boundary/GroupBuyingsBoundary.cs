using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public class GroupBuyingsBoundary : IGroupBuyingsBoundary
    {
        private DAO _dao;

        public GroupBuyingsBoundary()
        {
            _dao = new DAO();
        }

        public List<GroupBuying> ListAllOrders(string serverID)
        {
            throw new NotImplementedException();
        }

        public int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID, string groupBuyingName)
        {
            return _dao.SetData($"INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID, groupbuyingName) VALUES ('{storeID}',{1},'{serverID}','{endTime.ToString("yyyy-MM-dd")}','{userID}', '{groupBuyingName}');");
        }
    }
}
