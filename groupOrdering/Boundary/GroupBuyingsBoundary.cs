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
            return _dao.GetData<Store>(@$"SELECT storeID AS StoreID 
                                        FROM groupordering.groupbuying 
                                        WHERE groupbuying.groupbuyingID='{groupbuyingID}';").FirstOrDefault(new Store());
        }

        public List<GroupBuying> ListAllOrders(string serverID)
        {
            return _dao.GetData<GroupBuying>($@"SELECT groupbuyingID AS GroupBuyingID, 
                                                groupbuying.storeID AS StoreID, 
                                                groupbuying.serverID AS _serverID, 
                                                callerUserID AS CallerUserID, 
                                                groupbuyingName AS GroupBuyingName 
                                                FROM groupordering.groupbuying LEFT JOIN groupordering.store 
                                                ON groupbuying.storeID=store.storeID 
                                                WHERE groupbuying.serverID='{serverID}' 
                                                AND groupbuying.status=1;");
        }

        public int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID, string name)
        {
            return _dao.SetData($@"INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID, groupbuyingName) 
                                   VALUES ('{storeID}',{1},'{serverID}','{endTime.ToString("yyyy-MM-dd hh:mm:ss")}','{userID}', '{name}');");
        }

        public GroupBuying GetGroupBuyingByGroupID(string groupbuyingID)
        {
            return _dao.GetData<GroupBuying>(@$"SELECT groupbuyingID AS GroupBuyingID, 
                                                storeID AS StoreID, 
                                                serverID AS _serverID, 
                                                callerUserID AS CallerUserID, 
                                                groupbuyingName AS GroupBuyingName 
                                                FROM groupordering.groupbuying 
                                                WHERE groupbuying.groupbuyingID='{groupbuyingID}';").FirstOrDefault(new GroupBuying());
        }

        public GroupBuying GetGroupBuyingByGroupID(string callerUserID, string groupbuyingID)
        {
            return _dao.GetData<GroupBuying>(@$"SELECT groupbuyingID AS GroupBuyingID, 
                                                storeID AS StoreID, 
                                                serverID AS _serverID, 
                                                callerUserID AS CallerUserID, 
                                                groupbuyingName AS GroupBuyingName 
                                                FROM groupordering.groupbuying 
                                                WHERE groupbuying.groupbuyingID='{groupbuyingID}' 
                                                AND groupbuying.CallerUserID='{callerUserID}' 
                                                AND groupbuying.status=1;").FirstOrDefault(new GroupBuying());
        }

        public void EndGroupBuying(string groupbuyingID)
        {
            _dao.SetData(@$"UPDATE groupordering.groupbuying 
                            SET status=0 
                            WHERE groupbuying.groupbuyingID='{groupbuyingID}';");
        }
    }
}
