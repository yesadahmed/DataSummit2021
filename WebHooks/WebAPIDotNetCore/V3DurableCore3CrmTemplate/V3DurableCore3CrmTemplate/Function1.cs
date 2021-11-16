using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace V3DurableCore3CrmTemplate
{
	public class Function1
	{

		private readonly HttpClient _client;


		public Function1(IHttpClientFactory httpClientFactory)
		{
			this._client = httpClientFactory.CreateClient("crmclient");

		}

		[FunctionName("Function1")]
		public static async Task<List<string>> RunOrchestrator(
			[OrchestrationTrigger] IDurableOrchestrationContext context)
		{
			var outputs = new List<string>();

			// Replace "hello" with the name of your Durable Activity Function.
			outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Tokyo"));
			//outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Seattle"));
			//outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "London"));

			// returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
			return outputs;
		}

		[FunctionName("Function1_Hello")]
		public string SayHello([ActivityTrigger] string name, ILogger log)
		{
			string webAPI = "https://org098cccaa.api.crm4.dynamics.com/api/data/v9.2/accounts";
			string conents = "";
			try
			{
				var response = _client.GetAsync(webAPI).Result;

				 conents = response.Content.ReadAsStringAsync().Result;
			}
			catch (System.Exception ex)
			{

				throw;
			}
			
			log.LogInformation($"Saying hello to55555555 {9}.");
			return $"Hello !";
		}

		[FunctionName("Function1_HttpStart")]
		public static async Task<HttpResponseMessage> HttpStart(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
			[DurableClient] IDurableOrchestrationClient starter,
			ILogger log)
		{
			// Function input comes from the request content.
			string instanceId = await starter.StartNewAsync("Function1", null);

			log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

			return starter.CreateCheckStatusResponse(req, instanceId);
		}
	}
}