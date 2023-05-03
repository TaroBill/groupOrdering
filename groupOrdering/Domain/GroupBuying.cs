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
        private Store _store;
        private CreateOrderBoundary _createOrderBoundary;
        private string _serverID;
        private DateTime _endTime;
        private List<MemberOrder> _membersOrders;

        public GroupBuying()
        {
            _createOrderBoundary = new CreateOrderBoundary();
            _membersOrders = new List<MemberOrder>();
            _store = new Store();
            _serverID = "";
            _endTime = DateTime.Today;
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

        public void PublishGroupBuying(string userID)
        {
            _createOrderBoundary.PublishGroupBuying(_store.StoreID, _serverID, _endTime, userID);
        }

        public bool CheckEndTime(DateTime endTime)
        {
            return endTime > DateTime.Now;
        }

        public void SetGroupBuying(string userID)
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

        public void AddItem(string userID, string itemID, int quantity, Users users)
        {
            throw new NotImplementedException();
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

    }
}
