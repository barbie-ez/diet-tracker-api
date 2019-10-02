using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Repositories.Base.Interface;

namespace WeightLossTracker.DataStore.Entitties
{
    public class BaseModel : IEntityBase
    {
        public BaseModel()
        {
            this.DateCreated = DateUpdated = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
