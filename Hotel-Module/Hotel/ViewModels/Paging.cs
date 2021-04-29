﻿namespace Hotel.ViewModels
{
    public class Paging
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Paging()
        {
            PageNumber = 1;
            PageSize = 10;
        }
    }
}
