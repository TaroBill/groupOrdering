﻿using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class Store
    {
        private List<StoreMenu> _menus;
        private StoresBoundary _storesBoundary;
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhoneNumber { get; set; }

        public Store()
        {
            this._storesBoundary = new StoresBoundary();
            this._menus = new List<StoreMenu>();
            this.StoreID = "0";
            this.StoreName = string.Empty;
            this.StoreAddress = string.Empty;
            this.StorePhoneNumber = string.Empty;
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
