using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeightLossTracker.DataStore.Entitties
{
    public class DietTrackerModel : BaseModel
    {
        public MealCategoriesModel MealCategories { get; set; }
        [Required(ErrorMessage ="The meal category Id is required")]
        public int MealCategoriesId { get; set; }
        public FoodModel Food { get; set; }
        [Required(ErrorMessage = "The food Id is required")]
        public int FoodModelId { get; set; }
        [Required(ErrorMessage = "The portion size is required")]
        public int PortionSize { get; set; }
    }
}
