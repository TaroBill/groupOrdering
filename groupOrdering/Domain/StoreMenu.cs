using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class StoreMenu
    {
        private List<StoreItem> _items;

        public StoreMenu() 
        {
            _items = new List<StoreItem>();
        }

        public void AddItem(string itemName, int price)
        {
            throw new NotImplementedException();
        }
    }
}
