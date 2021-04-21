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
            CreateMap<OfferPreviewView, OfferDb>();
            CreateMap<OfferDb, OfferPreviewView>();
            CreateMap<Offer, OfferDb>();
            CreateMap<OfferDb, Offer>();
            #endregion

            #region Model -> ViewModel -> Model
            CreateMap<Offer, OfferView>();
            CreateMap<OfferView, Offer>();
            CreateMap<OfferPreview, OfferPreviewView>();
            CreateMap<OfferPreviewView, OfferPreview>();
            #endregion
        }
    }
}
