using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Services.Client
{
    public class HotelSearchService : IHotelSearchService
    {
        private readonly IHotelSearchDataAccess _hotelSearchDataAccess;
        public HotelSearchService(IHotelSearchDataAccess hotelSearchDataAccess)
        {
            _hotelSearchDataAccess = hotelSearchDataAccess;
        }
        public IServiceResult GetHotels(HotelFilter hotelFilter, Paging paging)
        {
            if (hotelFilter is null)
                throw new ArgumentNullException("hotelFilter");
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));

            List<HotelPreviewView> hotelPreviews = _hotelSearchDataAccess.GetHotels(hotelFilter, paging);
            return new ServiceResult(HttpStatusCode.OK, hotelPreviews);
        }
        public IServiceResult GetHotelDetails(int hotelID)
        {
            HotelView hotelView = _hotelSearchDataAccess.GetHotelDetails(hotelID);
            if(hotelView is null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist"));
            hotelView.HotelPictures = _hotelSearchDataAccess.GetHotelPictures(hotelID);

            return new ServiceResult(HttpStatusCode.OK, hotelView);
        }

        public IServiceResult GetHotelReviews(int hotelID, Paging paging)
        {
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));

            if(!_hotelSearchDataAccess.DoesHotelExist(hotelID))
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist"));

            return new ServiceResult(HttpStatusCode.OK, _hotelSearchDataAccess.GetHotelReviews(hotelID, paging));
        }
    }
}