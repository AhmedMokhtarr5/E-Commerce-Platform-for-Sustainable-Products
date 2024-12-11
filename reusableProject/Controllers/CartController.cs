
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.Linq;


namespace yarab.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _webHostEnvironment;

        public CartController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {

            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpPost]
        [Route("api/createcart")]
        public async Task<IActionResult> CreateCart([FromBody] CartDto cartDto)
        {
            // Validate that UserId is provided in the request body
            if (cartDto.UserId <= 0)
            {
                return BadRequest("Invalid UserId.");
            }

            // Create the cart entity with the UserId from the body
            var cartEntity = new Cart
            {
                UserId = cartDto.UserId,  // Use the UserId from the request body

            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Carts.AddAsync(cartEntity);
            await _context.SaveChangesAsync();

            return Ok(cartEntity);
        }

        // GET: api/cart/{userId}
        [HttpGet("cartbyuserid/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)  // Assuming CartItems is a navigation property in the Cart entity
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found for the given user.");
            }

            return Ok(cart);
        }

        // PUT: api/cart/{userId}
        [HttpPut("updatecart/{userId}")]
        public async Task<IActionResult> UpdateCart(int userId, [FromBody] CartDto cartDto)
        {
            // Validate that UserId is provided
            if (cartDto.UserId != userId)
            {
                return BadRequest("UserId mismatch.");
            }

            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found for the given user.");
            }

            // Update the cart details (e.g., CreatedAt or other properties)


            // Handle cart items update
            if (cartDto.CartItems != null)
            {
                foreach (var cartItemDto in cartDto.CartItems)
                {
                    var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);
                    if (existingItem != null)
                    {
                        // Update quantity if item already exists
                        existingItem.Quantity = cartItemDto.Quantity;
                    }
                    else
                    {
                        // Add new item if it doesn't exist
                        var newItem = new CartItem
                        {
                            ProductId = cartItemDto.ProductId,
                            Quantity = cartItemDto.Quantity,
                            CartId = cart.CartId
                        };
                        cart.CartItems.Add(newItem);
                    }
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        // DELETE: api/cart/{userId}
        [HttpDelete("deletecart/{userId}")]
        public async Task<IActionResult> DeleteCart(int userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found for the given user.");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/cart/{userId}/item/{productId}
        [HttpDelete("deleteitem{userId}/item/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(int userId, int productId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found for the given user.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Item not found in the cart.");
            }

            cart.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }





    }
