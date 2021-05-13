using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Client
{
    public class HotelSearchService : IHotelSearchService
    {
        private readonly IMapper _mapper;
        private readonly IHotelSearchDataAccess _hotelSearchDataAccess;
        private readonly IDatabaseTransaction _transaction;
        public HotelSearchService(IHotelSearchDataAccess hotelSearchDataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _mapper = mapper;
            _hotelSearchDataAccess = hotelSearchDataAccess;
            _transaction = transaction;
        }
        public IServiceResult GetHotels(HotelFilter hotelFilter, Paging paging)
        {
            if (hotelFilter == null)
            {
                throw new ArgumentNullException("hotelFilter");
            }
            if (paging.PageNumber < 1 || paging.PageSize < 1)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));
            }

            List<HotelPreviewView> hotelPreviews = _mapper.Map<List<HotelPreviewView>>(_hotelSearchDataAccess.GetHotels(hotelFilter, paging));
            
            return new ServiceResult(HttpStatusCode.OK, hotelPreviews);
        }
        public IServiceResult GetHotelDetails(int hotelID)
        {
            _transaction.BeginTransaction();
            HotelView hotelView = _mapper.Map<HotelView>(_hotelSearchDataAccess.GetHotelDetails(hotelID));
            if(hotelView == null)
            {
                return new ServiceResult(HttpStatusCode.NotFound);
            }
            hotelView.HotelPictures = _hotelSearchDataAccess.GetHotelPictures(hotelID);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, hotelView);
        }
    }
}
