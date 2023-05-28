﻿using groupOrdering.Domain;

namespace groupOrdering.Boundary
{
    public interface IGroupBuyingsBoundary
    {
        List<GroupBuying> ListAllOrders(string serverID);
        public Store getStoreIDByGroupbuyingID(string groupbuyingID);
        int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID);
    }
}