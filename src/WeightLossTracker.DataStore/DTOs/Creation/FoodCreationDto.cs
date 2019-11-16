using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeightLossTracker.DataStore.DTOs.Creation
{
    public class FoodCreationDto
    {
        public float DietaryFiber { get; set; }
        public float Carbohydrates { get; set; }
        public float Fats { get; set; }
        public float Protein { get; set; }
        public float Sugars { get; set; }
        [Required(ErrorMessage = "The calories field required")]
        public int Calories { get; set; }
        [Required(ErrorMessage = "The serving size field is required")]
        public string ServingSize { get; set; }
        [Required(ErrorMessage = "The Food name field is required")]
        public string FoodName { get; set; }
    }
}
