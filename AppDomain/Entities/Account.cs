using System.ComponentModel.DataAnnotations;

namespace AppDomain.Entities
{
    public class Account : EntityBase
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public string? AccountType { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
