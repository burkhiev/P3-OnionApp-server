namespace AppService.Dtos.Accounts
{
    public sealed class AccountCreatingDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? SecondName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
    }
}
