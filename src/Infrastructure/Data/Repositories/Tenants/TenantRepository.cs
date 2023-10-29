using Core.Entities.Tenants;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Tenants
{
    public class TenantRepository : BaseEntityRepository<Tenant>
    {
        public TenantRepository(DefaultContext context) : base(context)
        {

        }
    }
}
