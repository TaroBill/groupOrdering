﻿using Discord;
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
        private Dictionary<StoreItem, int> _items;
        private MemberOrderBoundary _memberorderboundary;
        private StoresBoundary _storesboundary;

        public MemberOrder()
        {
            UserID = "";
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

        public void CalculateDebt(string callerUserID)
        {
            Users.AddNewDebt(UserID, callerUserID, GetTotal());
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
            AddItem(item, quantity);
        }

        public void DeleteItem(StoreItem item)
        {
            _items.Remove(item);
        }
    }
}
