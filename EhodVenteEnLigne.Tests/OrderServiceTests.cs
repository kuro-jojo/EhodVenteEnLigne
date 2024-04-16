using EhodBoutiqueEnLigne.Models;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using Moq;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly OrderService _orderService;
        private readonly OrderViewModel[] _ordersViewModel;

        public OrderServiceTests()
        {
            var cartMock = new Mock<ICart>();
            var productServiceMock = new Mock<IProductService>();

            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(cartMock.Object, _orderRepositoryMock.Object, productServiceMock.Object);
            
            _ordersViewModel = new OrderViewModel[]
            {
                new OrderViewModel { OrderId = 1, Name = "Order 1" , Address = "Address 1", City = "City 1", Zip = "Zip 1", Country = "Country 1"},
                new OrderViewModel { OrderId = 2, Name = "Order 2" }
            };
        }

        [Fact]
        public void GetOrder_ShouldReturnOrder()
        {
            // Arrange
            var order = new Order { Id = 1, Name = "Order 1" };
            _orderRepositoryMock.Setup(x => x.GetOrder(1)).ReturnsAsync(order);

            // Act
            var result = _orderService.GetOrder(1).Result;

            // Assert
            Assert.Equal(order, result);
        }

        [Fact]
        public void GetOrders_ShouldReturnOrders()
        {
            // Arrange
            var orders = new Order[]
            {
                new Order { Id = 1, Name = "Order 1" },
                new Order { Id = 2, Name = "Order 2" }
            };
            _orderRepositoryMock.Setup(x => x.GetOrders()).ReturnsAsync(orders);

            // Act
            var result = _orderService.GetOrders().Result;

            // Assert
            Assert.Equal(orders, result);
        }

        [Fact]
        public void SaveOrder_ShouldSaveOrder()
        {
            // Arrange
            var lines = new CartLine[]
            {
                new CartLine { Product = new Product { Id = 1, Name = "Product 1" }, Quantity = 20 },
                new CartLine { Product = new Product { Id = 2, Name = "Product 2" }, Quantity = 10 }
            };
            _ordersViewModel[0].Lines = lines;
            _orderRepositoryMock.Setup(x => x.Save(It.IsAny<Order>()));

            // Act
            _orderService.SaveOrder(_ordersViewModel[0]);

            // Assert
            _orderRepositoryMock.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void SaveOrder_ShouldThrowException()
        {
            // Arrange
            _orderRepositoryMock.Setup(x => x.Save(It.IsAny<Order>())).Throws(new System.Exception());

            // Act & Assert
            Assert.Throws<System.Exception>(() => _orderService.SaveOrder(_ordersViewModel[0]));
        }
    }
}