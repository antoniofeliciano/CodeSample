using Core.Entities.Assessment;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Assessment
{
    public class DatabaseTypeRepository : BaseEntityRepository<DatabaseType>
    {
        public DatabaseTypeRepository(DefaultContext context) : base(context)
        {

        }
    }
}
