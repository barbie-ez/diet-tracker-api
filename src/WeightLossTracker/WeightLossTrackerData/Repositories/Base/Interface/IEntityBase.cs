using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTrackerData.Repositories.Base.Interface
{
    public interface IEntityBase
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}
