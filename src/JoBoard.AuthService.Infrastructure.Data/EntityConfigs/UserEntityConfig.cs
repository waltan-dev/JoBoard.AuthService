using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoBoard.AuthService.Infrastructure.Data.EntityConfigs;

public class UserEntityConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Ignore(x => x.DomainEvents);
        
        // map Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value, 
            value => new UserId(value));

        // map Email
        builder.OwnsOne(x => x.Email, navBuilder =>
        {
            navBuilder.HasIndex(x => x.Value)
                .IsUnique()
                .HasDatabaseName("IX_Users_" + nameof(User.Email));
            navBuilder.Property(y => y.Value)
                .HasColumnName(nameof(User.Email));
        });
        
        // map FullName
        builder.OwnsOne(x => x.FullName, navBuilder =>
        {
            navBuilder.Property(y => y.FirstName)
                .HasColumnName(nameof(FullName.FirstName));
            navBuilder.Property(y => y.LastName)
                .HasColumnName(nameof(FullName.LastName));
        });

        // map Role
        builder.Property(x => x.Role).HasConversion(role => role.Id,
            id => Enumeration.FromValue<UserRole>(id));
        
        // map Status
        builder.Property(x => x.Status).HasConversion(status => status.Id,
            id => Enumeration.FromValue<UserStatus>(id));
        
        // map RegisterConfirmToken
        builder.OwnsOne(x => x.RegisterConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value)
                .HasColumnName(nameof(User.RegisterConfirmToken));
            navBuilder.Property(y => y.Expiration)
                .HasColumnName(nameof(User.RegisterConfirmToken)+"Expiration");
        });
        
        // map ResetPasswordConfirmToken
        builder.OwnsOne(x => x.ResetPasswordConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value)
                .HasColumnName(nameof(User.ResetPasswordConfirmToken));
            navBuilder.Property(y => y.Expiration)
                .HasColumnName(nameof(User.ResetPasswordConfirmToken)+"Expiration");
        });
        
        // map ChangeEmailConfirmToken
        builder.OwnsOne(x => x.ChangeEmailConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value)
                .HasColumnName(nameof(User.ChangeEmailConfirmToken));
            navBuilder.Property(y => y.Expiration)
                .HasColumnName(nameof(User.ChangeEmailConfirmToken)+"Expiration");
        });
        
        // map AccountDeactivationConfirmToken
        builder.OwnsOne(x => x.AccountDeactivationConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value)
                .HasColumnName(nameof(User.AccountDeactivationConfirmToken));
            navBuilder.Property(y => y.Expiration)
                .HasColumnName(nameof(User.AccountDeactivationConfirmToken)+"Expiration");
        });
        
        // map NewEmail
        builder.OwnsOne(x => x.NewEmail, navBuilder =>
        {
            navBuilder.Property(y => y.Value)
                .HasColumnName(nameof(User.NewEmail));
        });

        // map ExternalAccounts
        builder.HasMany(x => x.ExternalAccounts)
            .WithOne()
            .HasForeignKey(x => x.Id);
    }
}