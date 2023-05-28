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
        private Dictionary<StoreItem, int> _items;
        private MemberOrderBoundary _memberorderboundary;
        private StoreItemBoundary _storeitemboundary;

        public MemberOrder()
        {
            _items = new Dictionary<StoreItem, int>();
            _memberorderboundary = new MemberOrderBoundary();
            _storeitemboundary = new StoreItemBoundary();
        }

        public int SubmitOrder(User user, string groupbuyingID)
        {
            foreach(var pair in _items)
            {
                _memberorderboundary.SubmitOrder(user, groupbuyingID, pair.Key.storeitemID, pair.Value);
            }            
            return GetTotal();
        }

        public void CalculateDebt()
        {
            throw new NotImplementedException();
        }

        public int GetTotal()
        {
            int total = 0;
            foreach(var pair in _items)
            {
                total += pair.Key.storeitemPrice * pair.Value;
                Console.WriteLine($"{pair.Key.storeitemPrice}, {pair.Value}, {total}");
            }
            return total;
        }

        public void AddItem(string itemID, int quantity)
        {
            foreach (var pair in _items)
            {
                if (pair.Key.storeitemID == itemID)
                {
                    _items[pair.Key] += quantity;
                    return;
                }
            }
            StoreItem storeItem = _storeitemboundary.getStoreItem(itemID);
            _items.Add(storeItem, quantity);
        }

        public void EditItem(string itemID, int quantity)
        {
            foreach (var pair in _items)
            {
                if (pair.Key.storeitemID == itemID)
                {
                    _items[pair.Key] = quantity;
                    return;
                }
            }
        }

        public void DeleteItem(User user, string groupbuyingID, string itemID)
        {
            foreach (var pair in _items)
            {
                if (pair.Key.storeitemID == itemID)
                {
                    _items.Remove(pair.Key);
                    return;
                }
            }
            _memberorderboundary.DeleteItem(user, groupbuyingID, itemID);
        }
    }
}
