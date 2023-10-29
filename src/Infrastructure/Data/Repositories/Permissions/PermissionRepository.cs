using Core.Entities.Permissions;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Permissions
{
    public class PermissionRepository : BaseTenantEntityRepository<Permission>
    {
        public PermissionRepository(DefaultContext context) : base(context)
        {

        }
    }
}
