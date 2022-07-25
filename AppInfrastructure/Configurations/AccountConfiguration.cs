using AppDomain.Entities;
using AppInfrastructure.Data;
using AppInfrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppInfrastructure.Database.Configurations
{
    public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public const int MAX_ACCOUNT_PASSWORD_LEGNTH = 100;
        public const int MIN_ACCOUNT_PASSWORD_LEGNTH = 6;
        public const int MAX_ACCOUNT_EMAIL_LEGNTH = 200;
        public const int MIN_ACCOUNT_EMAIL_LEGNTH = 5;

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.UseXminAsConcurrencyToken();

            //builder.Property(x => x.AccountType).IsRequired();
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasMaxLength(MAX_ACCOUNT_EMAIL_LEGNTH)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasMaxLength(MAX_ACCOUNT_PASSWORD_LEGNTH)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasConversion(
                    dateTime => dateTime.ToInstant(),
                    instant => instant.ToDateTimeUtc())
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasData(DataInitializer.Accounts);
        }
    }
}
