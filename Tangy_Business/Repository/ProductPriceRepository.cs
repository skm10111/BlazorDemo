using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using TangyModels;

namespace Tangy_Business.Repository
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductPriceRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Create(ProductPriceDTO productPriceDTO)
        {
            var productPrice = _mapper.Map<ProductPriceDTO, ProductPrice>(productPriceDTO);         
            await _context.ProductPrices.AddAsync(productPrice);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _context.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                _context.ProductPrices.Remove(obj);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductPriceDTO> Get(int id)
        {
            var productPrices = await _context.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (productPrices != null)
            {
                return _mapper.Map<ProductPrice, ProductPriceDTO>(productPrices);
            }
            return new ProductPriceDTO();
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAll(int? Id =null)
        {
            if(Id != null && Id> 0)
			{
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_context.ProductPrices.Where(x => x.ProductId == Id));
            }
			else
			{
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_context.ProductPrices);
            }

        }

        public async Task Update(ProductPriceDTO productPriceDTO)
        {
            var obj = _context.ProductPrices.FirstOrDefault(c => c.Id == productPriceDTO.Id);
            if (obj != null)
            {
                obj.DisplaySize = productPriceDTO.DisplaySize;
                obj.Price = productPriceDTO.Price;           
                _context.ProductPrices.Update(obj);
                await _context.SaveChangesAsync();
            }
        }
    }
}
