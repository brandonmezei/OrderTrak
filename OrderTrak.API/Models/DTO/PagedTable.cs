namespace OrderTrak.API.Models.DTO
{
    public class PagedTable<T>
    {
        public List<T> Data { get; set; } = [];

        public int TotalRecords { get; set; } = 0;

        public int PageIndex { get; set; } = 1;
    }
}
