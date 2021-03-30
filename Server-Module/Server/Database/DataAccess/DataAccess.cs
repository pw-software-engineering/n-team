using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class DataAccess
    {
        private readonly IMapper _mapper;
        public DataAccess(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
