using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels.Hotel;
using Server.Services.Result;
using Server.ViewModels.Hotel;
using System;
using System.Net;

namespace Server.Services.Hotel
{
    public class HotelAccountService : IHotelAccountService
    {
        private readonly IHotelAccountDataAccess _hotelAccountDataAccess;
        private readonly IDatabaseTransaction _transaction;

        public HotelAccountService(IHotelAccountDataAccess hotelAccountDataAccess, IDatabaseTransaction transaction)
        {
            _transaction = transaction;
            _hotelAccountDataAccess = hotelAccountDataAccess; 
        }

        public IServiceResult GetHotelInfo(int hotelId)
        {
            HotelInfoView hotelInfo = _hotelAccountDataAccess.GetHotelInfo(hotelId);
            hotelInfo.HotelPictures = _hotelAccountDataAccess.GetPictures(hotelId);
            return new ServiceResult(HttpStatusCode.OK, hotelInfo);
        }

        public IServiceResult UpdateHotelInfo(int hotelId, HotelInfoUpdate hotelInfoUpdate)
        {
            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                if (hotelInfoUpdate is null)
                    throw new ArgumentNullException("hotelInfoUpdate");

                if (hotelInfoUpdate.HotelPictures != null)
                {
                    _hotelAccountDataAccess.DeletePictures(hotelId);
                    _hotelAccountDataAccess.AddPictures(hotelInfoUpdate.HotelPictures, hotelId);
                }
                _hotelAccountDataAccess.UpdateHotelInfo(hotelId, hotelInfoUpdate);
                _transaction.CommitTransaction();
                return new ServiceResult(HttpStatusCode.OK);
            }
        }   
    }
}
