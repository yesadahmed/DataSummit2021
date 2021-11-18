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
{
	public class FuncAccountOnUpdateTrigger
	{
		private static HttpClient httpClient = new HttpClient();


		[FunctionName("AccountOnUpdateTrigger")]
		public async static Task<string> AccounUpdateTrigger([ActivityTrigger] AccountModel account, TraceWriter log)
		{
			log.Warning($"account : {account != null}");

			if (account != null)
			{
				log.Warning($" before crm");
				CrmConnHandler crmManager = new CrmConnHandler();
				crmManager.InitializeCrmService(log);
				if (crmManager.CrmServiceReady)
				{
					log.Warning($" inside crm");
					//You can perform single/many tasks like CRM and others types (Azure blob/Bus/queues/Sharepoint/PDF .........)
					// EXAMPLE just crm handler
					ProductAPI productMgr = new ProductAPI(httpClient, log);
					var product = await productMgr.UpdateProduct(account);
					log.Warning($"after oroduct result.");
					if (product != null && product.id > 0) //update crm
					{
						log.Warning($" product.id:  { product.id}");
						AccountHandler accountHandler = new AccountHandler(crmManager.crmService.Clone(), log);
						accountHandler.UpdateAccountProduct(product, new Guid(account.PrimaryEntityId));
						log.Warning($" account updated");
					}
					else
						log.Warning($" account updated");

				}


			}
			else
				log.Warning($"(AccountOnUpdateTrigger): strange we must have full serialzed model.");

			return "";// Change according to your need
		}


	}
}
