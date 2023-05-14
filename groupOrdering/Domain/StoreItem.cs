using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class StoreItem
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }

        public StoreItem(string itemID, string itemName, int price)
        {
            throw new NotImplementedException();
        }
    }
}
