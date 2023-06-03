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

        public List<GroupBuying> ListAllOrder(string serverID)
        {
            return GroupBuyings.ListAllOrders(serverID);
        }

        public bool JoinOrder(string serverID, string groupBuyingID, User user)
        {
            try
            {
                _joinOrderProcess[user.UserID] = new GroupBuying(_groupBuyingsBoundary, groupBuyingID);
                _joinOrderProcess[user.UserID].JoinOrder(user);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public List<StoreItem> ListItemsOfStore(string storeID, string serverID, IStoresBoundary storesBoundary)
        {
            Store store = new Store();
            store.SetStoresBoundary(storesBoundary);
            store.SetStore(storeID, serverID);
            return store.ListItemsOfStore();
        }

        private bool IsContainItem(string itemID, List<StoreItem> storeItems)
        {
            foreach (StoreItem item in storeItems)
            {
                if (item.storeitemID == itemID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddItem(User user, string itemID, int quantity)
        {
            if (!_joinOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            List<StoreItem> storeitems = _joinOrderProcess[user.UserID].GetStore().ListItemsOfStore();
            if (!IsContainItem(itemID, storeitems))
            {
                return false;
            }
            _joinOrderProcess[user.UserID].AddItem(user, itemID, quantity);
            return true;
        }

        public bool EditItem(User user, string itemID, int quantity)
        {
            if (!_joinOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            List<StoreItem> storeitems = _joinOrderProcess[user.UserID].GetStore().ListItemsOfStore();
            if (!IsContainItem(itemID, storeitems))
            {
                return false;
            }
            _joinOrderProcess[user.UserID].EditItem(user, itemID, quantity);
            return true;
        }

        public bool DeleteItem(User user, string itemID)
        {
            if (!_joinOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            List<StoreItem> storeitems = _joinOrderProcess[user.UserID].GetStore().ListItemsOfStore();
            if (!IsContainItem(itemID, storeitems))
            {
                return false;
            }
            _joinOrderProcess[user.UserID].DeleteItem(user, itemID);
            return true;
        }

        public bool SubmitOrder(User user)
        {
            if (!_joinOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            bool isSuccess = _joinOrderProcess[user.UserID].SubmitOrder(user);
            _joinOrderProcess.Remove(user.UserID);
            return isSuccess;
        }

        public int GetTotal(User user)
        {
            if (!_joinOrderProcess.ContainsKey(user.UserID))
            {
                return 0;
            }
            int total = _joinOrderProcess[user.UserID].GetTotal(user);
            return total;
        }
    }
}
