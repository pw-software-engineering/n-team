using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class Paging
    {
        public int pageSize = 10;
        public int pageNumber = 1;
        public Paging() { }
        public Paging(int size, int number)
        {
            pageSize = size;
            pageNumber = number;
        }
    }
}
