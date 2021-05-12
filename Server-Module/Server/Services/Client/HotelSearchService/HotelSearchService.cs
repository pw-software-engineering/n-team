using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
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
        public IServiceResult GetHotels(Paging paging, HotelFilter hotelFilter)
        {
            if (hotelFilter == null)
            {
                throw new ArgumentNullException("hotelFilter");
            }
            if (paging.pageNumber < 1 || paging.pageSize < 1)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));
            }

            List<HotelSearchPreviewView> hotelPreviews = _mapper.Map<List<HotelSearchPreviewView>>(_hotelSearchDataAccess.GetHotels(paging, hotelFilter));
            
            return new ServiceResult(HttpStatusCode.OK, hotelPreviews);
        }
        public IServiceResult GetHotelDetails(int hotelID)
        {
            _transaction.BeginTransaction();
            HotelSearchView hotelView = _mapper.Map<HotelSearchView>(_hotelSearchDataAccess.GetHotelDetails(hotelID));
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
