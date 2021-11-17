using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.Model;

namespace V3DurableCore3CrmTemplate.APIClients
{
	public interface IAccountClient
	{
		
		//custom direct mapping
		Task<Accounts> GetAllAccountsAsync();


		// ms webapi CDSService style can be injected here
		//https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/samples/cdswebapiservice


	}
}
