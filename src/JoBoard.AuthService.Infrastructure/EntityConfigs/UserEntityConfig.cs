using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoBoard.AuthService.Infrastructure.EntityConfigs;

public class UserEntityConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value, 
            value => new UserId(value));

        builder.OwnsOne(x => x.Email, navBuilder =>
        {
            navBuilder.HasIndex(x => x.Value).IsUnique().HasDatabaseName("IX_Users_" + nameof(User.Email));
            navBuilder.Property(y => y.Value).HasColumnName(nameof(User.Email));
        });
        
        builder.OwnsOne(x => x.FullName, navBuilder =>
        {
            navBuilder.Property(y => y.FirstName).HasColumnName(nameof(FullName.FirstName));
            navBuilder.Property(y => y.LastName).HasColumnName(nameof(FullName.LastName));
        });

        builder.Property(x => x.Role).HasConversion(role => role.Id,
            id => Enumeration.FromValue<UserRole>(id));
        
        builder.Property(x => x.Status).HasConversion(status => status.Id,
            id => Enumeration.FromValue<UserStatus>(id));
        
        builder.OwnsOne(x => x.RegisterConfirmToken);
        builder.OwnsOne(x => x.ResetPasswordConfirmToken);
        
        builder.OwnsOne(x => x.NewEmail, navBuilder =>
        {
            navBuilder.Property(y => y.Value).HasColumnName(nameof(User.NewEmail));
        });
        builder.OwnsOne(x => x.NewEmailConfirmationToken);

        builder.HasMany(x => x.ExternalNetworkAccounts)
            .WithOne()
            .HasForeignKey(x => x.Id);
    }
}