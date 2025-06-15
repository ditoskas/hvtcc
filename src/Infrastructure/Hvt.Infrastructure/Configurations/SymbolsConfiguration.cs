using Hvt.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hvt.Infrastructure.Configurations
{
    public class SymbolsConfiguration : IEntityTypeConfiguration<Symbol>
    {
        public void Configure(EntityTypeBuilder<Symbol> builder)
        {
            builder.ToTable("Symbols");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.BinanceSymbol).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PolygonTicker).IsRequired().HasMaxLength(100);
            builder.Property(x => x.EquityPercentageToBet).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(x => x.ProfitTargetPercentage).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(x => x.StopLossTargetPercentage).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();
            builder.Property(x => x.LeverageToTrade).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
        }
    }
}
