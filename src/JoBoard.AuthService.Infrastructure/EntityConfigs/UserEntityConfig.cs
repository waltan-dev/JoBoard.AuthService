using JoBoard.AuthService.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoBoard.AuthService.Infrastructure.EntityConfigs;

public class UserEntityConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            userId => userId.Value, 
            value => new UserId(value));
        
        builder.HasIndex(x => x.Email).IsUnique();

        builder.OwnsOne(x => x.FullName, x =>
        {
            x.Property(y => y.FirstName).HasColumnName(nameof(FullName.FirstName));
            x.Property(y => y.LastName).HasColumnName(nameof(FullName.LastName));
        });
    }
}