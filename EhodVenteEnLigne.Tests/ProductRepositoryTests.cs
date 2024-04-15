using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EhodBoutiqueEnLigne.Data;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodVenteEnLigne.Tests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class ProductRepositoryTests
    {
        private Product[] _products;
        public ProductRepositoryTests()
        {
            _products = new Product[]
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
        }

        [Fact]
        public void GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var mockProducts = _products.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.Setup(m => m.AsQueryable()).Returns(mockProducts); // Use Setup and pass mockProducts list

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(mockProducts.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(mockProducts.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(mockProducts.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(mockProducts.GetEnumerator());


            var mockContext = new Mock<EhodBDD>();
            mockContext.Setup(c => c.Product).Returns(mockSet.Object);

            var repository = new ProductRepository(mockContext.Object);
            // Act
            var products = repository.GetAllProducts();

            // Assert
            Assert.Equal(2, products.Count());
            Assert.Equal("Product 1", products.First().Name);
            Assert.Equal("Product 2", products.Last().Name);
        }

        [Fact]
        public async Task GetProduct_ReturnsProductById_IdExists()
        {
            // Arrange
            var mockProducts = CreateDbSetMock(_products.AsQueryable());
            mockProducts.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns((object[] r) =>
            {
                return new ValueTask<Product>(mockProducts.Object.FirstOrDefaultAsync(b => b.Id == (int)r[0]));
            });

            var mockContext = new Mock<EhodBDD>();
            mockContext.Setup(m => m.Product).Returns(mockProducts.Object);

            var repository = new ProductRepository(mockContext.Object);

            var result = await repository.GetProduct(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsProductById_IdDoesNotExist()
        {
            // Arrange
            var mockProducts = CreateDbSetMock(_products.AsQueryable());
            mockProducts.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns((object[] r) =>
            {
                return new ValueTask<Product>(mockProducts.Object.FirstOrDefaultAsync(b => b.Id == (int)r[0]));
            });

            var mockContext = new Mock<EhodBDD>();
            mockContext.Setup(m => m.Product).Returns(mockProducts.Object);

            var repository = new ProductRepository(mockContext.Object);

            var result = await repository.GetProduct(3);
            Assert.Null(result);

        }

        private static Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> items) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IAsyncEnumerable<T>>()
                .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new TestAsyncEnumerator<T>(items.GetEnumerator()));
            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(items.Provider));
            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Expression).Returns(items.Expression);
            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.ElementType).Returns(items.ElementType);
            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

            return dbSetMock;
        }
    }

}