using AutoMapper;
using Server.Database.Models;
using Server.Models;
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
            CreateMap<HotelRoomDb, HotelRoom>();
            CreateMap<HotelRoom, HotelRoomDb>();

            CreateMap<Hotel, HotelInfoDb>();
            CreateMap<HotelInfoDb, Hotel>();
            CreateMap<HotelPreview, HotelInfoDb>();
            CreateMap<HotelInfoDb, HotelPreview>().AfterMap((hdb, h) => h.PreviewPicture = hdb.HotelPreviewPicture);
            #endregion

            #region Model -> ViewModel -> Model
            CreateMap<Offer, OfferView>();
            CreateMap<OfferView, Offer>();
            CreateMap<OfferPreview, OfferPreviewView>();
            CreateMap<OfferPreviewView, OfferPreview>();
            CreateMap<HotelRoomView, HotelRoom>();
            CreateMap<HotelRoom, HotelRoomView>();

            CreateMap<Hotel, HotelView>();
            CreateMap<HotelView, Hotel>();
            CreateMap<HotelPreview, HotelPreviewView>();
            CreateMap<HotelPreviewView, HotelPreview>();
            #endregion
        }
    }
}
