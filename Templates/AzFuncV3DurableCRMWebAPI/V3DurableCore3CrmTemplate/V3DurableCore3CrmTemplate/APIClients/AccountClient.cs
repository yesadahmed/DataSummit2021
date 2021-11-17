using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.Helpers;
using V3DurableCore3CrmTemplate.Model;

namespace V3DurableCore3CrmTemplate.APIClients
{
	public class AccountClient : IAccountClient
	{
		#region Data members
		private readonly HttpClient _client;
		
		private readonly AppConfigurations config;
		#endregion


		#region Constructor
		public AccountClient(IHttpClientFactory httpClientFactory,  AppConfigurations Config)
		{
			this._client = httpClientFactory.CreateClient("crmclient");
	
			config = Config;
		}
		#endregion


		#region Implementation
		public async Task<Accounts> GetAllAccountsAsync()
		{
			Accounts accounts = new Accounts();
			try
			{
				var url = $"{config.apiurl}/accounts";
				var response = await _client.GetAsync(url);
				if(response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					accounts = JsonConvert.DeserializeObject<Accounts>(json);
				}

			}
			catch (Exception ex) 
			{
				
			}

			return accounts;
		}

		


		#endregion
	}
}
