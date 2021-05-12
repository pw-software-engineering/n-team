using AutoMapper;
using Server.Database.Models;
using Server.Models;
using Server.RequestModels;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region Db -> Model -> Db
            CreateMap<OfferUpdateInfo, OfferDb>();
            CreateMap<OfferDb, OfferUpdateInfo>();
            CreateMap<OfferPreview, OfferDb>();
            CreateMap<OfferDb, OfferPreview>();
            CreateMap<Offer, OfferDb>();
            CreateMap<OfferDb, Offer>();
            CreateMap<HotelRoomDb, HotelRoomView>();
            CreateMap<HotelRoomView, HotelRoomDb>();

            CreateMap<Hotel, HotelDb>();
            CreateMap<HotelDb, Hotel>();
            CreateMap<HotelPreview, HotelDb>();
            CreateMap<HotelDb, HotelPreview>().AfterMap((hdb, h) => h.PreviewPicture = hdb.HotelPreviewPicture);

            CreateMap<ClientOfferPreview, OfferDb>();
            CreateMap<OfferDb, ClientOfferPreview>();
            CreateMap<ClientOffer, OfferDb>();
            CreateMap<OfferDb, ClientOffer>();

            CreateMap<Reservation, ClientReservationDb>();
            CreateMap<ClientReservationDb, Reservation>();
            CreateMap<HotelDb, HotelInfoView>().AfterMap((hdb, h) => h.HotelDesc = hdb.HotelDescription);
            CreateMap<HotelInfoView, HotelDb>().AfterMap((h, hdb) =>  hdb.HotelDescription = h.HotelDesc);
            #endregion

            #region Model -> ViewModel -> Model
            CreateMap<Offer, OfferView>();
            CreateMap<OfferView, Offer>();
            CreateMap<OfferPreview, OfferPreviewView>();
            CreateMap<OfferPreviewView, OfferPreview>();
            CreateMap<HotelRoomView, HotelRoomView>();
            CreateMap<HotelRoomView, HotelRoomView>();

            CreateMap<Hotel, HotelSearchView>();
            CreateMap<HotelSearchView, Hotel>();
            CreateMap<HotelPreview, HotelSearchPreviewView>();
            CreateMap<HotelSearchPreviewView, HotelPreview>();

            CreateMap<ClientOfferPreview, OfferSearchPreviewView>();
            CreateMap<OfferSearchPreviewView, ClientOfferPreview>();

            CreateMap<Reservation, ReservationInfo>();
            CreateMap<ReservationInfo, Reservation>();
            #endregion

            CreateMap<ClientDb, ClientInfoView>();
        }
    }
}
