using System.ComponentModel.DataAnnotations;
using System;

namespace  entity_app.Models
{
    public class FormActivityViewModel
    {
        [Display(Name = "Title")]
        [Required]
        [MinLength(2)]
        public string title { get; set; }

        [Display(Name = "Time")]
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan time { get; set; }


        [Display(Name = "Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [Display(Name = "Duration")]
        [Required]
        public int duration { get; set; }

        [Display(Name = "Duration Time")]
        [Required]
        public string duration_time { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MinLength(2)]
        public string description { get; set; }
    }
}