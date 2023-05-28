using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public class StoresBoundary : IStoresBoundary
    {
        private DAO _dao;
        public StoresBoundary()
        {
            _dao = new DAO();
        }

        public List<Store> ListStores(string serverID)
        {
            return _dao.GetData<Store>($"SELECT * FROM groupordering.store WHERE serverID='{serverID}';");
        }

        public Store GetStore(string storeID, string serverID)
        {
            return _dao.GetData<Store>($"SELECT * FROM groupordering.store WHERE storeID='{storeID}' AND serverID='{serverID}';").FirstOrDefault(new Store());
        }
    }
}
