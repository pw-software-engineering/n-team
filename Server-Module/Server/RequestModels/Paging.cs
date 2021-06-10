using Microsoft.AspNetCore.Mvc;

namespace Server.RequestModels
{
    public class Paging
    {
        [FromQuery]
        public int PageSize { get; set; }

        [FromQuery]
        public int PageNumber { get; set; }
        public Paging() : this(10, 1) { }
        public Paging(int size, int number)
        {
            PageSize = size;
            PageNumber = number;
        }
    }
}
