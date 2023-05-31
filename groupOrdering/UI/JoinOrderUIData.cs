using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.UI
{
    public class JoinOrderUIData
    {
        public string OPERATOR { get; set; }
        public string ITEMID { get; set; }
        public int QUANTITY { get; set; }

        public JoinOrderUIData()
        {
            OPERATOR = "-1";
            ITEMID = "-1";
            QUANTITY = 0;
        }
    }
}
