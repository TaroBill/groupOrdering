using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace groupOrdering.Boundary
{
    public class StoresBoundary : IStoresBoundary
    {
        private DAO _dao;
        public StoresBoundary()
        {
            _dao = new DAO();
        }

        public List<Store> ListStores(string serverID)
        {
            return _dao.GetData<Store>(@$"SELECT storeID AS StoreID, 
                                        storeName AS StoreName, 
                                        storeAddress AS StoreAddress, 
                                        storePhoneNumber AS StorePhoneNumber 
                                        FROM groupordering.store 
                                        WHERE serverID='{serverID}';");
        }

        public Store GetStore(string storeID, string serverID)
        {
            return _dao.GetData<Store>(@$"SELECT storeID AS StoreID, 
                                        storeName AS StoreName, 
                                        storeAddress AS StoreAddress, 
                                        storePhoneNumber AS StorePhoneNumber 
                                        FROM groupordering.store 
                                        WHERE storeID='{storeID}' 
                                        AND serverID='{serverID}';").FirstOrDefault(new Store());
        }

        public List<StoreItem> ListItemsOfStore(string storeID)
        {
            return _dao.GetData<StoreItem>(@$"SELECT storeitem.storeitemID AS StoreitemID, 
                                            storeitem.storeitemName AS StoreitemName, 
                                            storeitem.storeitemPrice AS StoreitemPrice 
                                            FROM groupordering.storeitem 
                                            WHERE storeitem_storeID='{storeID}';");
        }

        public void SaveStoreData(Store store)
        {
            _dao.SetData($@"INSERT INTO `groupordering`.`store` (`storeName`, `storeAddress`, `storePhoneNumber`, `serverID`) 
                            VALUES ('{store.StoreName}', '{store.StoreAddress}', '{store.StorePhoneNumber}', '{store.ServerID}');");
            string storeID = _dao.GetData<string>($@"SELECT storeID FROM groupordering.store 
                                                     WHERE serverID='{store.ServerID}' and storeName='{store.StoreName}' and storeAddress='{store.StoreAddress}' and storePhoneNumber='{store.StorePhoneNumber}';").FirstOrDefault("0");
            List<StoreItem> items = store.GetStoreItems();
            foreach (StoreItem item in items)
            {
                SaveStoreItemData(item, storeID);
            }

        }
        private void SaveStoreItemData(StoreItem item, string store_ID)
        {
            _dao.SetData($@"INSERT INTO `groupordering`.`storeitem` (`storeitemName`, `storeitemPrice`, `storeitem_storeID`) 
                            VALUES ('{item.StoreitemName}', '{item.StoreitemPrice}', '{store_ID}');");
        }
    }
}
