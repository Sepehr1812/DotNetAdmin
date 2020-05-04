using Microsoft.AspNetCore.Identity;
using System;

namespace KanoonInternship.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ActiveState { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBanned { get; set; }

        public DateTime BanUntil { get; set; }
    }

    // Class for Json object
    public class UserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ActiveState { get; set; }

        public string IsAdmin { get; set; }

        public string BannedUntil { get; set; }
    }
}
