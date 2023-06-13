using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class StoreItem
    {
        public string StoreitemID { get; set; }
        public string StoreitemName { get; set; }
        public int StoreitemPrice { get; set; }

        public StoreItem()
        {
            StoreitemID = "0";
            StoreitemName = "";
            StoreitemPrice = 0;
        }

        public StoreItem(string storeitemName, int storeitemPrice)
        {
            this.StoreitemName = storeitemName;
            this.StoreitemPrice = storeitemPrice;
        }

        public StoreItem(string storeitemID, string storeitemName, int storeitemPrice)
        {
            this.StoreitemID = storeitemID;
            this.StoreitemName = storeitemName;
            this.StoreitemPrice = storeitemPrice;
        }
    }
}
