
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.Linq;


namespace yarab.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _webHostEnvironment;

        public UserController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {

            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }


        [HttpGet]
        [Route("api/Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Json(users);
        }

        // GET: api/user/{id}
        [HttpGet("GetById/{id}")]


        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/user

        [HttpPost("api/createuser")]
        public async Task<IActionResult> CreateUser([FromBody] UsersDto userDto)
        {
            var userEntity = new User
            {
                UserId = userDto.UserId,
                Name = userDto.Name,
                Email = userDto.Email,
                password = userDto.password,
                Area = userDto.Area,
                Address = userDto.Address,
                MobileNumber = userDto.MobileNumber
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return Ok(userEntity);
        }

        // PUT: api/user/{id}
        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UsersDto updatedUser)
        {
            if (id != updatedUser.UserId)
                return BadRequest("User ID mismatch");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.password = user.password;
            user.Address = updatedUser.Address;
            user.Area = updatedUser.Area;
            user.MobileNumber = updatedUser.MobileNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //// DELETE: api/user/{id}
        [HttpDelete("deleteById/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
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
