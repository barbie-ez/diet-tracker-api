using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.DTOs.Content
{
    /// <summary>
    /// a dto containing user data used for login
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// users id, ususally the email
        /// </summary>
        public string username;
        /// <summary>
        /// users password data
        /// </summary>
        public string password;
        public string Id { get; set; }
    }
}
