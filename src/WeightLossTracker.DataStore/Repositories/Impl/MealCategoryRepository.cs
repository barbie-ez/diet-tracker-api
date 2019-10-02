using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Base.Impl;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.DataStore.Repositories.Impl
{
    public class MealCategoryRepository : EntityBaseRepository<MealCategoriesModel>, IMealCategoryRepository
    {
        public MealCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
