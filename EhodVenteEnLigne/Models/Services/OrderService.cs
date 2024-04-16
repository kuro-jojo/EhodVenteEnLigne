using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Order = EhodBoutiqueEnLigne.Models.Entities.Order;

namespace EhodBoutiqueEnLigne.Models.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public OrderService(ICart cart, IOrderRepository orderRepository, IProductService productService)
        {
            _orderRepository = orderRepository;
            _cart = cart;
            _productService = productService;
        }
        public async Task<Order> GetOrder(int id)
        {
            var orderEntity = await _orderRepository.GetOrder(id);
            return orderEntity;
        }
        public async Task<IList<Order>> GetOrders()
        {
            var orders = await _orderRepository.GetOrders();
            return orders;
        }

        /// <summary>
        /// Saves the order.
        /// </summary>
        /// <param name="order">The order to save.</param>
        /// <exception cref="System.Exception">Thrown when the order is not valid.</exception>
        public void SaveOrder(OrderViewModel order)
        {
            OrderValidator(order);
            var orderToAdd = MapToOrderEntity(order);
            _orderRepository.Save(orderToAdd);
            UpdateInventory();
        }

        private static Order MapToOrderEntity(OrderViewModel order)
        {
            Order orderToAdd = new Order
            {
                Name = order.Name,
                Address = order.Address,
                City = order.City,
                Zip = order.Zip,
                Country = order.Country,
                Date = DateTime.UtcNow,
                OrderLine = new List<OrderLine>()
            };
            foreach (var orderLine in order.Lines)
            {
                OrderLine lineOrder = new OrderLine { ProductId = orderLine.Product.Id, Quantity = orderLine.Quantity };
                orderToAdd.OrderLine.Add(lineOrder);
            }

            return orderToAdd;
        }

        private void UpdateInventory()
        {
            _productService.UpdateProductQuantities();
            _cart.Clear();
        }

        private void OrderValidator(OrderViewModel order)
        {
            var context = new ValidationContext(order, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, context, results);

            if (order.Lines == null)
            {
                throw new Exception("\n" + "Please add products to the order.");
            }
            if (!isValid)
            {
                throw new Exception("\n" + string.Join("\n", results.Select(s => s.ErrorMessage).ToArray()));
            }
        }
    }
}
