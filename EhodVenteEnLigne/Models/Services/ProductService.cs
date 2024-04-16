using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
namespace EhodBoutiqueEnLigne.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICart _cart;
        private readonly IProductRepository _productRepository;
        public ProductService(ICart cart, IProductRepository productRepository)
        {
            _cart = cart;
            _productRepository = productRepository;
        }
        public List<ProductViewModel> GetAllProductsViewModel()
        {

            IEnumerable<Product> productEntities = GetAllProducts();
            return MapToViewModel(productEntities);
        }

        private static List<ProductViewModel> MapToViewModel(IEnumerable<Product> productEntities)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            foreach (Product product in productEntities)
            {
                products.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Stock = product.Quantity.ToString(),
                    Price = product.Price.ToString(CultureInfo.InvariantCulture),
                    Name = product.Name,
                    Description = product.Description,
                    Details = product.Details
                });
            }

            return products;
        }

        public List<Product> GetAllProducts()
        {
            IEnumerable<Product> productEntities = _productRepository.GetAllProducts();
            return productEntities?.ToList();
        }

        public ProductViewModel GetProductByIdViewModel(int id)
        {
            List<ProductViewModel> products = GetAllProductsViewModel().ToList();
            return products.Find(p => p.Id == id);
        }


        public Product GetProductById(int id)
        {
            List<Product> products = GetAllProducts().ToList();
            return products.Find(p => p.Id == id);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _productRepository.GetProduct(id);
            return product;
        }

        public async Task<IList<Product>> GetProduct()
        {
            var products = await _productRepository.GetProduct();
            return products;
        }
        public void UpdateProductQuantities()
        {
            Cart cart = (Cart)_cart;
            foreach (CartLine line in cart.Lines)
            {
                _productRepository.UpdateProductStocks(line.Product.Id, line.Quantity);
            }
        }

        /// <summary>
        /// Saves the product.
        /// </summary>
        /// <param name="product">The product to save.</param>
        /// <exception cref="System.Exception">Thrown when the product is not valid.</exception>
        public void SaveProduct(ProductViewModel product)
        {
            ProductValidator(product);

            var productToAdd = MapToProductEntity(product);
            _productRepository.SaveProduct(productToAdd);
        }

        private static Product MapToProductEntity(ProductViewModel product)
        {
            Product productEntity = new Product
            {
                Name = product.Name,
                Price = double.Parse(product.Price),
                Quantity = Int32.Parse(product.Stock),
                Description = product.Description,
                Details = product.Details
            };
            return productEntity;
        }

        public void DeleteProduct(int id)
        {
            // Get the product by id
            var product = GetProductById(id);

            // If the product doesn't exist in the inventory, throw an exception
            if (product == null)
            {
                // throw new InvalidOperationException("Product does not exist in the inventory.");
                return;
            }

            // Remove the product from the cart
            _cart.RemoveLine(product);

            // Delete the product from the inventory
            _productRepository.DeleteProduct(id);
        }


        private void ProductValidator(ProductViewModel product)
        {
            var context = new ValidationContext(product, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results);

            if (!isValid)
            {
                throw new Exception("\n" + string.Join("\n", results.Select(s => s.ErrorMessage).ToArray()));
            }
        }
    }
}
