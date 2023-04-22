using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace groupOrdering.Domain
{
    internal class MemberOrder
    {
        private string _userID;
        private IDictionary<StoreItem, int> _items;

        public MemberOrder()
        {

        }

        public void CalculateDebt()
        {
            throw new NotImplementedException();
        }

        public int GetTotal()
        {
            throw new NotImplementedException();
        }

        public void AddItem(StoreItem item, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
