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
            CreateMap<OfferDb, OfferPreviewView>();
            CreateMap<OfferDb, OfferView>();
            CreateMap<ClientDb, ClientInfoView>();
            #endregion

            #region Request -> Model -> Db
            CreateMap<ReservationInfo, Reservation>();
            CreateMap<Reservation, ClientReservationDb>();
            #endregion
        }
    }
}
