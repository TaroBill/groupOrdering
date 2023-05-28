using groupOrdering.Domain;
using static groupOrdering.Technical.DTO;

namespace groupOrdering.Boundary
{
    public interface IGroupBuyingsBoundary
    {
        List<GroupBuyingDTO> ListAllOrders(string serverID);
        public Store getStoreIDByGroupbuyingID(string groupbuyingID);
        int PublishGroupBuying(string storeID, string serverID, DateTime endTime, string userID);
    }
}