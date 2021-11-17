using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.Model;

namespace V3DurableCore3CrmTemplate.AzureFunctions
{
	public static class FuncAccountHttp
	{
		[FunctionName("AccountHttp")]
		public static async Task<HttpResponseMessage> HttpStart(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req,
			[DurableClient] IDurableOrchestrationClient starter,
			ILogger log)
		{
			string remoteExecutionJson = await req.Content.ReadAsStringAsync();// will be json RemoteExceutionContext

			GenericModel genericModel = new GenericModel();
			genericModel.RemoteExecutionContext = remoteExecutionJson;
			// Function input comes from the request content.
			string instanceId = await starter.StartNewAsync<string>("AccountOrchestrator", remoteExecutionJson);

			if (string.IsNullOrEmpty(instanceId))// (in DEV/TEST) plans
			{
				log.LogError($"somthing went wrong. sending json to fault queue for further processing.");
				// SENT TO BUS/BLOB/QUEUE(Stroage)  for later processing (Health check ) important
			}

			return starter.CreateCheckStatusResponse(req, instanceId);

		}
	}
}
