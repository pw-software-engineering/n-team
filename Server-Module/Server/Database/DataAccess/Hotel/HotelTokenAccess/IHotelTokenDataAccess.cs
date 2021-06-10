
namespace Server.Database.DataAccess.Hotel
{
    public interface IHotelTokenDataAccess
    {
        int? GetHotelIdFromToken(string hotelToken);
    }
}
