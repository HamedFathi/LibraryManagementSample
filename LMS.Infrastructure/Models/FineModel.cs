namespace LMS.Infrastructure.Models;

public class FineModel
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public MemberModel Member { get; set; }
    public Guid LoanId { get; set; }
    public LoanModel Loan { get; set; }
    public decimal Amount { get; set; }
    public int Currency { get; set; }
    public DateTime DateIssued { get; set; }
    public int Status { get; set; }
}