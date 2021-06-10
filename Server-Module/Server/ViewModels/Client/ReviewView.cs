using System;

namespace Server.ViewModels.Client
{
    public class ReviewView
    {
        public int ReviewID { get; set; }
        public string ReviewerUsername{ get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
