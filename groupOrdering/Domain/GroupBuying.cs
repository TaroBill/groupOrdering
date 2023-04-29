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
        private string _storeID;
        private string _serverID;
        private DateTime _endTime;

        public GroupBuying()
        {
            _createOrderBoundary = new CreateOrderBoundary();
            _store = new Store();
            _storeID = "";
            _serverID = "";
            _endTime = DateTime.Parse("2000-01-01");
        }

        public void ChooseExistStore(int storeID, string serverID)
        {
            _storeID = storeID.ToString();
            _serverID = serverID;
            _store.SetStore(storeID);
        }

        public void SetEndTime(DateTime endTime)
        {
            _endTime = endTime;
        }

        public bool PublishGroupBuying(string userID)
        {
            if (_endTime == DateTime.Parse("2000-01-01"))
            {
                return false;
            }
            else
            {
                return _createOrderBoundary.PublishGroupBuying(_storeID.ToString(), _serverID, _endTime, userID)==1;
            }
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

        public void AddItem(string userID, string itemID, int quantity)
        {
            throw new NotImplementedException();
        }

        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }

    }
}
