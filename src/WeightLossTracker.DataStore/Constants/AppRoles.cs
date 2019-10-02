using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLossTracker.DataStore.Constants
{
    public class AppRoles
    {
        public const string Administrator = "Administrator";
        public const string Member = "Member";

        public static IEnumerable<AppRoles> roles { get; set; }

    }
}
