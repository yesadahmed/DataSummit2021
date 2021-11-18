using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Handlers;
using V1DurableNetCRMTemplate.Model;

namespace V1DurableNetCRMTemplate.AzFunctions
{/// <summary>
 /// Adnan Samuel
 /// Feel free to extend to your needs
 /// </summary>
	public static class FunAccountOnCreateTrigger
	{
		private static HttpClient httpClient = new HttpClient();
		/// <summary>
		/// you can return any object/info you want
		/// </summary>
		/// <param name="arbeidsforhold"></param>
		/// <param name="log"></param>
		/// <returns></returns>
		[FunctionName("AccountOnCreateTrigger")]
		public async static Task<string> AccountCreateTrigger([ActivityTrigger] AccountModel account, TraceWriter log)
		{
			if (account != null)
			{
				CrmConnHandler crmManager = new CrmConnHandler();
				crmManager.InitializeCrmService(log);
				if (crmManager.CrmServiceReady)
				{
					//You can perform single/many like CRM and others types (Azure blob/Bus/queues/Sharepoint/PDF .........)
					// EXAMPLE just crm handler

										ProductAPI productMgr = new ProductAPI(httpClient, log);
					var product = await productMgr.AddProduct(account);
					log.Warning($"inside create product");
					if (product != null && product.id > 0) //update crm
					{
						log.Warning($"product.id: {product.id}");
						AccountHandler accountHandler = new AccountHandler(crmManager.crmService.Clone(), log);
						accountHandler.UpdateAccountProduct(product, new Guid(account.PrimaryEntityId));
						log.Warning($"  product created");
					}
				}

			}
			else
				log.Warning($"(AccountOnCreateTrigger): strange we must have full serialzed model.");

			return "";// Change according to your need
		}

	}
}
