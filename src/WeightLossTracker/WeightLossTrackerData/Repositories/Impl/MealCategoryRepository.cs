using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTrackerData.DataContext;
using WeightLossTrackerData.Entities;
using WeightLossTrackerData.Repositories.Base.Impl;
using WeightLossTrackerData.Repositories.Interface;

namespace WeightLossTracker.DataStore.Repositories.Impl
{
    public class MealCategoryRepository : EntityBaseRepository<MealCategoriesModel>, IMealCategoryRepository
    {
        public MealCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
