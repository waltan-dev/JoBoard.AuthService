using JoBoard.AuthService.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoBoard.AuthService.Infrastructure.EntityConfigs;

public class ExternalAccountConfig : IEntityTypeConfiguration<ExternalNetworkAccount>
{
    public void Configure(EntityTypeBuilder<ExternalNetworkAccount> builder)
    {
        builder.ToTable("ExternalNetworkAccounts");

        builder.HasKey(x => new { x.Id, x.ExternalUserId, x.Network });
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value, 
            value => new UserId(value));
    }
}