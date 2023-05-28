using Discord;
using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace groupOrdering.Domain
{
    public class JoinOrderHandler
    {
        private IDictionary<string, GroupBuying> _joinOrderProcess;

        private IGroupBuyingsBoundary _groupBuyingsBoundary;

        public JoinOrderHandler() 
        {
            _groupBuyingsBoundary = new GroupBuyingsBoundary();
            _joinOrderProcess = new Dictionary<string, GroupBuying>();
        }

        public GroupBuying? GetUserGroupBuyingProcess(User user)
        {
            if (!_joinOrderProcess.ContainsKey(user.UserID))
            {
                return null;
            }
            return _joinOrderProcess[user.UserID];
        }

        public void SetGroupBuyingsBoundary(IGroupBuyingsBoundary boundary)
        {
            _groupBuyingsBoundary = boundary;
        }

        public string ListAllOrder(string serverID)
        {
            throw new NotImplementedException();
        }

        public bool JoinOrder(string serverID, string groupBuyingID, User user)
        {
            throw new NotImplementedException();
        }

        public string ListItemsOfStore(string groupBuyingID, string serverID)
        {
            throw new NotImplementedException();
        }

        public bool AddItem(User user, string itemID, int quantity)
        {
            throw new NotImplementedException();
        }

        public bool SubmitOrder(User user)
        {
            throw new NotImplementedException();
        }
    }
}
