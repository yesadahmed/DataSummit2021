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
					description = AccountMdl.new_productname,
					image = AccountMdl.new_proimageurl,
					price = AccountMdl.new_productprice,
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


		public async Task<ProductModel> UpdateProduct(AccountModel AccountMdl)
		{
			ProductModelEx product = null;
			ProductModel result = null;
			try
			{

				var productid = Convert.ToInt32(AccountMdl.new_userid);

				product = new ProductModelEx()
				{

					category = GetCategoryName(AccountMdl.new_products),
					description = AccountMdl.new_productname,
					image = AccountMdl.new_proimageurl,
					price = AccountMdl.new_productprice,
					title = AccountMdl.new_productname
				};
				HttpResponseMessage httpResponse;


				var stringPayload = JsonConvert.SerializeObject(product);

				// Wrap our JSON inside a StringContent which then can be used by the HttpClient class
				var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");



				var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://fakestoreapi.com/products/{productid}");

				request.Content = httpContent;
				httpResponse = await httpClient.SendAsync(request);

				// Do the actual request and await the response

				// If the response contains content we want to read it!
				if (httpResponse.Content != null)
				{
					var responseContent = await httpResponse.Content.ReadAsStringAsync();
					result = JsonConvert.DeserializeObject<ProductModel>(responseContent);

				}
			}
			catch (Exception ex)
			{
				log.Error($"UpdateProduct: {ex.Message}");
			}

			return result;

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
