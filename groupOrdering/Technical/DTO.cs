using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Technical
{
    public class DTO
    {
        public class GroupBuyingDTO
        {
            public string groupbuyingID { get; set; }
            public string groupbuyingName { get;set; }
            public string storeID { get; set; }
        }

        public class GroupDTO
        {
            public string groupbuyingID { get; set; }
            public string groupbuyingName { get; set; }
            public string storeID { get; set; }
        }
    }
}
