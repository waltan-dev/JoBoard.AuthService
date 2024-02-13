// using JoBoard.AuthService.Domain.Aggregates.User;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace JoBoard.AuthService.Infrastructure.Data.EntityConfigs;
//
// public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
// {
//     public void Configure(EntityTypeBuilder<RefreshToken> builder)
//     {
//         builder.ToTable("RefreshTokens");
//         
//         // map Id
//         builder.HasKey(x => x.Id);
//         
//         // By convention, non-composite primary keys of type short, int, long, or Guid will be setup to have values generated on add
//         // Starting with EF Core 3.0, if an entity is using generated key values and some key value is set,
//         // then the entity will be tracked in the Modified state.
//         // https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations#conventions
//         builder.Property(x => x.Id).ValueGeneratedNever();
//         
//         builder.Property(x => x.UserId).HasConversion(
//             userId => userId.Value, 
//             value => new UserId(value));
//     }
// }