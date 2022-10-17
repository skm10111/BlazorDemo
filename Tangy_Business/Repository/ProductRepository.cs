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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Create(ProductDTO productDTO)
        {
            var product = _mapper.Map<ProductDTO,Product>(productDTO);            
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                _context.Products.Remove(obj);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductDTO> Get(int id)
        {
            var product = await _context.Products.Include(u => u.Category).Include(u => u.ProductPrices).FirstOrDefaultAsync(c => c.Id == id);
            if (product != null)
            {
                return _mapper.Map<Product, ProductDTO>(product);
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_context.Products.Include(u => u.Category).Include(u => u.ProductPrices));
        }

        public async Task Update(ProductDTO productDTO)
        {
            var obj = _context.Products.FirstOrDefault(c => c.Id == productDTO.Id);
            if (obj != null)
            {
                obj.Name = productDTO.Name;
                obj.CustomerFavorites = productDTO.CustomerFavorites;
                obj.ShopFavorites = productDTO.CustomerFavorites;
                obj.Color = productDTO.Color;
                obj.CategoryId = productDTO.CategoryId;
                obj.Description = productDTO.Description;
                obj.ImageUrl= productDTO.ImageUrl;
                _context.Products.Update(obj);
                await _context.SaveChangesAsync();
            }
        }
    }
}
