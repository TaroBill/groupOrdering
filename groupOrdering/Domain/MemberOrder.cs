using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace groupOrdering.Domain
{
    public class MemberOrder
    {
        private string _userID;
        private Dictionary<StoreItem, int> _items;
        private Users _users;

        public MemberOrder(Users users)
        {
            _users = users;
            _items = new Dictionary<StoreItem, int>();
            _userID = string.Empty;
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
