using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTrackerData.Entities;
using WeightLossTrackerData.Repositories.Base.Interface;

namespace WeightLossTrackerData.Repositories.Interface
{ 
    public interface IFoodRepository : IEntityBaseRepository<FoodModel> { }
}
