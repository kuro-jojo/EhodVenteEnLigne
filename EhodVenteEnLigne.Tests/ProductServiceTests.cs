using EhodBoutiqueEnLigne.Models;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class ProductServiceTests
    {
        private readonly Product[] _products;
        private readonly Cart _cart;
        ProductService _productService;
        Mock<IProductRepository> _context = new Mock<IProductRepository>();

        public ProductServiceTests()
        {
            _products = new Product[]
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            _cart = new Cart();
            _cart.AddItem(_products[0], 20);
            _cart.AddItem(_products[1], 10);
        }

        [Fact]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            _context.Setup(x => x.GetAllProducts()).Returns(_products);

            _productService = new ProductService(null, _context.Object);

            // Act
            List<ProductViewModel> products = _productService.GetAllProductsViewModel();

            // Assert
            Assert.Equal(2, products.Count);
            Assert.Equal("Product 1", products[0].Name);
            Assert.Equal("Product 2", products[1].Name);
        }

        [Fact]
        public void GetAllProducts_ShouldReturnEmptyList()
        {
            // Arrange
            _context.Setup(x => x.GetAllProducts()).Returns(new List<Product>());

            _productService = new ProductService(null, _context.Object);

            // Act
            List<ProductViewModel> products = _productService.GetAllProductsViewModel();

            // Assert
            Assert.Empty(products);
        }


        [Fact]
        public void GetProductByIdViewModel_ShouldReturnProductById()
        {
            // Arrange
            _context.Setup(x => x.GetAllProducts()).Returns(_products);

            _productService = new ProductService(null, _context.Object);

            // Act
            ProductViewModel product = _productService.GetProductByIdViewModel(1);

            // Assert
            Assert.Equal("Product 1", product.Name);
        }

        [Fact]
        public void GetProductById_ShouldReturnProduct1()
        {
            // Arrange
            _context.Setup(x => x.GetAllProducts()).Returns(_products);

            _context.Setup(x => x.GetProduct(1)).ReturnsAsync(new Product { Id = 1, Name = "Product 1" });

            _productService = new ProductService(null, _context.Object);

            // Act
            Product product = _productService.GetProductById(1);

            // Assert
            Assert.Equal("Product 1", product.Name);
        }

        [Fact]
        public void UpdateProductQuantities_ShouldUpdateProductQuantities()
        {
            // Arrange
            _context.Setup(x => x.UpdateProductStocks(1, 1));

            _productService = new ProductService(_cart, _context.Object);

            // Act
            _productService.UpdateProductQuantities();

            // Assert
            _context.Verify(x => x.UpdateProductStocks(1, 20), Times.Once);
        }


        [Fact]
        public void SaveProduct_ShouldSaveProduct()
        {
            // Arrange
            ProductViewModel product = new ProductViewModel
            {
                Name = "Product 15",
                Price = "10",
                Stock = "10",
                Description = "Description",
                Details = "Details"
            };

            _context.Setup(x => x.SaveProduct(It.IsAny<Product>()));
            _productService = new ProductService(_cart, _context.Object);

            // Act
            _productService.SaveProduct(product);

            // Assert
            _context.Verify(x => x.SaveProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void SaveProduct_ShouldThrowException()
        {
            // Arrange
            ProductViewModel product = new ProductViewModel
            {
                Name = "Product 15",
                Price = "10",
                Stock = "10"
            };

            _context.Setup(x => x.SaveProduct(It.IsAny<Product>()));

            _productService = new ProductService(_cart, _context.Object);

            // Act & Assert
            Assert.Throws<Exception>(() => _productService.SaveProduct(product));
        }

        [Fact]
        public void DeleteProduct_ShouldDeleteProduct()
        {
            // Arrange
            _context.Setup(x => x.GetAllProducts()).Returns(_products);
            _context.Setup(x => x.DeleteProduct(1));

            _productService = new ProductService(_cart, _context.Object);

            // Act
            _productService.DeleteProduct(1);

            // Assert
            _context.Verify(x => x.DeleteProduct(1), Times.Once);
        }
    }

}