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
            #endregion

            #region Model -> ViewModel -> Model
            CreateMap<OfferUpdateInfo, OfferView>();
            CreateMap<OfferView, OfferUpdateInfo>();
            #endregion
        }
    }
}
