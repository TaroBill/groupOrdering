using groupOrdering.Boundary.Interfaces;
using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public class StoreItemBoundary : IStoreItemBoundary
    {
        private DAO _dao;

        public StoreItemBoundary()
        {
            _dao = new DAO();
        }

        public StoreItem getStoreItem(string storeitemID)
        {
            return _dao.GetData<StoreItem>($"SELECT * FROM groupordering.storeitem " +
                                            $"WHERE storeitemID='{storeitemID}';").FirstOrDefault(new StoreItem());
        }
    }
}
