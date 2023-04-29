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
        private int StoreID { get; set; }
        private string StoreName { get; set; }
        private string StoreAddress { get; set; }
        private string StorePhoneNumber { get; set; }

        public Store()
        {
            this._menus = new List<StoreMenu>();
            this.StoreID = 0;
            this.StoreName = string.Empty;
            this.StoreAddress = string.Empty;
            this.StorePhoneNumber = string.Empty;
        }

        public string GetStoreName()
        {
            return this.StoreName;
        }

        public string GetStoreID()
        {
            return this.StoreID.ToString();
        }

        public void SetStore(int storeID)
        {
            this.StoreID = storeID;
        }

        public void CreateMenu()
        {
            throw new NotImplementedException();
        }

        public void AddStoreItem(string itemName, int price)
        {
            throw new NotImplementedException();
        }

        public void EndBuildStore(string userID)
        {
            throw new NotImplementedException();
        }

        public List<string> ListItemsOfStore()
        {
            throw new NotImplementedException();
        }
    }
}
