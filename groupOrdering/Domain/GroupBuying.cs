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
        private string _name;
        private Store _store;
        private IGroupBuyingsBoundary _groupBuyingsBoundary;
        private string _serverID;
        private DateTime _endTime;
        private List<MemberOrder> _membersOrders;

        private void InitGroupBuying(IGroupBuyingsBoundary boundary, string name = "", string serverID = "")
        {
            _groupBuyingsBoundary = boundary;
            _name = name;
            _serverID = serverID;
            _membersOrders = new List<MemberOrder>();
            _store = new Store();
            _endTime = DateTime.Today;
        }

        public GroupBuying(IGroupBuyingsBoundary boundary)
        {
            InitGroupBuying(boundary);
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string name, string serverID)
        {
            InitGroupBuying(boundary, name, serverID);
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string name, string serverID, DateTime endTime)
        {
            InitGroupBuying(boundary, name, serverID);
            _endTime = endTime;
        }

        public GroupBuying(IGroupBuyingsBoundary boundary, string groupBuyingID)
        {
            throw new NotImplementedException();
        }

        public Store GetStore()
        {
            return _store;
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

        public void ListItemsOfStore()
        {
            throw new NotImplementedException();
        }

        public void AddItem(User user, string itemID, int quantity)
        {
            throw new NotImplementedException();
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

    }
}
