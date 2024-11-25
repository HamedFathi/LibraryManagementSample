using LMS.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configs;

public class BookCopyModelConfig : IEntityTypeConfiguration<BookCopyModel>
{
    public void Configure(EntityTypeBuilder<BookCopyModel> builder)
    {
        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.Barcode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bc => bc.Condition)
            .IsRequired();

        builder.Property(bc => bc.Status)
            .IsRequired();

        builder.HasOne(bc => bc.Book)
            .WithMany(b => b.Copies)
            .HasForeignKey(bc => bc.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}