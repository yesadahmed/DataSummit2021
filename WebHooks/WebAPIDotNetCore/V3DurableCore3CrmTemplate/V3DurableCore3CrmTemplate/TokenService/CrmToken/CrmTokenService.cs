
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using V3DurableCore3CrmTemplate.Helpers;

namespace V3DurableCore3CrmTemplate
{
	public class CrmTokenService : ICrmTokenService
	{
		private readonly AppConfigurations _config;

		public CrmTokenService(AppConfigurations config)
		{
			_config = config;

		}
		async Task<string> ICrmTokenService.GetCRMTokenAsync()
		{
			
			string Tenant = _config.TenantId;
			AuthenticationResult authResult = null;
			string clientId = _config.ClientId;
			string secret = _config.ClientSecret;
			string[] scope = new string[] { _config.scope };
		
			string authority = $"{_config.authority}{Tenant}";

			var clientApp = ConfidentialClientApplicationBuilder.Create(clientId: clientId)
			.WithClientSecret(clientSecret: secret)
			.WithAuthority(new Uri(authority))
			.Build();

			try
			{
				authResult = await clientApp.AcquireTokenForClient(scope).ExecuteAsync();

			}
			catch (Exception ex)
			{
				string error = ex.Message;
				if (ex.InnerException != null)
					error += ". Inner: " + ex.InnerException;

			}


			return authResult.AccessToken;
		}
	}
}
