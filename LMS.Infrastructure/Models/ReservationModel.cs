namespace LMS.Infrastructure.Models;

public class ReservationModel
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public MemberModel Member { get; set; }
    public Guid BookId { get; set; }
    public BookModel Book { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int Status { get; set; }
}