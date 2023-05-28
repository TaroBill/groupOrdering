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

        public Store getStoreIDByGroupbuyingID(string groupbuyingID)
        {
            return _dao.GetData<Store>($"SELECT * FROM groupordering.groupbuying " +
                                        $"WHERE groupbuying.groupbuyingID='{groupbuyingID}';").FirstOrDefault(new Store());
        }

        public List<GroupBuying> ListAllOrders(string serverID)
        {
            return _dao.GetData<GroupBuying>($"SELECT CONVERT(groupbuying.groupbuyingID, CHAR) AS GroupBuyingID, store.storeID, store.storeName AS groupbuyingName " +
                                        $"FROM groupordering.groupbuying LEFT JOIN groupordering.store " +
                                        $"ON groupbuying.storeID=store.storeID " +
                                        $"WHERE groupbuying.serverID='{serverID}';");
        }

        public int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID)
        {
            return _dao.SetData($"INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID) VALUES ('{storeID}',{1},'{serverID}','{endTime.ToString("yyyy-MM-dd")}','{userID}');");
        }
    }
}
