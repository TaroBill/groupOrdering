using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class CreateOrderHandler
    {
        private IDictionary<string, GroupBuying> _CreateOrderProcess;

        public GroupBuying? GetGroupBuying(string userID)
        {
            if (!_CreateOrderProcess.ContainsKey(userID))
                return null;
            return _CreateOrderProcess[userID];
        }

        public void CreateGroupBuying(string userID)
        {
            throw new NotImplementedException();
        }

        public List<Store> ListStore(string serverID)
        {
            throw new NotImplementedException();
        }

        public void ChooseExistStore(string userID, int storeID)
        {
            throw new NotImplementedException();
        }

        public void SetEndTime(string userID, DateTime time)
        {
            throw new NotImplementedException();
        }

        public void EndEdit(string userID)
        {
            throw new NotImplementedException();
        }
    }
}
