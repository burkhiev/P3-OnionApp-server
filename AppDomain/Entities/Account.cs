﻿namespace AppDomain.Entities
{
    public class Account : EntityBase
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? AccountType { get; set; } = string.Empty;
        public virtual User User { get; set; }
    }
}
