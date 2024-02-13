using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoBoard.AuthService.Infrastructure.Data.EntityConfigs;

public class ExternalAccountConfig : IEntityTypeConfiguration<ExternalAccount>
{
    public void Configure(EntityTypeBuilder<ExternalAccount> builder)
    {
        builder.ToTable("ExternalAccounts");
        
        // map Id
        builder.HasKey(x => new { x.Id, x.Value });
        
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value, 
            value => new UserId(value));

        builder.OwnsOne(x => x.Value, navBuilder =>
        {
            navBuilder.Property(x => x.ExternalUserId).HasColumnName("ExternalUserId");
            navBuilder.Property(x => x.Provider).HasColumnName("Provider");

            navBuilder.HasIndex(x => new { x.ExternalUserId, x.Provider });
        });
    }
}