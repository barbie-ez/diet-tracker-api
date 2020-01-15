using System;
using WeightLossTrackerData.Repositories.Base.Interface;

namespace WeightLossTrackerData.Entities
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
