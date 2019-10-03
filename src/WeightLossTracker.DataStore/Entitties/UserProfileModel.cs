using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WeightLossTracker.DataStore.Repositories.Base.Interface;

namespace WeightLossTracker.DataStore.Entitties
{
    public class UserProfileModel : IdentityUser 
    {
        [Required(ErrorMessage = "The first name field is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name field is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The height field is required")]
        public float Height { get; set; }
        [Required(ErrorMessage = "The current weight field is required")]
        public float CurrentWeight { get; set; }
        [Required(ErrorMessage = "The date of birth field is required")]
        public DateTimeOffset DateOfBirth { get; set; }
        public string Token { get; set; }
        public ICollection<UserRoleModel> Roles { get; set; } = new List<UserRoleModel>();
        public ICollection<WeightHistories> WeightHistories { get; set; } = new List<WeightHistories>();
        public ICollection<DietTrackerModel> DietTrackers { get; set; } = new List<DietTrackerModel>();
    }
}
