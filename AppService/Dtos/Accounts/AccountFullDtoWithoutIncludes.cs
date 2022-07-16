﻿using AppDomain.Entities;

namespace AppService.Dtos.Accounts
{
    public class AccountFullDtoWithoutIncludes
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? AccountType { get; set; } = string.Empty;
    }
}
