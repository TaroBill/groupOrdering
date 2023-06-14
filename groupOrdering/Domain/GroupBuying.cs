using Discord;
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
        private IMemberOrderBoundary _memberOrderBoundary;
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
            _memberOrderBoundary = new MemberOrderBoundary();
            _endTime = DateTime.Today;
            if (groupBuyingID=="0")
            {
                _store = new Store();
            }
            else
            {
                _store = _groupBuyingsBoundary.GetStoreByGroupbuyingID(groupBuyingID);
                _store.SetItems(_store.ListItemsOfStore());
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
            CallerUserID = user.UserID;
            GroupBuying data = _groupBuyingsBoundary.GetGroupBuyingByGroupID(user.UserID, GroupBuyingID);
            if (data.GroupBuyingID == "0")
            {
                throw new NullReferenceException("GroupBuying is not exist");
            }
            InitGroupBuying(data._groupBuyingsBoundary, data.GroupBuyingID, data.GroupBuyingName, data._serverID, data.CallerUserID);
            List<MemberOrder> memberOrders = _memberOrderBoundary.LoadMemberUserID(GroupBuyingID);
            foreach (MemberOrder memberOrder in memberOrders)
            {
                _membersOrders[memberOrder.UserID] = new MemberOrder();
                _membersOrders[memberOrder.UserID].LoadMemberOrder(GroupBuyingID, memberOrder.UserID, _store);
            }
        }

        public void SelectGroupBuying(User user)
        {
            _membersOrders[user.UserID] = new MemberOrder();
            _membersOrders[user.UserID].LoadMemberOrder(GroupBuyingID, user.UserID, _store);
        }

        public string EndGroupBuying()
        {
            string result = "";
            int total=0;
            foreach (KeyValuePair<string, MemberOrder> order in _membersOrders) 
            {
                order.Value.CalculateDebt(CallerUserID);
                result += order.Value.OrderToString();
                total += order.Value.GetTotal();
            }
            _groupBuyingsBoundary.EndGroupBuying(GroupBuyingID);
            result += $"一共 : {total}元\n";
            return result;
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

        public int GetTotalPrice(User user)
        {
            return _membersOrders[user.UserID].GetTotal();
        }

        public MemberOrder GetMemberOrder(User user)
        {
            if (!_membersOrders.ContainsKey(user.UserID))
            {
                return new MemberOrder();
            }
            return _membersOrders[user.UserID];
        }
    }
}
