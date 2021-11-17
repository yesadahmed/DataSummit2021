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
	public class FuncAccountOnDeleteTrigger
	{

		#region Data Members

		private readonly IAccountClient accountClient;

		#endregion


		public FuncAccountOnDeleteTrigger(IAccountClient AccountClient)
		{
			accountClient = AccountClient;

		}


		[FunctionName("AccountOnDeleteTrigger")]
		public async Task<string> DeleteAccountTrigger([ActivityTrigger] AccountModel account, ILogger log)
		{
			if (account != null)
			{
				var accounts = await accountClient.GetAllAccountsAsync();
				//Do somthing

			}
			else
				log.LogWarning($"(AccountOnDeleteTrigger): strange we must have full serialized model.");

			return "";//it coukd be task or some other result in orchrator
		}

	}
}
