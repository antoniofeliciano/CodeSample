using Core.Entities.Permissions;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Permissions
{
    public class AreaRepository : BaseTenantEntityRepository<Area>
    {
        public AreaRepository(DefaultContext context) : base(context)
        {

        }
    }
}
