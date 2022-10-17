using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tangy_Business.Repository.IRepository;
using TangyModels;

namespace TangyWeb_Api.Controllers
{    
    public class ProductController : BaseController
    {
       
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAll());
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult> Get(int? productId)
        {
            if(productId == null | productId == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {                   
                    ErrorMessage ="Invalid Id",
                    StatusCode=StatusCodes.Status400BadRequest
                });
            }
            var product = await _productRepository.Get(productId.Value);
            if(product.Id == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(product);
        }
    }
}
