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
        private List<StoreItem> _items;
        private IStoresBoundary _storesBoundary;
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhoneNumber { get; set; }
        public string ServerID { get; set; }

        public Store()
        {
            this._storesBoundary = new StoresBoundary();
            this._items = new List<StoreItem>();
            this.StoreID = "0";
            this.ServerID = "0";
            this.StoreName = "";
            this.StoreAddress = "";
            this.StorePhoneNumber = "";
        }

        public Store(string storeID, string storeName, string storeAddress, string storePhoneNumber)
        {
            this._storesBoundary = new StoresBoundary();
            this._items = new List<StoreItem>();
            this.ServerID = "0";
            this.StoreID = storeID;
            this.StoreName = storeName;
            this.StoreAddress = storeAddress;
            this.StorePhoneNumber = storePhoneNumber;
        }

        public void SetItems(List<StoreItem> storeItems)
        {
            _items = storeItems;
        }

        public void SetStoresBoundary(IStoresBoundary storesBoundary)
        {
            this._storesBoundary = storesBoundary;
        }

        public StoreItem GetStoreItem(string storeitemID)
        {
            return _items.FirstOrDefault(x => x.StoreitemID == storeitemID, new StoreItem());
        }

        public void SetStore(string storeID, string serverID)
        {
            Store store = _storesBoundary.GetStore(storeID, serverID);
            _items = _storesBoundary.ListItemsOfStore(storeID);
            this.StoreID = store.StoreID;
            this.StoreName = store.StoreName;
            this.StoreAddress = store.StoreAddress;
            this.StorePhoneNumber = store.StorePhoneNumber;
        }

        public void DeleteStoreItemAt(int num)
        {
            if (num >= _items.Count)
                return;
            _items.RemoveAt(num);
        }

        public void DeleteStoreItem(string itemName)
        {
            foreach (StoreItem item in _items)
            {
                if (item.StoreitemName == itemName)
                {
                    _items.Remove(item);
                    return;
                }
            }
        }

        public bool IsInStoreItemList(int num)
        {
            return (num < _items.Count) && (num >= 0);
        }

        public int GetStoreItemCount()
        {
            return (_items.Count);
        }

        public void AddStoreItem(string itemName, int price)
        {
            StoreItem item = new StoreItem(itemName, price);
            _items.Add(item);
        }

        /// <summary>
        /// 專門給test case使用
        /// </summary>
        /// <param name="item"></param>
        public void AddStoreItem(StoreItem item)
        {
            _items.Add(item);
        }

        public void EndBuildStore()
        {
            _storesBoundary.SaveStoreData(this);
        }

        /// <summary>
        /// 從資料庫取得商品
        /// </summary>
        /// <returns></returns>
        public List<StoreItem> ListItemsOfStore()
        {
            return _storesBoundary.ListItemsOfStore(StoreID);
        }

        /// <summary>
        /// 從記憶體取得商品(僅限建立店家使用)
        /// </summary>
        /// <returns></returns>
        public List<StoreItem> GetStoreItems()
        {
            return _items;
        }
    }
}
