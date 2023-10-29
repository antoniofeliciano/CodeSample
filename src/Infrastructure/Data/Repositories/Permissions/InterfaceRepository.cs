using Core.Entities.Permissions;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Permissions
{
    public class InterfaceRepository : BaseTenantEntityRepository<Interface>
    {
        public InterfaceRepository(DefaultContext context) : base(context)
        {

        }
    }
}
