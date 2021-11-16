using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Model;
using V1DurableNetCRMTemplate.Parsers;

namespace V1DurableNetCRMTemplate.AzFunctions
{
	public class FunAccountOrchestrator
	{
		/// <summary>
		/// Adnan Samuel
		/// Feel free to extend to your needs
		/// </summary>
		[FunctionName("AccountOrchestrator")]
		public static async Task RunOrchestrator([OrchestrationTrigger] DurableOrchestrationContext context, TraceWriter log)
		{
			try
			{
				string contextJson = context.GetInput<string>();
				var parallelTasks = new List<Task>(); // FanIn/FanOut pattern (parallel executions)													  
				
				//parse object prepared
				JsonParser jsonParser = new JsonParser(log);
				AccountModel account = jsonParser.GetInfoOnCreateOrUpdateOrDelete(contextJson);//Crude

				if (account != null)
				{
					if (account.IsCreateMessage)
					{
						log.Info("create message tirggred");
						// You can add delays as well if you want using Thread.Sleep /  Context delay
						//e.g: if some legacy plugins are processing heavy computation , you can delay this process
						parallelTasks.Add(context.CallActivityAsync("OnCreateArbeidsforholdTrigger", account));//Add many task you want or chaninig if you need
					}
					else if (account.IsUpdateMessage)
					{
						log.Info("update message tirggred");
						// You can add delays as well if you want using Thread.Sleep /  Context delay
						//e.g: if some legacy plugins are processing heavy computation , you can delay this process
						parallelTasks.Add(context.CallActivityAsync("OnUpdateArbeidsforholdTrigger", account));//Add many task you want
					}
					else if (account.IsDeleteMessage)
					{
						log.Info("delete message tirggred");
						// You can add delays as well if you want using Thread.Sleep /  Context delay
						//e.g: if some legacy plugins are processing heavy computation , you can delay this process
						parallelTasks.Add(context.CallActivityAsync("OnDeleteArbeidsforholdTrigger", account));//Add many task you want
					}
					//Can add more PLUGN STEPS here if you want



					if (parallelTasks.Count > 0)
						await Task.WhenAll(parallelTasks);// run parallel
					else
						log.Error($"No TASKS to trigger.");
				}
			}
			catch (Exception ex)
			{
				log.Error($"AccountOrchestrator: {ex.Message} ");
			}
		}
	}
}
