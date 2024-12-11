
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.Linq;


namespace yarab.Controllers
{
	public class OrderController : Controller
	{
          	ApplicationDbContext _context;
			IWebHostEnvironment _webHostEnvironment;

			public OrderController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
			{

				_webHostEnvironment = webHostEnvironment;
				_context = context;
			}


        [HttpGet("getorders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Json(orders);
        }

        // GET: api/order/{id}
        [HttpGet("getorderbyid{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        //// GET: api/order/user/{userId}
        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetOrdersByUserId(int userId)
        //{
        //    var orders = await _context.Orders
        //                                .Where(o => o.UserId == userId)
        //                                .ToListAsync();
        //    if (orders == null || orders.Count == 0)
        //        return NotFound("No orders found for this user.");

        //    return Ok(orders);
        //}

        // POST: api/order
        
        [HttpPost]
        [Route("api/createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            // Validate that UserId is provided in the request body
            if (orderDto.UserId <= 0)
            {
                return BadRequest("Invalid UserId.");
            }

            // Create the order entity with the UserId from the body
            var orderEntity = new Order
            {
                UserId = orderDto.UserId,  // Use the UserId from the request body
                OrderDate = orderDto.OrderDate,
                TotalAmount = orderDto.TotalAmount,
                OrderStatus = orderDto.OrderStatus
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            return Ok(orderEntity);
        }


        // PUT: api/order/{id}
        [HttpPut("updateorder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto updatedOrder)
        {
            if (id != updatedOrder.OrderId)
                return BadRequest("Order ID mismatch");

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            
            order.OrderDate = updatedOrder.OrderDate;
            order.TotalAmount = updatedOrder.TotalAmount;
            order.OrderStatus = updatedOrder.OrderStatus;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/order/{id}
        [HttpDelete("deleteorder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }










}
