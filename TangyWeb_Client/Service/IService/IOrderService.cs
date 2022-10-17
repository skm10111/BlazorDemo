using TangyModels;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service.IService
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAll(string? userId);
        Task<OrderDTO> Get(int orderId);
        Task<OrderDTO> Create(StripePaymentDTO paymentDTO);
        Task<OrderHeaderDTO> MakePaymentSuccessful(OrderHeaderDTO orderHeaderDTO);
    }
}
