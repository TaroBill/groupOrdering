using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class CreateOrderHandler
    {
        private Dictionary<string, GroupBuying> _CreateOrderProcess;
        private IGroupBuyingsBoundary _groupBuyingsBoundary;

        public CreateOrderHandler()
        {
            _CreateOrderProcess = new Dictionary<string, GroupBuying>();
            _groupBuyingsBoundary = new GroupBuyingsBoundary();
        }

        public void SetGroupBuyingsBoundary(IGroupBuyingsBoundary groupBuyingBoundary)
        {
            _groupBuyingsBoundary = groupBuyingBoundary;
        }

        public GroupBuying? GetGroupBuying(User user)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return null;
            }
            return _CreateOrderProcess[user.UserID];
        }

        public void CreateGroupBuying(User user, string name, string serverID)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                _CreateOrderProcess.Add(user.UserID, new GroupBuying(_groupBuyingsBoundary, name, serverID));
            }
        }

        public List<Store> ListStore(string serverID)
        {
            if (serverID == string.Empty)
                return new List<Store>();
            List<Store> list = Stores.ListStores(serverID);
            return list;
        }

        public void ChooseExistStore(User user, string storeID, string serverID)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _CreateOrderProcess[user.UserID].ChooseExistStore(storeID, serverID);
        }

        public void SetEndTime(User user, DateTime time)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _CreateOrderProcess[user.UserID].SetEndTime(time);
        }

        public void EndEdit(User user)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _CreateOrderProcess[user.UserID].PublishGroupBuying(user);
            _CreateOrderProcess.Remove(user.UserID);
        }

        public bool CheckStartOrder(User user)
        {
            return _CreateOrderProcess.ContainsKey(user.UserID);
        }

        public bool CheckChooseStore(User user)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            return _CreateOrderProcess[user.UserID].GetStore().StoreID != "0";
        }

        public bool CheckEndTime(DateTime endTime)
        {
            return endTime > DateTime.Now;
        }

        public bool CheckEndTimeValid(User user)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            return CheckEndTime(_CreateOrderProcess[user.UserID].GetEndTime());
        }
    }
}
