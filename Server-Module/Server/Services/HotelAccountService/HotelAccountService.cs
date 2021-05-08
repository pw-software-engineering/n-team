using AutoMapper;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.Database.Models;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.HotelAccountService
{
    public class HotelAccountService : IHotelAccountService
    {
        private IMapper mapper;
        private IHotelAccountDataAccess hotelAccountDataAccess;
        private readonly IDatabaseTransaction _transaction;

        public HotelAccountService(IHotelAccountDataAccess hotelAccountDataAccess, IDatabaseTransaction databaseTransaction,IMapper mapper)
        {
            this.mapper = mapper;
            _transaction = databaseTransaction;
            this.hotelAccountDataAccess = hotelAccountDataAccess;
            
        }

        public IServiceResult GetInfo(int hotelId)
        {
            HotelGetInfo result;
            try
            {
                result = mapper.Map<HotelGetInfo>(hotelAccountDataAccess.GetInfo(hotelId));
                result.HotelPictures = hotelAccountDataAccess.FindPictres(hotelId);
            } catch (Exception e)
            {
                return new ServiceResult(System.Net.HttpStatusCode.NotFound, new Error(e.Message));
               
            }
            return new ServiceResult(System.Net.HttpStatusCode.OK, result);
        }

        public IServiceResult UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            _transaction.BeginTransaction();
            try {
                var h = mapper.Map<HotelInfoDb>(hotelUpdateInfo);
                h.HotelID = hotelId;
                var old = hotelAccountDataAccess.GetInfo(hotelId);
                if(hotelUpdateInfo.HotelPictures!=null)
                    hotelAccountDataAccess.DeletePicteres(old);
                hotelAccountDataAccess.AddPictures(h);
                hotelAccountDataAccess.UpdateInfo(h);
            }catch(Exception e)
            {
                _transaction.RollbackTransaction();
                return new ServiceResult(System.Net.HttpStatusCode.NotFound, new Error(e.Message));
                
            }
            _transaction.CommitTransaction();
            return new ServiceResult(System.Net.HttpStatusCode.OK, null);
        }   
    }
}
