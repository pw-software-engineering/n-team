using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess;
using Server.Models;
using Server.RequestModels;
using Server.Services.Result;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.HotelSearchService
{
    public class HotelSearchService : IHotelSearchService
    {
        private readonly IMapper _mapper;
        private readonly IHotelSearchDataAccess _hotelSearchDataAccess;
        public HotelSearchService(IHotelSearchDataAccess hotelSearchDataAccess, IMapper mapper)
        {
            _mapper = mapper;
            _hotelSearchDataAccess = hotelSearchDataAccess;
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
                    new Error("Invalid paging arguments"));
            }
            List<HotelPreviewView> hotelPreviews = _mapper.Map<List<HotelPreviewView>>(_hotelSearchDataAccess.GetHotels(paging, hotelFilter));
            return new ServiceResult(HttpStatusCode.OK, hotelPreviews);
        }
        public IServiceResult GetHotelDetails(int hotelID)
        {
            HotelView hotelView = _mapper.Map<HotelView>(_hotelSearchDataAccess.GetHotelDetails(hotelID));
            if(hotelView == null)
            {
                return new ServiceResult(HttpStatusCode.NotFound);
            }
            hotelView.HotelPictures = _hotelSearchDataAccess.GetHotelPictures(hotelID);
            return new ServiceResult(HttpStatusCode.OK, hotelView);
        }
    }
}
