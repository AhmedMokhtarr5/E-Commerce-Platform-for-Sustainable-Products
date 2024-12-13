
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.Linq;


namespace yarab.Controllers
{
    public class PaymentController : Controller
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _webHostEnvironment;

        public PaymentController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {

            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }


        [HttpGet("Getpayments")]
        public async Task<IActionResult> GetPayments()
        {
            var payments = await _context.Payments.ToListAsync();
            return Json(payments);
        }

        // GET: api/payment/{id}
        [HttpGet("getpaymnetbyid/{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        // GET: api/payment/user/{userId}
        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetPaymentsByUserId(int userId)
        //{
        //    var payments = await _context.Payments
        //                                  .Where(p => p.UserId == userId)
        //                                  .ToListAsync();
        //    if (payments == null || payments.Count == 0)
        //        return NotFound("No payments found for this user.");

        //    return Ok(payments);
        //}

        // POST: api/payment
      
        [HttpPost("api/createpayment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            var paymentEntity = new Payment
            {
                PaymentId = paymentDto.PaymentId,
                UserId = paymentDto.UserId,
                OrderId = paymentDto.OrderId,
                PaymentMethod = paymentDto.PaymentMethod,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Payments.AddAsync(paymentEntity);
            await _context.SaveChangesAsync();

            return Ok(paymentEntity);
        }

        // PUT: api/payment/{id}
        [HttpPut("updatePayment/{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentDto updatedPayment)
        {
            if (id != updatedPayment.PaymentId)
                return BadRequest("Payment ID mismatch");

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            payment.UserId = updatedPayment.UserId;
            payment.OrderId = updatedPayment.OrderId;
            payment.PaymentMethod = updatedPayment.PaymentMethod;
            payment.Amount = updatedPayment.Amount;
            payment.PaymentDate = updatedPayment.PaymentDate;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/payment/{id}
        [HttpDelete("DeletePayment/{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            _context.Payments.Remove(payment);
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
