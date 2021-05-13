using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public class HotelTokenDataAccess : IHotelTokenDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public HotelTokenDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public int? GetHotelIdFromToken(string hotelToken)
        {
            return _dbContext.Hotels.FirstOrDefault(x => x.AccessToken == hotelToken)?.HotelID;
        }
    }
}
