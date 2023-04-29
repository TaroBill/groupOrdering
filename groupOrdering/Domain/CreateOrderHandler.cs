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

        public string SetEndTime(string userID, DateTime time)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return "尚未挑選店家";
            }
            else
            {
                _CreateOrderProcess[userID].SetEndTime(time);
                return "已設定團購結束時間";
            }
        }

        public string EndEdit(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
            {
                return "尚未挑選店家";
            }
            else
            {
                if (_CreateOrderProcess[userID].PublishGroupBuying(userID))
                {
                    _CreateOrderProcess.Remove(userID);
                    return "已建立團購";
                }
                else
                {
                    return "尚未設定團購結束時間";
                }
            }
        }
    }
}
