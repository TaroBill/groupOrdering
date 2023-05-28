using groupOrdering.Domain;

namespace groupOrdering.Boundary
{
    public interface IStoresBoundary
    {
        Store GetStore(string storeID, string serverID);
        List<Store> ListStores(string serverID);
    }
}