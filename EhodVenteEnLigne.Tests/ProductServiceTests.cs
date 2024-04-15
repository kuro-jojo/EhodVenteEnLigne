using EhodBoutiqueEnLigne.Data;
using EhodBoutiqueEnLigne.Models;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class ProductServiceTests
    {
        ProductService _productService;
        Mock<EhodBDD> _context = new Mock<EhodBDD>();

        public ProductServiceTests()
        {
        }

        [Fact]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            
        }
    }

}