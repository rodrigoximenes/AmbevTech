using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
            builder.Property(s => s.SaleDate).IsRequired().HasColumnType("timestamp");

            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey(item => item.SaleId)  
                   .OnDelete(DeleteBehavior.Cascade);  

            builder.Property(s => s.TotalAmount)
                   .HasColumnType("decimal(18,2)") 
                   .HasDefaultValue(0m);  

            builder.Property(s => s.Discount)
                   .HasColumnType("decimal(5,2)")  
                   .HasDefaultValue(0m);  
        }
    }
}
