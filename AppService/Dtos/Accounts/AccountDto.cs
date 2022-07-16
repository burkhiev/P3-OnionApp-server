namespace AppService.Dtos.Accounts
{
    public sealed class AccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
