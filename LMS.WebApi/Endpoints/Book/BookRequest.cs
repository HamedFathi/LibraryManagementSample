namespace LMS.WebApi.Endpoints.Book
{
    public class BookRequest
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; } = new();
        public string Isbn { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }
        public string Edition { get; set; }
        public int Category { get; set; }
        public int MaxCopies { get; set; }
    }
}
