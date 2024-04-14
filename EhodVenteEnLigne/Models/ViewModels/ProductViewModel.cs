using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace EhodBoutiqueEnLigne.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        [Range(0.0, double.MaxValue, ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorNotANumber")]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        [Range(0.0, double.MaxValue, ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorNotANumber")]
        public string Price { get; set; }
    }
}
