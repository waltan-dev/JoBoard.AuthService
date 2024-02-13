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
            navBuilder.HasIndex(x => x.Value).IsUnique().HasDatabaseName("IX_Users_Email");
            navBuilder.Property(y => y.Value).HasColumnName("Email");
        });
        
        // map FullName
        builder.OwnsOne(x => x.FullName, navBuilder =>
        {
            navBuilder.Property(y => y.FirstName).HasColumnName("FirstName");
            navBuilder.Property(y => y.LastName).HasColumnName("LastName");
        });
        
        // map Password
        builder.OwnsOne(x => x.Password, navBuilder =>
        {
            navBuilder.Property(y => y.Hash).HasColumnName("PasswordHash");
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
            navBuilder.Property(y => y.Value).HasColumnName("RegisterConfirmToken");
            navBuilder.Property(y => y.Expiration).HasColumnName("RegisterConfirmTokenExpiration");
        });
        
        // map ResetPasswordConfirmToken
        builder.OwnsOne(x => x.ResetPasswordConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value).HasColumnName("ResetPasswordConfirmToken");
            navBuilder.Property(y => y.Expiration).HasColumnName("ResetPasswordConfirmTokenExpiration");
        });
        
        // map ChangeEmailConfirmToken
        builder.OwnsOne(x => x.ChangeEmailConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value).HasColumnName("ChangeEmailConfirmToken");
            navBuilder.Property(y => y.Expiration).HasColumnName("ChangeEmailConfirmTokenExpiration");
        });
        
        // map AccountDeactivationConfirmToken
        builder.OwnsOne(x => x.AccountDeactivationConfirmToken, navBuilder =>
        {
            navBuilder.Property(y => y.Value).HasColumnName("AccountDeactivationConfirmToken");
            navBuilder.Property(y => y.Expiration).HasColumnName("AccountDeactivationConfirmTokenExpiration");
        });
        
        // map NewEmail
        builder.OwnsOne(x => x.NewEmail, navBuilder =>
        {
            navBuilder.Property(y => y.Value).HasColumnName("NewEmail");
        });

        // map ExternalAccounts
        builder.HasMany(x => x.ExternalAccounts)
            .WithOne()
            .HasForeignKey(x => x.Id);
    }
}