using System.Linq;

namespace Server.Database.DataAccess.Hotel
{
    public class HotelTokenDataAccess : IHotelTokenDataAccess
    {
        private readonly ServerDbContext _dbContext;
        public HotelTokenDataAccess(ServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int? GetHotelIdFromToken(string hotelToken)
        {
            return _dbContext.Hotels.FirstOrDefault(h => h.AccessToken == hotelToken)?.HotelID;
        }
    }
}
