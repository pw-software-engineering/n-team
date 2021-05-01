using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels
{
    public class Paging
    {
        [FromQuery]
        public int pageSize { get; set; }

        [FromQuery]
        public int pageNumber { get; set; }
        public Paging() : this(10, 1) { }
        public Paging(int size, int number)
        {
            pageSize = size;
            pageNumber = number;
        }
    }
}
