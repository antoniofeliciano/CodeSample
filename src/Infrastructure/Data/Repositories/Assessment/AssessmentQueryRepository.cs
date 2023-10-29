using Core.Entities.Assessment;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Assessment
{
    public class AssessmentQueryRepository : BaseTenantEntityRepository<AssessmentQuery>
    {
        public AssessmentQueryRepository(DefaultContext context) : base(context)
        {

        }
    }
}
