using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Audit
{
    public class Change
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string? ModifiedProperties { get; set; }
    }
}
