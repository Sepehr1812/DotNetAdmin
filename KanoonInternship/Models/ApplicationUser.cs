using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public string IsBanned { get; set; }

        public string BanUntil { get; set; }
    }
}
