using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeightLossTracker.DataStore.Entitties
{
    public class WeightHistories : BaseModel
    {
        [Required(ErrorMessage ="The weight field is required")]
        public float Weight { get; set; }
        public string Comment { get; set; }
    }
}
