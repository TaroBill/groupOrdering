using Discord;
using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace groupOrdering.Domain
{
    public class MemberOrder
    {
        public string UserID { get; set; }
        public string StoreitemID { get; set; }
        public int Quantity { get; set; }
        private Dictionary<StoreItem, int> _items;
        private MemberOrderBoundary _memberorderboundary;
        private StoresBoundary _storesboundary;

        public MemberOrder()
        {
            UserID = "";
            StoreitemID = "";
            Quantity = 0;
            _items = new Dictionary<StoreItem, int>();
            _memberorderboundary = new MemberOrderBoundary();
            _storesboundary = new StoresBoundary();
        }

        public bool SubmitOrder(User user, string groupbuyingID)
        {
            try
            {
                _memberorderboundary.DeleteItems(user, groupbuyingID);
                foreach (var pair in _items)
                {
                    _memberorderboundary.SubmitItem(user, groupbuyingID, pair.Key.StoreitemID, pair.Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void LoadMemberOrder(string groupbuyingID, string userID, Store store)
        {
            List<MemberOrder> memberOrders = _memberorderboundary.LoadMemberOrder(groupbuyingID, userID);
            UserID = memberOrders[0].UserID;
            foreach (var memberOrder in memberOrders)
            {
                _items[store.GetStoreItem(memberOrder.StoreitemID)] = memberOrder.Quantity;
            }
        }

        public void CalculateDebt(string callerUserID)
        {
            Users.AddNewDebt(callerUserID, UserID, GetTotal());
        }

        public string OrderToString()
        {
            string result = $"{UserID}\n";
            foreach (var pair in _items)
            {
                result += $"\t{pair.Key.StoreitemName} X {pair.Value}\n";
            }
            return result;
        }

        public int GetTotal()
        {
            int total = 0;
            foreach(var pair in _items)
            {
                total += pair.Key.StoreitemPrice * pair.Value;
            }
            return total;
        }

        public void AddItem(StoreItem item, int quantity)
        {
            _items[item] = _items.GetValueOrDefault(item, 0);
            _items[item] += quantity;
        }

        public void EditItem(StoreItem item, int quantity)
        {
            _items[item] = quantity;
        }

        public void DeleteItem(StoreItem item)
        {
            _items.Remove(item);
        }
    }
}
