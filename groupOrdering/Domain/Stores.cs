using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public static class Stores
    {
        private static IStoresBoundary _storesBoundary = new StoresBoundary();

        public static List<Store> ListStores(string serverID)
        {
            return _storesBoundary.ListStores(serverID);
        }
    }
}
