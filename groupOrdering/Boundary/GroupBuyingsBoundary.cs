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

        public List<GroupBuying> ListAllOrders(string serverID)
        {
            throw new NotImplementedException();
        }
    }
}
