using System;
using System.Collections.Generic;


namespace entity_app.Models
{
    public class Center
    {
        public int centerid { get; set; }
        public string title { get; set; }
        public TimeSpan time { get; set; }
        public DateTime date { get; set; }
        public int duration { get; set; }
        public string duration_time { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int coordinatorid { get; set; }
        public User coordinator { get; set; }
        public List<Participant> participants { get; set; }



        public Center()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            participants = new List<Participant>();
        }


        
    }
}