using groupOrdering.Boundary;
using groupOrdering.Domain;
using groupOrdering.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public class MemberOrderBoundary : IMemberOrderBoundary
    {
        private DAO _dao;

        public MemberOrderBoundary()
        {
            _dao = new DAO();
        }

        public void DeleteItems(User user, string groupbuyingID)
        {
            _dao.SetData($"DELETE FROM groupordering.memberorder " +
                            $"WHERE userID='{user.UserID}' " +
                            $"AND groupbuyingID='{groupbuyingID}';");
        }

        public bool SubmitItem(User user, string groupbuyingID, string itemID, int quantity)
        {
            int result = _dao.SetData(@$"INSERT INTO groupordering.memberorder(userID, groupbuyingID, storeitemID, quantity) 
                                        values('{user.UserID}', '{groupbuyingID}', '{itemID}', '{quantity}');");
            return result != 0;
        }

        public List<MemberOrder> LoadMemberUserID(string groupbuyingID)
        {
            return _dao.GetData<MemberOrder>(@$"SELECT DISTINCT userID AS UserID 
                                                FROM groupordering.memberorder 
                                                WHERE groupbuyingID='{groupbuyingID}';");
        }

        public List<MemberOrder> LoadMemberOrder(string groupbuyingID, string userID)
        {
            return _dao.GetData<MemberOrder>(@$"SELECT userID AS UserID, 
                                                storeitemID AS StoreitemID, 
                                                quantity AS Quantity 
                                                FROM groupordering.memberorder 
                                                WHERE groupbuyingID='{groupbuyingID}' 
                                                AND userID='{userID}';");
        }
    }
}
