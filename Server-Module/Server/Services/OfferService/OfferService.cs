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
        private readonly IDataAccess _dataAccess;
        private readonly IMapper _mapper; 
        public OfferService(IDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }
        
        public int AddOffer(OfferView offerView, int hotelID)
        {
            Offer offer = _mapper.Map<Offer>(offerView);
            offer.HotelID = hotelID;

            return _dataAccess.AddOffer(offer);
        }

        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void DeleteOffer(int offerID, int hotelID)
        {
            Offer offer = _dataAccess.GetOffer(offerID);
            CheckExceptions(offer, hotelID);
            offer.IsDeleted = true;

            _dataAccess.UpdateOffer();
        }

        public List<OfferPreviewView> GetHotelOffers(int hotelID)
        {
            return _mapper.Map<List<OfferPreviewView>>(_dataAccess.GetHotelOffers(hotelID));
        }

        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public OfferView GetOffer(int offerID, int hotelID)
        {
            Offer offer = _dataAccess.GetOffer(offerID);
            CheckExceptions(offer, hotelID);
            return _mapper.Map<OfferView>(offer);
        }

        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void UpdateOffer(int offerID, int hotelID, bool? isActive, string offerTitle, string description, string offerPreviewPicture, List<string> offerPictures)
        {
            Offer offer = _dataAccess.GetOffer(offerID);
            CheckExceptions(offer, hotelID);

            offer.IsActive = isActive ?? offer.IsActive;
            offer.OfferTitle = offerTitle ?? offer.OfferTitle;
            offer.Description = description ?? offer.Description;
            offer.OfferPreviewPicture = offerPreviewPicture ?? offer.OfferPreviewPicture;

            if (offerPictures != null)
            {
                offer.OfferPictures.RemoveAll(op => op.OfferID == offerID);
                foreach (string picture in offerPictures)
                    offer.OfferPictures.Add(new OfferPictureDb(picture, offerID));
            }
        }
        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void CheckExceptions(Offer offer, int hotelID)
        {
            if (offer == null)
                throw new NotFoundException();
            if (offer.HotelID != hotelID)
                throw new NotOwnerException();
        }
    }
}
