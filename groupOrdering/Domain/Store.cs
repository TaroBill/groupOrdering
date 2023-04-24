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
        public int StoreID { get; }
        public string StoreName { get; }
        public string StoreAddress { get; }
        public string StorePhoneNumber { get; }

        public Store()
        {
            _menus = new List<StoreMenu>();
            StoreID = 0;
            StoreName = string.Empty;
            StoreAddress = string.Empty;
            StorePhoneNumber = string.Empty;
        }

        public void SetStore(int storeID)
        {
            throw new NotImplementedException();
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
