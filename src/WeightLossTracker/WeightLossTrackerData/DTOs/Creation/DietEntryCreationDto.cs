using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTrackerData.DTOs.Creation
{
    public class DietEntryCreationDto
    {
        public int MealCategoriesId { get; set; }
        public int FoodId { get; set; }
        public int PortionSize { get; set; }
    }
}
