
namespace LMS.Infrastructure.Models;

public class LoanModel
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public MemberModel Member { get; set; }
    public Guid BookCopyId { get; set; }
    public BookCopyModel BookCopy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int Status { get; set; }
}