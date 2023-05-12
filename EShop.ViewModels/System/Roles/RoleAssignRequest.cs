using eShop.ViewModels.Common;

namespace eShop.ViewModels.System.Roles
{
    public class RoleAssignRequest
    {
        public RoleAssignRequest()
        {
            Roles = new List<SelectItem>();
        }

        public Guid Id { set; get; }
        public List<SelectItem> Roles { set; get; }
    }
}
