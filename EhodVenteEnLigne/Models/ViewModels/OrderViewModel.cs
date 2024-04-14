using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EhodBoutiqueEnLigne.Models.ViewModels
{
    public class OrderViewModel
    {
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string City { get; set; }
        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Zip { get; set; }

        [Required(ErrorMessageResourceType = typeof(EhodVenteEnLigne.Resources.SharedResources), ErrorMessageResourceName = "ErrorMissing")]
        public string Country { get; set; }

        [BindNever]
        public DateTime Date { get; set; }
    }
}
