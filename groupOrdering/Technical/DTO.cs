using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Technical
{
    public class DTO
    {
        public class StoreDTO
        {
            public int storeID { get; set; }
            public string storeName { get; set; }
            public string storeAddress { get; set; }
            public string storePhoneNumber { get; set; }
            public string serverID { get; set; }
        }

        public class StoreMenuDTO
        {
            public int storeID { get; set; }
            public int storemenuID { get; set; }
        }

        public class StoreItemDTO
        {
            public int storeitemID { get; set; }
            public string storeitenName { get; set; }
            public int storeitenPrice { get; set; }
            public int storemenuID { get; set; }
        }

        public class GroupBuyingID
        {
            public int groupbuyingID { get; set; }
            public int storeID { get; set; }
            public int status { get; set; }
            public string serverID { get; set; }
        }
    }
}
