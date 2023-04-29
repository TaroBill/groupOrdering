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

        public List<Store> ListStores()
        {
            return _dao.GetData<Store>("SELECT * FROM groupordering.store WHERE serverID='test';");
        }

        public int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID)
        {
            return _dao.SetData(string.Format("INSERT INTO groupordering.groupbuying(storeID,status,serverID,endTime,callerUserID) VALUES ({0},{1},{2},'{3}',{4});", storeID, 1, serverID, endTime.ToString("yyyy-MM-dd"), userID));
        }

        public List<GroupBuying> ListAllOrders()
        {
            throw new NotImplementedException();
        }
    }
}
