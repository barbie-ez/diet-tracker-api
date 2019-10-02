using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Base.Impl;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.DataStore.Repositories.Impl
{
    public class FoodRepository : EntityBaseRepository<FoodModel>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
