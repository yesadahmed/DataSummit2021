using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Model;

namespace V1DurableNetCRMTemplate.Handlers
{
	public class ProductAPI
	{
		private readonly HttpClient httpClient;
		TraceWriter log;

		public ProductAPI(HttpClient HTTPClient, TraceWriter Log)
		{
			httpClient = HTTPClient;

			log = Log;
		}


		public async Task<ProductModel> AddProduct(AccountModel AccountMdl)
		{
			ProductModel product = null;
			try
			{


				product = new ProductModel()
				{
					category = GetCategoryName(AccountMdl.new_products),
					description = AccountMdl.PrimaryEntityId,
					image = AccountMdl.new_proimageurl,
					price = AccountMdl.prenew_productprice,
					title = AccountMdl.new_productname
				};

				var stringPayload = JsonConvert.SerializeObject(product);

				// Wrap our JSON inside a StringContent which then can be used by the HttpClient class
				var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
				// Do the actual request and await the response
				var httpResponse = await httpClient.PostAsync("https://fakestoreapi.com/products", httpContent);

				// If the response contains content we want to read it!
				if (httpResponse.Content != null)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					product = JsonConvert.DeserializeObject<ProductModel>(responseContent);

				}
			}
			catch (Exception ex)
			{
				log.Error($"UpdateProduct: {ex.Message}");
			}

			return product;

		}



		string GetCategoryName(int number)
		{
			string name = "";
			switch (number)
			{
				case 100000000:
					name = "Elektronikk";
					break;


				case 100000001:
					name = "Klær herre";
					break;


				case 100000003:
					name = "Klær dame";
					break;



				case 100000002:
					name = "Sykkel";
					break;


				case 100000005:
					name = "Felleski";
					break;


				case 100000004:
					name = "Vinter Sko";
					break;


			}

			return name;

		}

	}
}
