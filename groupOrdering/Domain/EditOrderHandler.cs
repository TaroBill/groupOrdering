using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class EditOrderHandler
    {
        private IGroupBuyingsBoundary _groupBuyingsBoundary;
        private IDictionary<string, GroupBuying> _editOrderProcess;

        public EditOrderHandler()
        {
            _groupBuyingsBoundary = new GroupBuyingsBoundary();
            _editOrderProcess = new Dictionary<string, GroupBuying>();
        }

        public List<GroupBuying> ListAllOrder(string serverID)
        {
            return GroupBuyings.ListAllOrders(serverID);
        }

        public void SelectGroupBuying(User user, string groupbuyingID)
        {
            _editOrderProcess[user.UserID] = new GroupBuying(_groupBuyingsBoundary, groupbuyingID);
            _editOrderProcess[user.UserID].SelectGroupBuying(user);
        }

        private bool IsContainItem(string itemID, List<StoreItem> storeItems)
        {
            foreach (StoreItem item in storeItems)
            {
                if (item.StoreitemID == itemID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddItem(User user, string itemID, int quantity)
        {
            if (!_editOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            List<StoreItem> storeitems = _editOrderProcess[user.UserID].GetStore().ListItemsOfStore();
            if (!IsContainItem(itemID, storeitems))
            {
                return false;
            }
            _editOrderProcess[user.UserID].AddItem(user, itemID, quantity);
            return true;
        }

        public bool EditItem(User user, string itemID, int quantity)
        {
            if (!_editOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            List<StoreItem> storeitems = _editOrderProcess[user.UserID].GetStore().ListItemsOfStore();
            if (!IsContainItem(itemID, storeitems))
            {
                return false;
            }
            _editOrderProcess[user.UserID].EditItem(user, itemID, quantity);
            return true;
        }

        public bool DeleteItem(User user, string itemID)
        {
            if (!_editOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            List<StoreItem> storeitems = _editOrderProcess[user.UserID].GetStore().ListItemsOfStore();
            if (!IsContainItem(itemID, storeitems))
            {
                return false;
            }
            _editOrderProcess[user.UserID].DeleteItem(user, itemID);
            return true;
        }

        public bool EndEdit(User user)
        {
            if (!_editOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            bool isSuccess = _editOrderProcess[user.UserID].SubmitOrder(user);
            _editOrderProcess.Remove(user.UserID);
            return isSuccess;
        }

        public int GetTotal(User user)
        {
            if (!_editOrderProcess.ContainsKey(user.UserID))
            {
                return 0;
            }
            int total = _editOrderProcess[user.UserID].GetTotalPrice(user);
            return total;
        }
    }
}
