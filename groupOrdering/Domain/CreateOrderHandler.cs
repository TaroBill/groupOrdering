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
        private DataCatalog _DataCatalog;

        public CreateOrderHandler()
        {
            _DataCatalog = new DataCatalog();
            _CreateOrderProcess = new Dictionary<string, GroupBuying>();
        }

        public GroupBuying? GetGroupBuying(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return null;
            }
            return _CreateOrderProcess[userID];
        }

        public void CreateGroupBuying(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                _CreateOrderProcess.Add(userID, new GroupBuying());
            }
        }

        public string ListStore(string serverID)
        {
            string stores = "";
            List<Store> list = _DataCatalog.ListStores(serverID);
            foreach (Store store in list)
            {
                stores += $"{store.StoreID}  {store.StoreName}\n";
            }
            return stores;
        }

        public void ChooseExistStore(string userID, string storeID, string serverID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return;
            }
            _CreateOrderProcess[userID].ChooseExistStore(storeID, serverID);
        }

        public void SetEndTime(string userID, DateTime time)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return;
            }
            _CreateOrderProcess[userID].SetEndTime(time);
        }

        public void EndEdit(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return;
            }
            _CreateOrderProcess[userID].PublishGroupBuying(userID);
            _CreateOrderProcess.Remove(userID);
        }

        public bool CheckStartOrder(string userID)
        {
            return _CreateOrderProcess.ContainsKey(userID);
        }

        public bool CheckChooseStore(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return false;
            }
            return _CreateOrderProcess[userID].GetStore().StoreID != "0";
        }

        public bool CheckEndTime(string userID, DateTime endTime)
        {
            return _CreateOrderProcess[userID].CheckEndTime(endTime);
        }

        public bool CheckOrderValid(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return false;
            }
            return _CreateOrderProcess[userID].GetEndTime() != DateTime.Today;
        }
    }
}
