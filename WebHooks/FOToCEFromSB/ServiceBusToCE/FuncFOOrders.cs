using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ServiceBusToCE.Handlers;
using ServiceBusToCE.Models;
using System.Threading.Tasks;

namespace ServiceBusToCE
{
	public static class FuncFOOrders
	{
		[FunctionName("FOOrders")]
		public static void Run([ServiceBusTrigger("%Topic%", "%TopicSub%", AccessRights.Manage, Connection = "SBConnection")] string mySbMsg, TraceWriter log)
		{
			CrmHandlers crmManager = new CrmHandlers(log);
			log.Info($" {mySbMsg}");
			Orders order = JsonConvert.DeserializeObject<Orders>(mySbMsg,
					new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			if (order != null)
			{
				log.Info($" after searlize");
				crmManager.InitializeCrmService();
				if (crmManager.crmService.IsReady)
				{
					crmManager.CreteOrders(order);

				}
			}

		}
	}
}
