using AppDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppInfrastructure.Database.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public static int MAX_USER_FIRSTNAME_LENGTH = 100;
        public static int MIN_USER_FIRSTNAME_LENGTH = 2;
        public static int MAX_USER_LASTNAME_LENGTH = 100;
        public static int MIN_USER_LASTNAME_LENGTH = 2;

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.UseXminAsConcurrencyToken();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasMaxLength(MAX_USER_FIRSTNAME_LENGTH)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(MAX_USER_LASTNAME_LENGTH);
        }
    }
}
