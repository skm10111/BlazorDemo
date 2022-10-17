using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangyModels;

namespace Tangy_Business.Repository.IRepository
{
    public interface IProductPriceRepository
    {
        Task Create(ProductPriceDTO productPriceDTO);
        Task Update(ProductPriceDTO productPriceDTO);
        Task<int> Delete(int id);
        Task<IEnumerable<ProductPriceDTO>> GetAll(int? id = null);
        Task<ProductPriceDTO> Get(int id);
    }
}
