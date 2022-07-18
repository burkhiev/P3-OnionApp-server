namespace AppService.Dtos.Accounts
{
    public class AccountUpdatingDto : DtoBase
    {
        public Guid AccountId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
