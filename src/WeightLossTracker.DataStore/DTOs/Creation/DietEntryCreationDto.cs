using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.DTOs.Creation
{
    public class DietEntryCreationDto
    {
        public MealCategoriesCreationDto MealCategories { get; set; }
        public FoodCreationDto Food { get; set; }
        public int PortionSize { get; set; }
    }
}
