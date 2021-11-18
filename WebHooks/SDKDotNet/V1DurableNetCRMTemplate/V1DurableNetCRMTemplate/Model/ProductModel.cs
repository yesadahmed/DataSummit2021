using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1DurableNetCRMTemplate.Model
{
	public class ProductModel
	{
		public string title { get; set; }
		public double price { get; set; }
		public string description { get; set; }
		public string image { get; set; }
		public string category { get; set; }
		public int id { get; set; }
	}

	public class ProductModelEx
	{
		public string title { get; set; }
		public double price { get; set; }
		public string description { get; set; }
		public string image { get; set; }
		public string category { get; set; }
		
	}
}
