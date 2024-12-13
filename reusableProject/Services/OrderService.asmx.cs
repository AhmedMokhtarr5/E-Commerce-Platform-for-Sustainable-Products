using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.ServiceModel;

namespace reusableProject.Services
{
    [ServiceContract(Namespace = "http://tempuri.org/")]

    public interface IOrderService
    {
        [OperationContract]
        Task<OrderDto> GetOrderById(int id);

        [OperationContract]
        Task<List<OrderDto>> GetOrders();

        [OperationContract]
        Task<OrderDto> CreateOrder(OrderDto orderDto);

        [OperationContract]
        Task<bool> UpdateOrder(int id, OrderDto updatedOrder);

        [OperationContract]
        Task<bool> DeleteOrder(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDto> GetOrderById(int id)
        {

            return await _context.Orders
                 .Where(order => order.OrderId == id)
                 .Select(order => new OrderDto
                 {
                     UserId = order.UserId, 
                     OrderDate = order.OrderDate,
                     TotalAmount = order.TotalAmount,
                     OrderStatus = order.OrderStatus
                 })
                 .FirstOrDefaultAsync();
        }

        public async Task<List<OrderDto>> GetOrders()
        {
            return await _context.Orders
              .Select(order => new OrderDto
              {
                  UserId = order.UserId,  
                  OrderDate = order.OrderDate,
                  TotalAmount = order.TotalAmount,
                  OrderStatus = order.OrderStatus
              })
              .ToListAsync();
        }

        public async Task<OrderDto> CreateOrder(OrderDto orderDto)
        {
            var orderEntity = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate,
                TotalAmount = orderDto.TotalAmount,
                OrderStatus = orderDto.OrderStatus
            };

            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            // Create and return OrderDto from the saved entity
            var createdOrderDto = new OrderDto
            {
                OrderId = orderEntity.OrderId,
                UserId = orderEntity.UserId,
                OrderDate = orderEntity.OrderDate,
                TotalAmount = orderEntity.TotalAmount,
                OrderStatus = orderEntity.OrderStatus
            };

            return createdOrderDto;
        }

        public async Task<bool> UpdateOrder(int id, OrderDto updatedOrder)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;


            order.UserId = updatedOrder.UserId;
            order.OrderDate = updatedOrder.OrderDate;
            order.TotalAmount = updatedOrder.TotalAmount;
            order.OrderStatus = updatedOrder.OrderStatus;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
