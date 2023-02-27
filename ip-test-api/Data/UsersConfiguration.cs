using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ip_test_api.Data.Entities;
using ip_test_api.Data.Enums;

namespace ip_test_api.Data;

internal sealed class UsersConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("User");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserName)
            .IsUnique();

        // For MSSQL
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.UserStatus)
            .HasConversion(
                v => ConvertEnumToString(v),
                v => ConvertStringToEnum(v)).HasMaxLength(1);

        builder.Property(e => e.Department)
            .HasMaxLength(255);
    }

    public string ConvertEnumToString(UserStatusType value)
    {
        return value switch
        {
            UserStatusType.Active => "A",
            UserStatusType.Inactive => "I",
            UserStatusType.Terminated => "T",
            _ => throw new ArgumentException($"Unknown value {value}", nameof(value)),
        };
    }

    public UserStatusType ConvertStringToEnum(string value)
    {
        return value switch
        {
            "A" => UserStatusType.Active,
            "I" => UserStatusType.Inactive,
            "T" => UserStatusType.Terminated,
            _ => throw new ArgumentException($"Unknown value {value}", nameof(value)),
        };
    }
}
