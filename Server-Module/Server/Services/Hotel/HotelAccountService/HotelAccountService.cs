﻿using AutoMapper;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            if (hotelInfoUpdate == null)
                throw new ArgumentNullException("hotelInfoUpdate");
            _transaction.BeginTransaction();
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