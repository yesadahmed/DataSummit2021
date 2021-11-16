using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace V1DurableNetCRMTemplate
{
	/// <summary>
	/// Adnan Samuel
	/// Feel free to extend to your needs
	/// </summary>
	public static class FunAccountHtttp
	{
		[FunctionName("AccountHtttp")]
		public static async Task<HttpResponseMessage> Run([HttpTrigger(WebHookType = "github")] HttpRequestMessage req,
			[OrchestrationClient] DurableOrchestrationClient starter, TraceWriter log)
		{


			// Get request body		
			string remoteExecutionJson = await req.Content.ReadAsStringAsync();// will be json RemoteExceutionContext

			//Fire & forget (This will do all the task/tasks)
			string instanceId = await starter.StartNewAsync("AccountOrchestrator", remoteExecutionJson);

			if (string.IsNullOrEmpty(instanceId))// (in DEV/TEST) plans
			{
				log.Error($"somthing went wrong. sending json to fault queue for further processing.");
				// SENT TO BUS/BLOB/QUEUE(Stroage)  for later processing
			}

			return new HttpResponseMessage(HttpStatusCode.OK);//just return control instantly
		}
	}
}
