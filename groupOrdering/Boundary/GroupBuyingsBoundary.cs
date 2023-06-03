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
        private readonly DAO _dao;

        public GroupBuyingsBoundary()
        {
            _dao = new DAO();
        }

        public Store GetStoreByGroupbuyingID(string groupbuyingID)
        {
            return _dao.GetData<Store>($"SELECT * FROM groupordering.groupbuying " +
                                        $"WHERE groupbuying.groupbuyingID='{groupbuyingID}';").FirstOrDefault(new Store());
        }

        public List<GroupBuying> ListAllOrders(string serverID)
        {
            return _dao.GetData<GroupBuying>($@"SELECT groupbuying.groupbuyingID AS GroupBuyingID, store.storeID AS StoreID, store.storeName AS GroupBuyingName, groupbuying.callerUserID AS CallerUserID 
                                        FROM groupordering.groupbuying LEFT JOIN groupordering.store 
                                        ON groupbuying.storeID=store.storeID
                                        WHERE groupbuying.serverID='{serverID}';");
        }

        public int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID, string name)
        {
            return _dao.SetData($@"INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID, groupbuyingName) 
                                   VALUES ('{storeID}',{1},'{serverID}','{endTime.ToString("yyyy-MM-dd")}','{userID}', '{name}');");
        }

        public GroupBuying GetGroupBuyingByGroupID(string groupbuyingID)
        {
            //TODO 整個SQL寫錯
            return _dao.GetData<GroupBuying>(@$"SELECT * FROM groupordering.memberorder 
                                                WHERE groupbuyingID='{groupbuyingID}';").FirstOrDefault(new GroupBuying());
        }
    }
}
