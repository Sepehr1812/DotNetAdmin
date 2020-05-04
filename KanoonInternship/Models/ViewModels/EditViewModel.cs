using System;
using System.ComponentModel.DataAnnotations;

namespace KanoonInternship.Models.ViewModels
{
    public class EditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Ban Until")]
        public DateTime BanUntil { get; set; }
    }
}
