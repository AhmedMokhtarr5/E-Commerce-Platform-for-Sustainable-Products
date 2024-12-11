
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace yarab.Controllers
{
	public class ProductController : Controller
	{
          	ApplicationDbContext _context;
			IWebHostEnvironment _webHostEnvironment;

			public ProductController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
			{

				_webHostEnvironment = webHostEnvironment;
				_context = context;
			}

       









        [HttpGet]
        [Route("api/products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Json(products);
        }


        // GET: api/products/{id}
        [HttpGet("{id}")]
            public async Task<IActionResult> GetProductById(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }

        // POST: api/products
        [Route("api/createproducts")]
        [HttpPost]
            public async Task<IActionResult> CreateProduct([FromBody] ProductsDto product)
            {

                var productentity = new Product() { 
                
                ProductId= product.ProductId,
                    Name= product.Name,
                    Description= product.Description,
                    Price = product.Price,
                    Category = product.Category,
                    StockQuantity = product.StockQuantity

                };  

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _context.Products.AddAsync(productentity);
                await _context.SaveChangesAsync();

                return Ok(productentity);
            }

            // PUT: api/products/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductsDto updatedProduct)
            {
                if (id != updatedProduct.ProductId)
                    return BadRequest("Product ID mismatch");

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                product.Name = updatedProduct.Name;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.Category = updatedProduct.Category;
                product.StockQuantity = updatedProduct.StockQuantity;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/products/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
      


    public string GreetVisitor()
		{
			return "Welcome to Managment Website!";
		}

		public string GreetUser(string name)
		{
			return $"Hi {name}\nHow are you?";
		}

		public string GetAge(string name, int birthYear)
		{
			return $"Hi {name}\nYou are {DateTime.Now.Year - birthYear} years old.";
		}
	}
}
