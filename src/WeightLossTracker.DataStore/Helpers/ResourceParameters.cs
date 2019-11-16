using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightLossTracker.DataStore.Helpers
{
    public class ResourceParameters
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSIze = 10;
        public int PageSize 
        { 
            get 
            {
                return _pageSIze;
            } 
            
            set 
            {
                _pageSIze = value > maxPageSize ? maxPageSize : value;
            } 
        }
    }
}
