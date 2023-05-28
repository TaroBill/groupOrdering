using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static groupOrdering.Technical.DTO;

namespace groupOrdering.Domain
{
    public class GroupBuying
    {
        public string GroupBuyingID { get; set; }

        private string groupbuyingName;
        private Store _store;
        private IGroupBuyingsBoundary _groupBuyingsBoundary;
        private string _serverID;
        private DateTime _endTime;
        private MemberOrder _membersOrders;

        private void InitGroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID = "0", string name = "", string serverID = "")
        {
            GroupBuyingID = groupBuyingID;
            _groupBuyingsBoundary = boundary;
            groupbuyingName = name;
            _serverID = serverID;
            _membersOrders = new MemberOrder();
            _store = new Store();
            _endTime = DateTime.Today;
        }

        public GroupBuying()
        {

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

        public GroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID, string serverID)
        {
            InitGroupBuying(boundary, groupBuyingID, "", serverID);
        }

        public string getGroupbuyingName()
        {
            return this.groupbuyingName;
        }

        public string getGroupbuyingID()
        {
            return this.GroupBuyingID;
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

        public string getStoreIDByGroupbuyingID(string groupbuyingID)
        {
            return _groupBuyingsBoundary.getStoreIDByGroupbuyingID(groupbuyingID).StoreID;
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
            _groupBuyingsBoundary.PublishGroupBuying(_store.StoreID, _serverID, _endTime, user.UserID);
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
            throw new NotImplementedException();
        }

        public List<StoreItem> ListItemsOfStore()
        {
            return _store.ListItemsOfStore();
        }

        public void AddItem(string itemID, int quantity)
        {
            _membersOrders.AddItem(itemID, quantity);
        }

        public void EditItem(string itemID, int quantity)
        {
            _membersOrders.EditItem(itemID, quantity);
        }

        public void DeleteItem(User user, string itemID)
        {
            _membersOrders.DeleteItem(user, GroupBuyingID, itemID);
        }

        public int SubmitOrder(User user)
        {
            return _membersOrders.SubmitOrder(user, GroupBuyingID);
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

    }
}
