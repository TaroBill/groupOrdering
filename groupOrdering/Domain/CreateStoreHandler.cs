using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class CreateStoreHandler
    {
        private readonly Dictionary<string, Store> _createStoreProcess;

        public CreateStoreHandler()
        {
            _createStoreProcess = new Dictionary<string, Store>();
        }

        public void CreateStore(User user, string serverID, string storeName, string storeAddress, string storePhoneNumber)
        {
            _createStoreProcess[user.UserID] = _createStoreProcess.GetValueOrDefault(user.UserID, new Store());
            _createStoreProcess[user.UserID].ServerID = serverID;
            _createStoreProcess[user.UserID].StoreName = storeName;
            _createStoreProcess[user.UserID].StoreAddress = storeAddress;
            _createStoreProcess[user.UserID].StorePhoneNumber = storePhoneNumber;
        }

        public void AddStoreItem(User user, string itemName, int money)
        {
            if (!_createStoreProcess.ContainsKey(user.UserID))
                return;
            _createStoreProcess[user.UserID].AddStoreItem(itemName, money);
        }

        public void DeleteStoreItem(User user, string itemName)
        {
            if (!_createStoreProcess.ContainsKey(user.UserID))
                return;
            _createStoreProcess[user.UserID].DeleteStoreItem(itemName);
        }

        public void EndBuildStore(User user)
        {
            if (!_createStoreProcess.ContainsKey(user.UserID))
                return;
            _createStoreProcess[user.UserID].EndBuildStore();
            _createStoreProcess.Remove(user.UserID);
        }

        public void EditStoreName(User user, string storeName)
        {
            if (!_createStoreProcess.ContainsKey(user.UserID))
                return;
            _createStoreProcess[user.UserID].StoreName = storeName;
        }

        public int GetTotalStoreItemCount(User user)
        {
            if (!_createStoreProcess.ContainsKey(user.UserID))
                return 0;
            return _createStoreProcess[user.UserID].GetStoreItemCount();
        }

        public Store GetStore(User user)
        {
            if (!_createStoreProcess.ContainsKey(user.UserID))
                return new Store();
            return _createStoreProcess[user.UserID];
        }
    }
}
