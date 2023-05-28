using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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

        public List<StoreItem> ListItemsOfStore(string storeID)
        {
            return _dao.GetData<StoreItem>($"SELECT storeitem.storeitemID AS storeitemID, storeitem.storeitemName AS storeitemName, storeitem.storeitemPrice AS storeitemPrice " +
                                            $"FROM groupordering.storeitem " +
                                            $"WHERE storeitem_storeID='{storeID}';");
        }
    }
}
