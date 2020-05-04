using System.ComponentModel.DataAnnotations;

namespace KanoonInternship.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Remeber me.")]
        //public bool RememberMe { get; set; }
    }
}
