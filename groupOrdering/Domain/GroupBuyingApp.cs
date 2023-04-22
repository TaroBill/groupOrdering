using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class GroupBuyingApp
    {
        private CreateOrderHandler _createOrderHandler;
        private CreateStoreHandler _createStoreHandler;
        private JoinOrderHandler  _joinOrderHandler;
        private EndGroupBuyingHandler _endGroupBuyingHandler;

        public GroupBuyingApp()
        {

        }

        public CreateStoreHandler GetCreateStoreHandler() { return _createStoreHandler; }

        public JoinOrderHandler GetJoinOrderHandler() { return _joinOrderHandler; }

        public CreateOrderHandler GetCreateOrderHandler() { return _createOrderHandler; }   

        public EndGroupBuyingHandler GetEndGroupBuyingHandler() { return _endGroupBuyingHandler;}
    }
}
