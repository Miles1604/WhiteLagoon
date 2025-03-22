using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Web.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //automatically hides whatever is being typed in this field
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))] //validation control - compares to make sure is correct with password - if not will throw error
        [Display(Name= "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Display (Name= "Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? RedirectUrl { get; set; }
        public string? Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}
