using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.DTOs.Content
{
    public class UserProfileDto
    {
        public string Name;
        public float Height;
        public float Weight;
        public int Age;
        public List<WeightHistoriesDto> WeightHistories { get; set; }
    }
}
