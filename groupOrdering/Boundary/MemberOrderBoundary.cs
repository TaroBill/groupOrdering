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

        public void DeleteItem(User user, string groupbuyingID, string itemID)
        {
            _dao.SetData($"DELETE FROM groupordering.memberorder " +
                            $"WHERE userID='{user.UserID}' " +
                            $"AND groupbuyingID='{groupbuyingID}' " +
                            $"AND storeitemID='{itemID}';");
        }

        public bool SubmitOrder(User user, string groupbuyingID, string itemID, int quantity)
        {
            DeleteItem(user, groupbuyingID, itemID);
            int result = _dao.SetData($"INSERT INTO groupordering.memberorder(userID, groupbuyingID, storeitemID, quantity) " +
                                        $"values('{user.UserID}', '{groupbuyingID}', '{itemID}', '{quantity}');");
            return result != 0;
        }
    }
}
