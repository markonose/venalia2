namespace WebApi.Shared
{
    public class Pagination
    {
        public int? Limit { get; set; }
        public long NumberOfPages { get; set; }
        public long? Offset { get; set; }
    }
}
