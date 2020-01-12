using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTrackerData.DTOs.Content
{
    public class UserProfileDto
    {
        public string Name;
        public float Height;
        public float Weight;
        public int Age;
        public List<WeightHistoriesDto> WeightHistories { get; set; }
        public int Id { get; set; }
    }
}
