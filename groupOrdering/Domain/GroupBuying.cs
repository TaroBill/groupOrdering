﻿using Discord;
using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class GroupBuying
    {
        public string GroupBuyingID { get; set; }
        public string StoreID { get; set; }
        public string GroupBuyingName { get; set; }
        private Store _store;
        private IGroupBuyingsBoundary _groupBuyingsBoundary;
        private string _serverID;
        private DateTime _endTime;
        private string CallerUserID { get; set; }
        private Dictionary<string, MemberOrder> _membersOrders;

        private void InitGroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID = "0", string name = "", string serverID = "", string callerUserID = "")
        {
            GroupBuyingID = groupBuyingID;
            _groupBuyingsBoundary = boundary;
            GroupBuyingName = name;
            _serverID = serverID;
            CallerUserID = callerUserID;
            _membersOrders = new Dictionary<string, MemberOrder>();
            _endTime = DateTime.Today;
            if (groupBuyingID=="0")
            {
                _store = new Store();
            }
            else
            {
                _store = _groupBuyingsBoundary.GetStoreByGroupbuyingID(groupBuyingID);
                _store.setItems(_store.ListItemsOfStore());
            }
        }

        public GroupBuying()
        {
            InitGroupBuying(new GroupBuyingsBoundary());
        }

        public GroupBuying(IGroupBuyingsBoundary boundary)
        {
            InitGroupBuying(boundary);
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID, string name, string serverID)
        {
            InitGroupBuying(boundary, groupBuyingID,  name, serverID);
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID, string name, string serverID, DateTime endTime)
        {
            InitGroupBuying(boundary, groupBuyingID, name, serverID);
            _endTime = endTime;
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID)
        {
            GroupBuying group = boundary.GetGroupBuyingByGroupID(groupBuyingID);
            if (group.GroupBuyingID=="0")
            {
                throw new NullReferenceException("groupbuying not exist");
            }
            InitGroupBuying(boundary, groupBuyingID, group.GroupBuyingName, group._serverID);
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string name, string serverID)
        {
            InitGroupBuying(boundary, "0", name, serverID);
        }

        public Store GetStore()
        {
            return _store;
        }

        public void SetStore(Store store)
        {
            _store = store;
        }

        public DateTime GetEndTime()
        {
            return _endTime;
        }

        public void ChooseExistStore(string storeID, string serverID)
        {
            _serverID = serverID;
            _store.SetStore(storeID, serverID);
        }

        public void SetEndTime(DateTime endTime)
        {
            _endTime = endTime;
        }

        public void PublishGroupBuying(User user)
        {
            _groupBuyingsBoundary.PublishGroupBuying(_store.StoreID, _serverID, _endTime, user.UserID, GroupBuyingName);
        }

        public void SetGroupBuying(User user)
        {
            throw new NotImplementedException();
        }

        public void CreateMemberOrder()
        {
            throw new NotImplementedException();
        }

        public void EndGroupBuying()
        {
            foreach (KeyValuePair<string, MemberOrder> order in _membersOrders) 
            {
                order.Value.CalculateDebt(CallerUserID);
            }
        }

        public void JoinOrder(User user)
        {
            _membersOrders.Add(user.UserID, new MemberOrder());
            _membersOrders[user.UserID].UserID = user.UserID;
        }

        public void AddItem(User user, string itemID, int quantity)
        {
            StoreItem storeItem = _store.GetStoreItem(itemID);
            _membersOrders[user.UserID].AddItem(storeItem, quantity);
        }

        public void EditItem(User user, string itemID, int quantity)
        {
            StoreItem storeItem = _store.GetStoreItem(itemID);
            _membersOrders[user.UserID].EditItem(storeItem, quantity);
        }

        public void DeleteItem(User user, string itemID)
        {
            StoreItem storeItem = _store.GetStoreItem(itemID);
            _membersOrders[user.UserID].DeleteItem(storeItem);
        }

        public bool SubmitOrder(User user)
        {
            return _membersOrders[user.UserID].SubmitOrder(user, GroupBuyingID);
        }

        public int GetTotal(User user)
        {
            return _membersOrders[user.UserID].GetTotal();
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

    }
}
