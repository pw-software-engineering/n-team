namespace Hotel.ViewModels
{
    public class Paging
    {
        public int PageNumber;
        public int PageSize;
        public Paging(int pageNumber = 1, int pageSize = 10)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
