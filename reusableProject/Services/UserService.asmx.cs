using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.ServiceModel;

namespace reusableProject.Services
        
{
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IUserService
    {
        [OperationContract]
        Task<List<UsersDto>> GetUsers();

        [OperationContract]
        Task<UsersDto> GetUserById(int id);

        [OperationContract]
        Task<int> CreateUser(UsersDto userDto);

        [OperationContract]
        Task<bool> UpdateUser(int id, UsersDto updatedUser);

        [OperationContract]
        Task<bool> DeleteUser(int id);
    }


    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsersDto> GetUserById(int id)
        {
            return await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UsersDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    password = u.password,
                    Area = u.Area,
                    Address = u.Address,
                    MobileNumber = u.MobileNumber
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<UsersDto>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UsersDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    password = u.password,
                    Area = u.Area,
                    Address = u.Address,
                    MobileNumber = u.MobileNumber
                })
                .ToListAsync();
        }

        public async Task<int> CreateUser(UsersDto userDto)
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


            try
            {
                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
            

            return userEntity.UserId;
        }

        public async Task<bool> UpdateUser(int id, UsersDto updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.password = updatedUser.password;
            user.Address = updatedUser.Address;
            user.Area = updatedUser.Area;
            user.MobileNumber = updatedUser.MobileNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}