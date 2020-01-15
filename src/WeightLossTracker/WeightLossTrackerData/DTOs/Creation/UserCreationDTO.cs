using System;
using System.ComponentModel.DataAnnotations;

namespace WeightLossTrackerData.DTOs.Creation
{
    public class UserCreationDTO
    {
        [Required(ErrorMessage = "The first name field is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name field is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The email field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The height field is required")]
        public float Height { get; set; }
        [Required(ErrorMessage = "The current weight field is required")]
        public float CurrentWeight { get; set; }
        [Required(ErrorMessage = "The date of birth field is required")]
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
