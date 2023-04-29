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

        public List<Store> ListStores()
        {
            return _boundary.ListStores();
        }

        public List<GroupBuying> ListAllOrders()
        {
            throw new NotImplementedException();
        }
    }
}
