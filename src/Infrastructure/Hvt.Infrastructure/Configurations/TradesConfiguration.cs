using Hvt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Hvt.Infrastructure.Configurations
{
    public class TradesConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Trade> builder)
        {
            builder.ToTable("Trades");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.SymbolId).IsRequired();
            builder.HasOne(x => x.Symbol)
                .WithMany()
                .HasForeignKey(x => x.SymbolId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.TradeType).HasConversion<int>().IsRequired();
            builder.Property(x => x.OpenPrice).HasColumnType("decimal(18, 8)");
            builder.Property(x => x.ClosePrice).HasDefaultValue(null).HasColumnType("decimal(18, 8)");
            builder.Property(x => x.ProfitAndLoss).HasDefaultValue(null).HasColumnType("decimal(18, 8)");
            builder.Property(x => x.InvestedAmount).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(x => x.Volume).IsRequired().HasColumnType("decimal(18, 8)");
            builder.Property(x => x.OpenAt).IsRequired();
            builder.Property(x => x.Leverage).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.CloseAt).HasDefaultValue(null);
        }
    }
}
