using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
		private readonly ILogger log;
		private readonly AppConfigurations config;
		#endregion


		#region Constructor
		public AccountClient(IHttpClientFactory httpClientFactory, ILogger Log, AppConfigurations Config)
		{
			this._client = httpClientFactory.CreateClient("crmclient");
			this.log = Log;
			config = Config;
		}
		#endregion


		#region Implementation
		public async Task<List<AccountModel>> GetAllAccounts()
		{
			List<AccountModel> accounts = new List<AccountModel>();
			try
			{
				var url = $"{config.apiurl}/accounts";
				var response = await _client.GetAsync(url);
				if(response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					accounts = JsonConvert.DeserializeObject<List<AccountModel>>(json);
				}

			}
			catch (Exception ex) 
			{
				log.LogError($" GetAllAccounts: erorr: {ex.Message}");
			}

			return accounts;
		}


		#endregion
	}
}
