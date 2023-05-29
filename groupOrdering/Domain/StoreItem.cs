using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class StoreItem
    {
        public string storeitemID { get; set; }
        public string storeitemName { get; set; }
        public int storeitemPrice { get; set; }

        public StoreItem()
        {

        }

        public StoreItem(string storeitemName, int storeitemPrice)
        {
            this.storeitemName = storeitemName;
            this.storeitemPrice = storeitemPrice;
        }

        public StoreItem(string storeitemID, string storeitemName, int storeitemPrice)
        {
            this.storeitemID = storeitemID;
            this.storeitemName = storeitemName;
            this.storeitemPrice = storeitemPrice;
        }
    }
}
