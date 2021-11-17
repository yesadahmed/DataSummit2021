using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.APIClients;
using V3DurableCore3CrmTemplate.Model;

namespace V3DurableCore3CrmTemplate.AzureFunctions
{
	public class FuncAccountOnCreateTrigger
	{

		#region Data Members

		private readonly IAccountClient accountClient;
		
		#endregion


		public FuncAccountOnCreateTrigger(IAccountClient AccountClient)
		{
			accountClient = AccountClient;

		}


		[FunctionName("AccountOnCreateTrigger")]
		public async Task<string> CreateAccountTrigger([ActivityTrigger] AccountModel account, ILogger log)
		{
			if (account != null)
			{
				var accounts= await accountClient.GetAllAccountsAsync();
				//Do somthing

			}
			else
				log.LogWarning($"(AccountOnCreateTrigger): strange we must have full serialized model.");

			return "";//it coukd be task or some other result in orchrator
		}

	}
}
