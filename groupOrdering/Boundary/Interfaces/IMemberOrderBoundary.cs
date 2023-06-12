using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Boundary
{
    public interface IMemberOrderBoundary
    {
        public void DeleteItems(User user, string groupbuyingID);
        public bool SubmitItem(User user, string groupbuyingID, string itemID, int quantity);
        public List<MemberOrder> LoadMemberUserID(string groupbuyingID);
        public List<MemberOrder> LoadMemberOrder(string groupbuyingID, string userID);
    }
}
