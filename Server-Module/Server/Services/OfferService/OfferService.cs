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
        
        public int AddOffer(OfferView offerView, int hotelID)
        {
            Offer offer = _mapper.Map<Offer>(offerView);
            offer.HotelID = hotelID;
            offer.IsDeleted = false;
            int offerID = _dataAccess.AddOffer(offer);
            _dataAccess.AddOfferPictures(offer.Pictures, offerID);
            return offerID;
        }

        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void DeleteOffer(int offerID, int hotelID)
        {
            CheckExceptions(_dataAccess.FindOfferAndGetOwner(offerID), hotelID);
            _dataAccess.DeleteOffer(offerID);
        }

        public List<OfferPreviewView> GetHotelOffers(int hotelID)
        {
            return _mapper.Map<List<OfferPreviewView>>(_dataAccess.GetHotelOffers(hotelID));
        }

        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public OfferView GetOffer(int offerID, int hotelID)
        {
            CheckExceptions(_dataAccess.FindOfferAndGetOwner(offerID), hotelID);
            return _mapper.Map<OfferView>(_dataAccess.GetOffer(offerID));
        }

        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void UpdateOffer(int offerID, int hotelID, OfferUpdateInfo offerUpdateInfo)
        {
            CheckExceptions(_dataAccess.FindOfferAndGetOwner(offerID), hotelID);
            _dataAccess.UpdateOffer(offerID, offerUpdateInfo);
        }
        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void CheckExceptions(int? ownerID, int hotelID)
        {
            if (ownerID == null)
                throw new NotFoundException();
            if (ownerID != hotelID)
                throw new NotOwnerException();
        }
    }
}
