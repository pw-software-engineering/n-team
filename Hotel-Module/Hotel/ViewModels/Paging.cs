namespace Hotel.ViewModels
{
    public class Paging
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Paging() : this(1, 10)
        { }
        public Paging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
