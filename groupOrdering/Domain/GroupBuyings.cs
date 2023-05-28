using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static groupOrdering.Technical.DTO;

namespace groupOrdering.Domain
{
    public static class GroupBuyings
    {
        private static IGroupBuyingsBoundary _groupBuyingsBoundary = new GroupBuyingsBoundary();

        public static List<GroupBuyingDTO> ListAllOrders(string serverID)
        {
            return _groupBuyingsBoundary.ListAllOrders(serverID);
        }
    }
}
