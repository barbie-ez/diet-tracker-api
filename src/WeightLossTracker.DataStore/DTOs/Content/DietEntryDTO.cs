using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.DTOs.Content
{
    public class DietEntryDto
    {
        public MealCategoriesDto MealCategories { get; set; }
        public FoodDto Food { get; set; }
        public int PortionSize { get; set; }
    }
}
