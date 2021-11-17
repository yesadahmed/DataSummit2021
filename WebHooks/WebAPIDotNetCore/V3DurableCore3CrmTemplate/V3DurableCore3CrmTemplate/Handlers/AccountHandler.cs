using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.APIClients;
using V3DurableCore3CrmTemplate.Model;

namespace V3DurableCore3CrmTemplate.Handlers
{
	public class AccountHandler
	{

		#region Data Members

		private readonly IAccountClient accountClient;
		
		#endregion


		#region Constructor
		public AccountHandler(IAccountClient AccountClient)
		{
			accountClient = AccountClient;
			
		}
		#endregion


		#region Public functiosn

		public async Task<Accounts> GetAllAccountsAsync()
		{

			return await accountClient.GetAllAccountsAsync();

		}

		#endregion



		#region Private Functions

		#endregion
	}
}
