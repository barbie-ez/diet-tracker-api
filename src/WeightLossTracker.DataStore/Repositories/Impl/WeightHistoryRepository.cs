using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Base.Impl;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.DataStore.Repositories.Impl
{
    public class WeightHistoryRepository : EntityBaseRepository<WeightHistories>, IWeightHistoryRepository
    {
        public WeightHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
