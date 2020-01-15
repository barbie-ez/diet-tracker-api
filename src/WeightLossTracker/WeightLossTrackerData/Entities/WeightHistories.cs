using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeightLossTrackerData.Entities
{
    public class WeightHistories : BaseModel
    {
        [Required(ErrorMessage ="The weight field is required")]
        public float Weight { get; set; }
        public string Comment { get; set; }
        public UserProfileModel User { get; set; }
        public string UserId { get; set; }
    }
}
