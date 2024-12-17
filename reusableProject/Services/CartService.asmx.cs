using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.ServiceModel;

namespace reusableProject.Services
{
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface ICartService
    {
        [OperationContract]
        Task<bool> CreateCart(CartDto cartDto);

        [OperationContract]
        Task<CartDto> GetCartByUserId(int userId);

        [OperationContract]
        Task<CartDto> UpdateCart(int userId ,CartDto cartDto);

        [OperationContract]
        Task<bool> DeleteCart(int userId);

        [OperationContract]
        Task<bool> RemoveItemFromCart(int userId,int productId);


    }
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCart(CartDto cartDto)
        {
            if (cartDto.UserId <= 0)
                return false;

            var cartEntity = new Cart
            {
                UserId = cartDto.UserId
            };

            await _context.Carts.AddAsync(cartEntity);
            await _context.SaveChangesAsync();

            return true;

            //return new CartDto { CartId = cartEntity.CartId, UserId = cartEntity.UserId };
        }

        public async Task<CartDto> GetCartByUserId(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            return new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };
        }
        //TODO: Update this function to handle insert/delete
        public async Task<CartDto> UpdateCart(int userId, CartDto cartDto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            if (cartDto.CartItems != null)
            {
                foreach (var cartItemDto in cartDto.CartItems)
                {
                    var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = cartItemDto.Quantity;
                    }
                    else
                    {
                        cart.CartItems.Add(new CartItem
                        {
                            ProductId = cartItemDto.ProductId,
                            Quantity = cartItemDto.Quantity,
                            CartId = cart.CartId
                        });
                    }
                }
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cartDto;
        }

        public async Task<bool> DeleteCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemFromCart(int userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return false;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
                return false;

            cart.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
