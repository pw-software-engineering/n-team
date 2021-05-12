using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.AutoMapper
{
    public class HotelAutoMapperProfile : Profile
    {
        public HotelAutoMapperProfile()
        {
            #region Request -> Db
            CreateMap<OfferInfo, OfferDb>();
            #endregion

            #region Db -> View
            CreateMap<HotelDb, HotelInfoView>().AfterMap((h, hi) =>
            {
                hi.HotelDesc = h.HotelDescription;
            });
            CreateMap<HotelRoomDb, HotelRoomView>();
            CreateMap<OfferDb, OfferView>();
            CreateMap<OfferDb, OfferPreviewView>();
            #endregion
        }
    }
}
