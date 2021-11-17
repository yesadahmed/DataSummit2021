using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.Model;
using V3DurableCore3CrmTemplate.Parsers;

namespace V3DurableCore3CrmTemplate.AzureFunctions
{
	public static class FuncAccountOrchestrator
	{

		[FunctionName("AccountOrchestrator")]
		public static async Task FuncProcessJson(
		[OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
		{
			try
			{
				var contextJson = context.GetInput<string>();
				var parallelTasks = new List<Task>(); // FanIn/FanOut pattern (parallel executions)													  

				//parse object prepared
				JsonParser jsonParser = new JsonParser(log);
				AccountModel account = jsonParser.GetInfoOnCreateOrUpdateOrDelete(contextJson);//Crude

				if (account != null)
				{
					if (account.IsCreateMessage)
					{
						log.LogInformation("create message tirggred");
						// You can add delays as well if you want using Thread.Sleep /  Context delay
						//e.g: if some legacy plugins are processing heavy computation , you can delay this process
						parallelTasks.Add(context.CallActivityAsync("AccountOnCreateTrigger", account));//Add many task you want or chaninig if you need
					}
					else if (account.IsUpdateMessage)
					{
						log.LogInformation("update message tirggred");
						// You can add delays as well if you want using Thread.Sleep /  Context delay
						//e.g: if some legacy plugins are processing heavy computation , you can delay this process
						parallelTasks.Add(context.CallActivityAsync("AccountOnUpdateTrigger", account));//Add many task you want
					}
					else if (account.IsDeleteMessage)
					{
						log.LogInformation("delete message tirggred");
						// You can add delays as well if you want using Thread.Sleep /  Context delay
						//e.g: if some legacy plugins are processing heavy computation , you can delay this process
						parallelTasks.Add(context.CallActivityAsync("AccountOnDeleteTrigger", account));//Add many task you want
					}
					//Can add more PLUGN STEPS here if you want



					if (parallelTasks.Count > 0)
						await Task.WhenAll(parallelTasks);// run parallel
					else
						log.LogError($"No TASKS to trigger.");
				}
			}
			catch (Exception ex)
			{
				log.LogError($"AccountOrchestrator: {ex.Message} ");
			}

		}


	}
}
