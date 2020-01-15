using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeightLossTrackerData.Entities
{
    public class MealCategoriesModel : BaseModel
    {
        [Required(ErrorMessage = "The meal category name field is required")]
        public string Name { get; set; }
    }
}
