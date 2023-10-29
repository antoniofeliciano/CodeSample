using Core.Entities.Authentication;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Authentication
{
    public class UserRepository : BaseTenantEntityRepository<User>
    {
        public UserRepository(DefaultContext context) : base(context)
        {

        }
    }
}
