using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.Entitties
{
    public class DietTrackerModel : BaseModel
    {
        public MealCategoriesModel MealCategories { get; set; }
        public int MealCategoriesId { get; set; }
        public FoodModel Food { get; set; }
        public int FoodModelId { get; set; }
    }
}
