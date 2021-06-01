using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels.Client
{
    public class ReviewUpdater
    {
        public string content { get; set; }
        public int rating { get; set; }

    }
    public class ReviewID
    {
        public int reviewID { get; set; }
    }

    public class ReviewInfo:ReviewUpdater
    {
        public int reviewID { get; set; }
        public string revewerUsername{ get; set; }
        public DateTime creationDate { get; set; }
    }
}
