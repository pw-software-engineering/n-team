using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.HotelAccountService
{
    public class HotelAccountService : IHotelAccountService
    {
        private IHotelAccountDataAccess hotelAccountDataAccess;
        private readonly IDatabaseTransaction _transaction;

        public HotelAccountService(IHotelAccountDataAccess hotelAccountDataAccess, IDatabaseTransaction databaseTransaction)
        {
            _transaction = databaseTransaction;
            this.hotelAccountDataAccess = hotelAccountDataAccess;
        }

        public HotelGetInfo GetInfo(int hotelId)
        {
            try
            {
                return hotelAccountDataAccess.GetInfo(hotelId);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            _transaction.BeginTransaction();
            try {
                hotelAccountDataAccess.UpdateInfo(hotelId, hotelUpdateInfo);
            }catch(NotFundExepcion e)
            {
                _transaction.RollbackTransaction();
                throw new Exception(e.Message);
            }
            _transaction.CommitTransaction();
            
        }   
    }
}
