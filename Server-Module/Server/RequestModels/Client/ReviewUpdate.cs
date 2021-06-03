using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels.Client
{
    public class ReviewUpdate
    {
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
