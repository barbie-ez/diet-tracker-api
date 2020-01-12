using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTrackerData.Entities
{
    public class UserRoleModel : IdentityRole
    {
        public UserRoleModel()
        {

        }

        public UserRoleModel(string roleName) : this()
        {
            Name = NormalizedName = roleName;
        }
        public string Description { get; set; }

    }
}
