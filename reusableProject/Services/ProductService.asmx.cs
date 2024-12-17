using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.ServiceModel;

namespace reusableProject.Services
{
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IProductService
    {
        [OperationContract]
        Task<ProductsDto> GetProductById(int id);

        [OperationContract]
        Task<List<ProductsDto>> GetProducts();

        [OperationContract]
        Task<bool> CreateProduct(ProductsDto product);

        [OperationContract]
        Task<bool> UpdateProduct(int id, ProductsDto updatedProduct);

        [OperationContract]
        Task<bool> DeleteProduct(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductsDto> GetProductById(int id)
        {

            return await _context.Products
                 .Where(product => product.ProductId == id)
                 .Select(product => new ProductsDto
                 {
                     ProductId = product.ProductId,
                     Name = product.Name,
                     Description = product.Description,
                     Price = product.Price,
                     Category = product.Category,
                     StockQuantity = product.StockQuantity
                 })
                 .FirstOrDefaultAsync();

        }

        public async Task<List<ProductsDto>> GetProducts()
        {
            return await _context.Products
                .Select(product => new ProductsDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Category = product.Category,
                    StockQuantity = product.StockQuantity
                })
                .ToListAsync();
        }

        public async Task<bool> CreateProduct(ProductsDto productDto)
        {
            var productEntity = new Product
            {

                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                StockQuantity = productDto.StockQuantity
            };

           try
            {
                await _context.Products.AddAsync(productEntity);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateProduct(int id, ProductsDto updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;


            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Category = updatedProduct.Category;
            product.StockQuantity = updatedProduct.StockQuantity;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
