namespace AppDomain.Entities
{
    public class User : EntityBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
