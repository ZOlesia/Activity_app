using System;
using System.Collections.Generic;


namespace entity_app.Models
{
    public class Participant
    {
        public int participantid { get; set; }
        public int userid { get; set; }
        public User user { get; set; }
        public int activityid { get; set; }
        public Center activity { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }


        public Participant()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
        }
    }
}







                //                 
                // 
                // <td>@activity.coordinator.first_name</td>
                // 