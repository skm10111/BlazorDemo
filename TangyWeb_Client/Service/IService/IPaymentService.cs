using TangyModels;

namespace TangyWeb_Client.Service.IService
{
    public interface IPaymentService
    {
       Task<SuccessModelDTO>  Checkout(StripePaymentDTO model);
    }
}
