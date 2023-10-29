using Core.Entities.Assessment;
using Core.Entities.Authentication;
using Core.Entities.Permissions;
using Core.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {

        }

        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Interface> Interface { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<SystemApiKey> SystemApiKey { get; set; }



        public virtual DbSet<AssessmentQuery> AssessmentQuery { get; set; }
        public virtual DbSet<AssessmentCollect> AssessmentCollect { get; set; }
        public virtual DbSet<DatabaseType> DatabaseType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).ToList().ForEach(e => { e.DeleteBehavior = DeleteBehavior.Restrict; });
        }
    }
}
