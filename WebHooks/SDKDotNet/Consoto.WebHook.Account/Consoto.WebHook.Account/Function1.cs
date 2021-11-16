using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Consoto.WebHook.Account
{
	public static class Function1
	{
		[FunctionName("Function1")]
		public static async Task<HttpResponseMessage> Run([HttpTrigger(WebHookType = "github")] HttpRequestMessage req, TraceWriter log)
		{
			log.Info("C# HTTP trigger function processed a request.");

			// Get request body
			dynamic data = await req.Content.ReadAsAsync<object>();

			// Extract github comment from request body
			string gitHubComment = data?.comment?.body;

			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
