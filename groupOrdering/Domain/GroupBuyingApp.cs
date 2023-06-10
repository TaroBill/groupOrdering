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
        private EditOrderHandler _editOrderHandler;
        private EndGroupBuyingHandler _endGroupBuyingHandler;

        public GroupBuyingApp()
        {
            _createOrderHandler = new CreateOrderHandler();
            _createStoreHandler = new CreateStoreHandler();
            _joinOrderHandler = new JoinOrderHandler();
            _editOrderHandler = new EditOrderHandler();
            _endGroupBuyingHandler = new EndGroupBuyingHandler();
        }

        public CreateStoreHandler GetCreateStoreHandler() { return _createStoreHandler; }

        public JoinOrderHandler GetJoinOrderHandler() { return _joinOrderHandler; }

        public CreateOrderHandler GetCreateOrderHandler() { return _createOrderHandler; }   

        public EditOrderHandler GetEditOrderHandler() { return _editOrderHandler; }

        public EndGroupBuyingHandler GetEndGroupBuyingHandler() { return _endGroupBuyingHandler;}
    }
}
