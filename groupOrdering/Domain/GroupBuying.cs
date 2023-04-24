using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class GroupBuying
    {
        private Store _store;
        public DateTime EndTime { get; }

        public GroupBuying()
        {
            _store = new Store();
            EndTime = DateTime.Today;
        }

        public Store GetStore()
        {
            return _store;
        }


        public void ChooseExistStore(int storeID)
        {
            throw new NotImplementedException();
        }

        public void SetEndTime(DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public void PublishGroupBuying()
        {
            throw new NotImplementedException();
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
