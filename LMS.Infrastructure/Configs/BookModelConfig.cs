namespace LMS.Infrastructure.Configs;

using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BookModelConfig : IEntityTypeConfiguration<BookModel>
{
    public void Configure(EntityTypeBuilder<BookModel> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(b => b.Isbn)
            .IsRequired()
            .HasMaxLength(13);

        builder.Property(b => b.Publisher)
            .HasMaxLength(150);

        builder.Property(b => b.Edition)
            .HasMaxLength(50);

        builder.Property(b => b.PublicationYear)
            .IsRequired();

        builder.Property(b => b.Category)
            .IsRequired();

        builder.Property(b => b.MaxCopies)
            .IsRequired();

        builder.HasMany(b => b.Copies)
            .WithOne(c => c.Book)
            .HasForeignKey(c => c.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.BookAuthors)
            .WithOne(ba => ba.Book)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}