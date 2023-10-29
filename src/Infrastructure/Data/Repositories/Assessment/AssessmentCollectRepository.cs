using Core.Entities.Assessment;
using Infrastructure.Data.Repositories.Bases;

namespace Infrastructure.Data.Repositories.Assessment
{
    public class AssessmentCollectRepository : BaseTenantEntityRepository<AssessmentCollect>
    {
        public AssessmentCollectRepository(DefaultContext context) : base(context)
        {

        }
    }
}
