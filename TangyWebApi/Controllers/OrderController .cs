using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Tangy_Business.Repository.IRepository;
using TangyModels;

namespace TangyWeb_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : Controller
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IEmailSender _emailSender;
        public OrderController(IOrderRepository orderRepository, IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _emailSender = emailSender;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _orderRepository.GetAll());
        }
        [HttpGet("{orderHeaderId}")]
        public async Task<ActionResult> Get(int? orderHeaderId)
        {
            if(orderHeaderId == null | orderHeaderId == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {                   
                    ErrorMessage ="Invalid Id",
                    StatusCode=StatusCodes.Status400BadRequest
                });
            }
            var orderHeader = await _orderRepository.Get(orderHeaderId.Value);
            if(orderHeader.OrderHeader.Id == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(orderHeader);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDTO paymentDTO)
        {
            paymentDTO.Order.OrderHeader.OrderDate = DateTime.Now;
            var result = await _orderRepository.Create(paymentDTO.Order);
            return Ok(result);
        }
        [HttpPost]
        [ActionName("PaymentSuccessful")]
        public async Task<IActionResult> PaymentSuccessful([FromBody] OrderHeaderDTO orderHeaderDTO)
        {
            var service = new SessionService();
            var sessionDetails = service.Get(orderHeaderDTO.SessionId);
            if(sessionDetails.PaymentStatus == "paid")
            {
                var result = await _orderRepository.MarkPaymentSuccessful(orderHeaderDTO.Id, sessionDetails.PaymentIntentId);
              await _emailSender.SendEmailAsync(orderHeaderDTO.Email, "tanhy Order Confirmation", "New Order has been create: "+ orderHeaderDTO.Id);
                if(result == null)
                {
                    return BadRequest(new ErrorModelDTO()
                    {
                        ErrorMessage="can not mark payment as successful"
                    });
                }
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
