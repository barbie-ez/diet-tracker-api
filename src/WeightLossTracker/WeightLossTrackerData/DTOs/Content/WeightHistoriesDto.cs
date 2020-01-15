using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTrackerData.DTOs.Content
{
    public class WeightHistoriesDto
    {
        public float Weight { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public int Id { get; set; }
    }
}
