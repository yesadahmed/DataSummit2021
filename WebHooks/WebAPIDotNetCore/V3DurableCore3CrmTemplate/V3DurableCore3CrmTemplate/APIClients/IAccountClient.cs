using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.Model;

namespace V3DurableCore3CrmTemplate.APIClients
{
	public interface IAccountClient
	{

		Task<List<AccountModel>> GetAllAccounts();
	}
}
