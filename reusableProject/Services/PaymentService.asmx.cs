using Microsoft.EntityFrameworkCore;
using reusableProject.Dtos;
using System.ServiceModel;

namespace reusableProject.Services
{

    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IPaymentService
    {
        [OperationContract]
        Task<PaymentDto> GetPaymentById(int id);

        [OperationContract]
        Task<List<PaymentDto>> GetPayments();

        [OperationContract]
        Task<bool> CreatePayment(PaymentDto paymentDto);

        [OperationContract]
        Task<bool> UpdatePayment(int id, PaymentDto updatedPayment);

        [OperationContract]
        Task<bool> DeletePayment(int id);
    }
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PaymentDto> GetPaymentById(int id)
        {

            return await _context.Payments
                 .Where(payment => payment.PaymentId == id)
                 .Select(payment => new PaymentDto
                 {
                     PaymentId = payment.PaymentId,
                     UserId = payment.UserId,
                     OrderId = payment.OrderId,
                     PaymentMethod = payment.PaymentMethod,
                     Amount = payment.Amount,
                     PaymentDate = payment.PaymentDate
                 })
                 .FirstOrDefaultAsync();
        }

        public async Task<List<PaymentDto>> GetPayments()
        {
            return await _context.Payments
               .Select(payment => new PaymentDto
               {
                   PaymentId = payment.PaymentId,
                   UserId = payment.UserId,
                   OrderId = payment.OrderId,
                   PaymentMethod = payment.PaymentMethod,
                   Amount = payment.Amount,
                   PaymentDate = payment.PaymentDate
               })
               .ToListAsync();
        }

        public async Task<bool> CreatePayment(PaymentDto paymentDto)
        {
            var paymentEntity = new Payment
            {
                UserId = paymentDto.UserId,
                OrderId = paymentDto.OrderId,
                PaymentMethod = paymentDto.PaymentMethod,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate
            };

           try
            {
                await _context.Payments.AddAsync(paymentEntity);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdatePayment(int id, PaymentDto updatedPayment)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;


            payment.UserId = updatedPayment.UserId;
            payment.OrderId = updatedPayment.OrderId;
            payment.PaymentMethod = updatedPayment.PaymentMethod;
            payment.Amount = updatedPayment.Amount;
            payment.PaymentDate = updatedPayment.PaymentDate;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
