namespace LMS.Infrastructure.Models;

public class MemberModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid AddressId { get; set; }
    public AddressModel Address { get; set; }
    public Guid ContactInfoId { get; set; }
    public ContactInfoModel ContactInfo { get; set; }
    public int MembershipType { get; set; }
    public string PreferredCurrency { get; set; }
    public DateTime MembershipStart { get; set; }
    public DateTime MembershipEnd { get; set; }
    public List<LoanModel> Loans { get; set; } = new();
    public List<ReservationModel> Reservations { get; set; } = new();
    public List<FineModel> Fines { get; set; } = new();
}