using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WeightLossTracker.DataStore.Repositories.Base.Interface;

namespace WeightLossTracker.DataStore.Entitties
{
    public class UserProfileModel : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Height { get; set; }
        public string CurrentWeight { get; set; }
        public string Token { get; set; }
        public ICollection<UserRoleModel> Roles { get; set; } = new List<UserRoleModel>();
    }
}
