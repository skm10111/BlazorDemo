using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModel;
using TangyModels;

namespace Tangy_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderHeaderDTO> CancleOrder(int id)
        {
            var orderHeader = await _context.OrderHeaders.FindAsync(id);
            if (orderHeader == null)
            {
                return new OrderHeaderDTO();
            }
            if(orderHeader.Status == SD.Status_Pending)
            {
                orderHeader.Status = SD.Status_Canceled;
                await _context.SaveChangesAsync();
            }
            if(orderHeader.Status == SD.Status_Confirmed)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    Amount = (long)orderHeader.OrderTotal,
                    PaymentIntent = orderHeader.PaymentintentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                orderHeader.Status = SD.Status_Refunded;
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
        }

        public async Task<OrderDTO> Create(OrderDTO orderDTO)
        {
            try
            {
                var obj = _mapper.Map<OrderDTO, Order>(orderDTO);
                _context.OrderHeaders.Add(obj.OrderHeader);
                await _context.SaveChangesAsync();

                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }
                _context.OrderDetails.AddRange(obj.OrderDetails);
                await _context.SaveChangesAsync();
                return new OrderDTO()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(obj.OrderDetails).ToList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orderDTO;
        }

        public async Task<int> Delete(int id)
        {
            var objheader = await _context.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);
            if (objheader != null)
            {
                IEnumerable<OrderDetail> orderDetails = _context.OrderDetails.Where(u => u.OrderHeaderId == id);
                _context.OrderDetails.RemoveRange(orderDetails);
                _context.OrderHeaders.Remove(objheader);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<OrderDTO> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = await _context.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id),
                OrderDetails = _context.OrderDetails.Where(u => u.OrderHeaderId == id)
            };
            if (order != null)
            {
                return _mapper.Map<Order, OrderDTO>(order);
            }
            return new OrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> order = new List<Order>();
            IEnumerable<OrderHeader> orderHeadersList = _context.OrderHeaders;
            IEnumerable<OrderDetail> orderDetailList = _context.OrderDetails;
            foreach (OrderHeader header in orderHeadersList)
            {
                Order tempOrder = new()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailList.Where(u => u.OrderHeaderId == header.Id)
                };
                order.Add(tempOrder);
            }
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(order);
        }

        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id, string paymentIntentId)
        {
            try
            {
                var data = await _context.OrderHeaders.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (data == null)
                {
                    return new OrderHeaderDTO();
                }
                if (data.Status == SD.Status_Pending)
                {
                    data.PaymentintentId = paymentIntentId;
                    data.Status = SD.Status_Confirmed;
                    await _context.SaveChangesAsync();
                    return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
                }
               
            }
            catch (Exception ex)
            {

            }
            return new OrderHeaderDTO();
        }

        public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
        {
            if (objDTO != null)
            {
                var orderHeaderDb =  await _context.OrderHeaders.FirstOrDefaultAsync(x => x.Id == objDTO.Id);
                orderHeaderDb.Name = objDTO.Name;
                orderHeaderDb.PhoneNumber = objDTO.PhoneNumber;
                orderHeaderDb.Carrier = objDTO.Carrier;
                orderHeaderDb.Tracking = objDTO.Tracking;
                orderHeaderDb.StreetAddress = objDTO.StreetAddress;
                orderHeaderDb.City = objDTO.City;
                orderHeaderDb.State = objDTO.State;
                orderHeaderDb.PostalCode = objDTO.PostalCode;
                orderHeaderDb.Status = objDTO.Status;
                orderHeaderDb.ShippingDate = DateTime.Now;
                var orderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(objDTO);             
                await _context.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeaderDb);
            }
            return new OrderHeaderDTO();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _context.OrderHeaders.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (status == SD.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
