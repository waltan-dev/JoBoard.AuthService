using JoBoard.AuthService.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoBoard.AuthService.Infrastructure.Data.EntityConfigs;

public class ExternalAccountConfig : IEntityTypeConfiguration<ExternalAccount>
{
    public void Configure(EntityTypeBuilder<ExternalAccount> builder)
    {
        builder.ToTable("ExternalAccounts");

        // map Id
        builder.HasKey(x => new { x.Id, x.ExternalUserId, x.Provider });
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value, 
            value => new UserId(value));
    }
}