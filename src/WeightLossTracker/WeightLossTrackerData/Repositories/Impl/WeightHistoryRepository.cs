using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTrackerData.DataContext;
using WeightLossTrackerData.Entities;
using WeightLossTrackerData.Repositories.Base.Impl;
using WeightLossTrackerData.Repositories.Interface;

namespace WeightLossTracker.DataStore.Repositories.Impl
{
    public class WeightHistoryRepository : EntityBaseRepository<WeightHistories>, IWeightHistoryRepository
    {
        public WeightHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
