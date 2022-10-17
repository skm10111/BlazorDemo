using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TangyModels
{
	public class ProductPriceDTO
	{
		public int Id { get; set; }
		[Required]
		public int ProductId { get; set; }
		[Required]
		public string DisplaySize { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage ="Price must be greater then 1")]
		public double Price { get; set; }
	}
}
