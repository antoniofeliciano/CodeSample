using Core.Entities.Authentication;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Authentication
{
    public class SystemApiKeyRepository : BaseTenantEntityRepository<SystemApiKey>
    {
        public SystemApiKeyRepository(DefaultContext context) : base(context)
        {

        }
    }
}
