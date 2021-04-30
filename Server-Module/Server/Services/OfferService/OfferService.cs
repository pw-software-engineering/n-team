using Server.Database;
using Server.Exceptions;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels;
using Server.Database.DataAccess;
using AutoMapper;
using Server.Services.Response;
using System.Net;
using Server.Services.Result;
using Server.RequestModels;

namespace Server.Services.OfferService
{   
    public class OfferService : IOfferService
    {
        private readonly IOfferDataAccess _dataAccess;
        private readonly IMapper _mapper; 
        public OfferService(IOfferDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }
        
        public IServiceResult AddOffer(OfferView offerView, int hotelID)
        {
            Offer offer = _mapper.Map<Offer>(offerView);
            offer.HotelID = hotelID;
            offer.IsDeleted = false;
            int offerID = _dataAccess.AddOffer(offer);
            _dataAccess.AddOfferPictures(offer.Pictures, offerID);

            return new ServiceResult(HttpStatusCode.OK, new OfferID(offerID));
        }

        public IServiceResult DeleteOffer(int offerID, int hotelID)
        {
            IServiceResult response = CheckExistanceAndOwnership(offerID, hotelID);
            if (response.StatusCode != HttpStatusCode.OK)
                return response;

            if (_dataAccess.AreThereUnfinishedReservationsForOffer(offerID))
                return new ServiceResult(HttpStatusCode.Conflict, new Error("There are still pending reservations for this offer"));

            _dataAccess.UnpinRoomsFromOffer(offerID);
            _dataAccess.DeleteOffer(offerID);

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult GetHotelOffers(Paging paging, int hotelID, bool? isActive = null)
        {
            if (paging.pageNumber < 1 || paging.pageSize < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Invalid paging arguments"));

            List<OfferPreviewView> offersPreviews = _mapper.Map<List<OfferPreviewView>>(_dataAccess.GetHotelOffers(paging, hotelID, isActive));
            return new ServiceResult(HttpStatusCode.OK, offersPreviews);
        }

        public IServiceResult GetOffer(int offerID, int hotelID)
        {
            IServiceResult response = CheckExistanceAndOwnership(offerID, hotelID);
            if (response.StatusCode != HttpStatusCode.OK)
                return response;

            OfferView offerView = _mapper.Map<OfferView>(_dataAccess.GetOffer(offerID));
            offerView.pictures = _dataAccess.GetOfferPictures(offerID);
            offerView.rooms = _dataAccess.GetOfferRooms(offerID);
            return new ServiceResult(HttpStatusCode.OK, offerView);
        }

        public IServiceResult UpdateOffer(int offerID, int hotelID, OfferUpdateInfo offerUpdateInfo)
        {
            IServiceResult response = CheckExistanceAndOwnership(offerID, hotelID);
            if (response.StatusCode != HttpStatusCode.OK)
                return response;

            _dataAccess.UpdateOffer(offerID, offerUpdateInfo);

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult CheckExistanceAndOwnership(int offerID, int hotelID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound);
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized);
            return new ServiceResult(HttpStatusCode.OK);
        }
    }
}
