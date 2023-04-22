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

        public JoinOrderHandler() 
        { 

        }

        public List<string> ListAllOrder()
        {
            throw new NotImplementedException();
        }

        public void JoinOrder(string groupBuyingID, string userID)
        {
            throw new NotImplementedException();
        }

        public List<string> ListItemsOfStore(string groupBuyingID)
        {
            throw new NotImplementedException();
        }

        public void AddItem(string userID, string itemID, int quantity)
        {
            throw new NotImplementedException();
        }

        public void SubmitOrder(string useerID)
        {
            throw new NotImplementedException();
        }
    }
}
