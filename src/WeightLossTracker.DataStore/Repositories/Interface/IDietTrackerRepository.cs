using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Base.Interface;

namespace WeightLossTracker.DataStore.Repositories.Interface
{ 
    public interface IDietTrackerRepository : IEntityBaseRepository<DietTrackerModel> { }
}
