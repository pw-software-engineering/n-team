using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.Authentication
{
    public class HotelInfo
    {
        [JsonProperty(Required = Required.Always)]
        public string hotelName { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string hotelDesc { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string country { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string city { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string hotelPreviewPicture { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string[] hotelPictures { get; set; }
    }
}
