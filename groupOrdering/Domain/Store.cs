using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class Store
    {
        private List<StoreMenu> _menus;
        private IStoresBoundary _storesBoundary;
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhoneNumber { get; set; }

        private void InitStore(string storeID = "0", string storeName = "", string storeAddress = "", string storePhoneNumber = "")
        {
            this._storesBoundary = new StoresBoundary();
            this._menus = new List<StoreMenu>();
            this.StoreID = storeID;
            this.StoreName = storeName;
            this.StoreAddress = storeAddress;
            this.StorePhoneNumber = storePhoneNumber;
        }

        public Store()
        {
            InitStore();
        }

        public Store(string storeID, string storeName, string storeAddress, string storePhoneNumber)
        {
            InitStore(storeID, storeName, storeAddress, storePhoneNumber);
        }

        public void SetStoresBoundary(IStoresBoundary storesBoundary)
        {
            this._storesBoundary = storesBoundary;
        }

        public void SetStore(string storeID, string serverID)
        {
            Store store = _storesBoundary.GetStore(storeID, serverID);
            this.StoreID = store.StoreID;
            this.StoreName = store.StoreName;
            this.StoreAddress = store.StoreAddress;
            this.StorePhoneNumber = store.StorePhoneNumber;
        }

        public void CreateMenu()
        {
            throw new NotImplementedException();
        }

        public void AddStoreItem(string itemName, int price)
        {
            throw new NotImplementedException();
        }

        public void EndBuildStore(User user)
        {
            throw new NotImplementedException();
        }

        public List<string> ListItemsOfStore()
        {
            throw new NotImplementedException();
        }
    }
}
