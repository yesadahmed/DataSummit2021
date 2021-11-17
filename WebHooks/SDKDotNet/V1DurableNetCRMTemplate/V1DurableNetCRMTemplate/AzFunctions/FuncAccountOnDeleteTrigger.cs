using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Handlers;
using V1DurableNetCRMTemplate.Model;

namespace V1DurableNetCRMTemplate.AzFunctions
{
	public class FuncAccountOnDeleteTrigger
	{

		[FunctionName("AccountOnDeleteTrigger")]
		public static string AccountDeleteTrigger([ActivityTrigger] AccountModel account, TraceWriter log)
		{
			if (account != null)
			{
				CrmConnHandler crmManager = new CrmConnHandler();
				crmManager.InitializeCrmService(log);
				if (crmManager.CrmServiceReady)
				{
					//You can perform single/many like CRM and others types (Azure blob/Bus/queues/Sharepoint/PDF .........)
					// EXAMPLE just crm handler

					

				}
			}
			else
				log.Warning($"(AccountOnDeleteTrigger): strange we must have full serialzed model.");

			return "";// Change according to your need
		}


	}
}
