using groupOrdering.Domain;

namespace groupOrdering.Boundary.Interfaces
{
    public interface IStoreItemBoundary
    {
        StoreItem getStoreItem(string storeutemID);
    }
}