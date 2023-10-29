using Core.Entities.Authentication;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Permissions
{
    public class RoleRepository : BaseTenantEntityRepository<Role>
    {
        public RoleRepository(DefaultContext context) : base(context)
        {

        }
    }
}
