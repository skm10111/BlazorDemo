using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TangyModels
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }

        [Required]
        public int ProductId { get; set; }       
        public   ProductDTO Product { get; set; }

        [Required]
        public int Count { get; set; }
        [Required]
        public double  Price { get; set; }
        [Required]
        public string DisplaySize { get; set; }
         [Required]
        public string ProductName { get; set; }
        

    }
}
