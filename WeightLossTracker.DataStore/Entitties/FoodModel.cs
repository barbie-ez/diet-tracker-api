using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.Entitties
{
    public class FoodModel : BaseModel
    {
        public float DietaryFiber { get; set; }
        public float Carbohydrates { get; set; }
        public float Fats { get; set; }
        public float Protein { get; set; }
        public float Sugars { get; set; }
        public int Calories { get; set; }
        public string ServingSize { get; set; }
        public string FoodName { get; set; }

    }
}
