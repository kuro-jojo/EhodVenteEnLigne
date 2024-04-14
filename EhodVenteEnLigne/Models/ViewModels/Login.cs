using System.ComponentModel.DataAnnotations;

namespace EhodBoutiqueEnLigne.Models.ViewModels
{
    public class Login
    {
        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}