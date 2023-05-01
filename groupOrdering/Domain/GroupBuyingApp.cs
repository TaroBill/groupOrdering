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
        private DataCatalog _dataCatalog;
        private Users _users;

        public GroupBuyingApp()
        {
            _dataCatalog = new DataCatalog();
            _createOrderHandler = new CreateOrderHandler(_dataCatalog);
            _createStoreHandler = new CreateStoreHandler();
            _joinOrderHandler = new JoinOrderHandler();
            _endGroupBuyingHandler = new EndGroupBuyingHandler(_users);
            _users = new Users();
        }

        public CreateStoreHandler GetCreateStoreHandler() { return _createStoreHandler; }

        public JoinOrderHandler GetJoinOrderHandler() { return _joinOrderHandler; }

        public CreateOrderHandler GetCreateOrderHandler() { return _createOrderHandler; }   

        public EndGroupBuyingHandler GetEndGroupBuyingHandler() { return _endGroupBuyingHandler;}
    }
}
