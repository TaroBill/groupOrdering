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

        public CreateOrderHandler()
        {
            _CreateOrderProcess = new Dictionary<string, GroupBuying>();
        }

        public GroupBuying? GetGroupBuying(User user)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                return null;
            }
            return _CreateOrderProcess[user.UserID];
        }

        public void CreateGroupBuying(User user)
        {
            if (!_CreateOrderProcess.ContainsKey(user.UserID))
            {
                _CreateOrderProcess.Add(user.UserID, new GroupBuying());
            }
        }

        public string ListStore(string serverID)
        {
            string stores = "";
            List<Store> list = Stores.ListStores(serverID);
            foreach (Store store in list)
            {
                stores += $"{store.StoreID}  {store.StoreName}\n";
            }
            return stores;
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
