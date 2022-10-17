using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangyModels;

namespace Tangy_Business.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<OrderDTO> Get(int id);
        Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status =null);
        Task<OrderDTO> Create(OrderDTO orderDTO);
        Task<int> Delete(int id);
        Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO);
        Task<OrderHeaderDTO> MarkPaymentSuccessful(int id, string paymentIntentId);
        Task<bool> UpdateOrderStatus(int orderId, string status);
        Task<OrderHeaderDTO> CancleOrder(int id);
    }
}
