namespace AppDomain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } = Guid.Empty;
    }
}
