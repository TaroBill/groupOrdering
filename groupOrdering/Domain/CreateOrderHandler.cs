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

        public void CreateGroupBuying(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                _CreateOrderProcess.Add(userID, new GroupBuying());
            }
        }

        public string ListStore()
        {
            string stores = "";
            List<Store> list = _DataCatalog.ListStores();
            foreach (Store store in list)
            {
                stores += String.Format("{0}  {1}", store.GetStoreID(), store.GetStoreName()) + '\n';
            }
            return stores;
        }

        public void ChooseExistStore(string userID, string storeID, string serverID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                _CreateOrderProcess.Add(userID, new GroupBuying());
            }
            _CreateOrderProcess[userID] = new GroupBuying();
            _CreateOrderProcess[userID].ChooseExistStore(Int32.Parse(storeID), serverID);
        }

        public void SetEndTime(string userID, DateTime time)
        {
            _CreateOrderProcess[userID].SetEndTime(time);
        }

        public void EndEdit(string userID)
        {
            _CreateOrderProcess[userID].PublishGroupBuying(userID);
            _CreateOrderProcess.Remove(userID);
        }

        public bool CheckChooseStore(string userID)
        {
            return _CreateOrderProcess.ContainsKey(userID) && _CreateOrderProcess[userID].GetStore().GetStoreID() != "0";
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
            return _CreateOrderProcess[userID].GetEndTime() != DateTime.Parse("2000-01-01");
        }
    }
}
