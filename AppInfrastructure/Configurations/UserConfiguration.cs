using AppDomain.Entities;
using AppInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppInfrastructure.Utilities;
using NodaTime;

namespace AppInfrastructure.Database.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public const int MAX_USER_FIRSTNAME_LENGTH = 100;
        public const int MIN_USER_FIRSTNAME_LENGTH = 2;
        public const int MAX_USER_LASTNAME_LENGTH = 100;
        public const int MIN_USER_LASTNAME_LENGTH = 2;
        public const int MAX_NEW_USER_AGE = 200;

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasMaxLength(MAX_USER_FIRSTNAME_LENGTH)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(MAX_USER_LASTNAME_LENGTH);

            builder.Property(x => x.DateOfBirth)
                .HasConversion<Instant?>(
                    dateTime => dateTime.HasValue ? dateTime.Value.ToInstant() : null,
                    instant => instant.HasValue ? instant.Value.ToDateTimeUtc() : null);

            builder.HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<User>(u => u.AccountId);

            builder.Property(x => x.AccountId).IsRequired();

            builder.HasData(DataInitializer.Users);
        }
    }
}
