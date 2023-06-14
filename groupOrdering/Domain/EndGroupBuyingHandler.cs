using Discord;
using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class EndGroupBuyingHandler
    {
        public EndGroupBuyingHandler() 
        {

        }

        public string EndGroupBuying(User user, string groupbuyingID)
        {
            GroupBuying groupBuying = new GroupBuying(new GroupBuyingsBoundary(), groupbuyingID);
            groupBuying.SetGroupBuying(user);
            if (groupBuying.GroupBuyingID == "0")
            {
                return "非團購建立者無法結束團購";
            }
            return groupBuying.EndGroupBuying();
        }

        public List<string> GetUsers(User user, string groupbuyingID)
        {
            GroupBuying groupBuying = new GroupBuying(new GroupBuyingsBoundary(), groupbuyingID);
            groupBuying.SetGroupBuying(user);
            return groupBuying.GetUsers();
        }
    }
}
