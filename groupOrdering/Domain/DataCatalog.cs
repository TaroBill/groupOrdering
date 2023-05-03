using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class DataCatalog
    {
        private CreateOrderBoundary _boundary;

        public DataCatalog()
        {
            _boundary = new CreateOrderBoundary();
        }

        public List<Store> ListStores(string serverID)
        {
            return _boundary.ListStores(serverID);
        }

        public List<GroupBuying> ListAllOrders(string serverID)
        {
            throw new NotImplementedException();
        }
    }
}
