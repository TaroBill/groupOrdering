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
                _joinOrderProcess[user.UserID] = new GroupBuying(_groupBuyingsBoundary, groupBuyingID, serverID);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<StoreItem> ListItemsOfStore(string groupBuyingID, string serverID)
        {
            _joinOrderProcess.Add("test", new GroupBuying(_groupBuyingsBoundary));
            string storeID = _joinOrderProcess["test"].getStoreIDByGroupbuyingID(groupBuyingID);
            _joinOrderProcess["test"].ChooseExistStore(storeID, serverID);
            List<StoreItem> storeItems = _joinOrderProcess["test"].ListItemsOfStore();
            _joinOrderProcess.Remove("test");
            return storeItems;
        }

        public bool AddItem(User user, string itemID, int quantity)
        {
            if (_joinOrderProcess.ContainsKey(user.UserID))
            {
                _joinOrderProcess[user.UserID].AddItem(itemID, quantity);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditItem(User user, string itemID, int quantity)
        {
            if (_joinOrderProcess.ContainsKey(user.UserID))
            {
                _joinOrderProcess[user.UserID].EditItem(itemID, quantity);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteItem(User user, string itemID)
        {
            if (_joinOrderProcess.ContainsKey(user.UserID))
            {
                _joinOrderProcess[user.UserID].DeleteItem(user, itemID);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int SubmitOrder(User user)
        {
            int totalPrice = _joinOrderProcess[user.UserID].SubmitOrder(user);
            _joinOrderProcess.Remove(user.UserID);
            return totalPrice;
        }

        public bool CheckValid(User user)
        {
            return _joinOrderProcess.ContainsKey(user.UserID);
        }
    }
}
