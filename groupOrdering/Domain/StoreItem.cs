using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class StoreItem
    {
        private readonly string _itemID;
        private readonly string _itemName;
        private readonly int _price;

        public StoreItem(string itemID, string itemName, int price)
        {
            throw new NotImplementedException();
        }

        public int GetPrice(string itemID)
        {
            return _price;
        }
    }
}
