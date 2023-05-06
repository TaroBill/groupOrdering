using groupOrdering.Domain;

namespace groupOrdering.Boundary
{
    public interface IGroupBuyingsBoundary
    {
        List<GroupBuying> ListAllOrders(string serverID);
    }
}