using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangyModels;

namespace Tangy_Business.Repository.IRepository
{
	public interface IProductRepository
	{
        Task Create(ProductDTO productDTO);
        Task Update(ProductDTO productDTO);
        Task<int> Delete(int id);
        Task<IEnumerable<ProductDTO>> GetAll();
        Task<ProductDTO> Get(int id);
    }
}
