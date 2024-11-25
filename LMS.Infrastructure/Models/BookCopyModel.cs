namespace LMS.Infrastructure.Models;

public class BookCopyModel
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public BookModel Book { get; set; }
    public string Barcode { get; set; }
    public int Condition { get; set; }
    public int Status { get; set; }
}