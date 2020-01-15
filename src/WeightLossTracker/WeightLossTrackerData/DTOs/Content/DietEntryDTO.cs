using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTrackerData.DTOs.Content
{
    public class DietEntryDto
    {
        public MealCategoriesDto MealCategories { get; set; }
        public FoodDto Food { get; set; }
        public int PortionSize { get; set; }
        public int Id { get; set; }
    }
}
