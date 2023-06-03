using groupOrdering.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Domain
{
    public class CreateOrderHandler
    {
        private readonly Dictionary<string, GroupBuying> _createOrderProcess;

        private IGroupBuyingsBoundary _groupBuyingsBoundary;

        public CreateOrderHandler()
        {
            _createOrderProcess = new Dictionary<string, GroupBuying>();
            _groupBuyingsBoundary = new GroupBuyingsBoundary();
        }

        public void SetGroupBuyingsBoundary(IGroupBuyingsBoundary groupBuyingBoundary)
        {
            _groupBuyingsBoundary = groupBuyingBoundary;
        }

        public GroupBuying? GetGroupBuying(User user)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return null;
            }
            return _createOrderProcess[user.UserID];
        }

        public void CreateGroupBuying(User user, string name, string serverID)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                _createOrderProcess.Add(user.UserID, new GroupBuying(_groupBuyingsBoundary, name, serverID));
            }
        }

        public List<Store> ListStore(string serverID)
        {
            if (serverID == string.Empty)
                return new List<Store>();
            List<Store> list = Stores.ListStores(serverID);
            return list;
        }

        public void ChooseExistStore(User user, string storeID, string serverID)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _createOrderProcess[user.UserID].ChooseExistStore(storeID, serverID);
        }

        public void SetEndTime(User user, DateTime time)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _createOrderProcess[user.UserID].SetEndTime(time);
        }

        public void SetGroupBuyingName(User user, string name)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _createOrderProcess[user.UserID].GroupBuyingName = name;
        }

        public void EndEdit(User user)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return;
            }
            _createOrderProcess[user.UserID].PublishGroupBuying(user);
            _createOrderProcess.Remove(user.UserID);
        }

        public bool CheckStartOrder(User user)
        {
            return _createOrderProcess.ContainsKey(user.UserID);
        }

        public bool CheckChooseStore(User user)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            return _createOrderProcess[user.UserID].GetStore().StoreID != "0";
        }

        public bool CheckEndTime(DateTime endTime)
        {
            return endTime > DateTime.Now;
        }

        public bool CheckEndTimeValid(User user)
        {
            if (!_createOrderProcess.ContainsKey(user.UserID))
            {
                return false;
            }
            return CheckEndTime(_createOrderProcess[user.UserID].GetEndTime());
        }
    }
}
