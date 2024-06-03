using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class RegisterViewModel
    {
        //Display is a method of System.ComponentModel.DataAnnotations
        //It just gives you how to display in view. For eg- Email Address instead of property EmailAddress
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
