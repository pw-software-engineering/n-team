using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.AutoMapper
{
    public class ClientAutoMapperProfile : Profile
    {
        public ClientAutoMapperProfile()
        {
            #region Request -> Db
            #endregion

            #region Db -> View
            CreateMap<HotelDb, HotelPreviewView>();
            CreateMap<HotelDb, HotelView>();
            CreateMap<HotelDb, HotelInfoPreview>();
            CreateMap<OfferDb, OfferPreviewView>();
            CreateMap<OfferDb, OfferView>();
            CreateMap<OfferDb, ReservationOfferInfoPreview>();
            CreateMap<ClientDb, ClientInfoView>();
            CreateMap<ClientReservationDb, ReservationInfoView>().AfterMap((cr, r) =>
            {
                r.From = cr.FromTime;
                r.To = cr.ToTime;
            });
            //CreateMap<ClientReviewDb, ReviewInfoView>();
            #endregion

            #region Request -> Model -> Db
            CreateMap<ReservationInfo, Reservation>();
            CreateMap<Reservation, ClientReservationDb>().AfterMap((r, cr) =>
            {
                cr.FromTime = r.From;
                cr.ToTime = r.To;
            });
            #endregion
        }
    }
}
