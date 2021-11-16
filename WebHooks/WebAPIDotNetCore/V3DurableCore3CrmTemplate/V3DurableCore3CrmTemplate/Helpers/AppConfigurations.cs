using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace V3DurableCore3CrmTemplate.Helpers
{
	public class AppConfigurations
	{
		private readonly IConfiguration _configuration;

		public AppConfigurations(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string ClientId { get { return _configuration.GetValue<string>("ClientId"); } }
		
		public string TenantId { get { return _configuration.GetValue<string>("TenantId"); } }
		public string scope { get { return _configuration.GetValue<string>("scope"); } }
		public string ClientSecret { get { return _configuration.GetValue<string>("ClentSecret"); } }
		public string authority { get { return _configuration.GetValue<string>("authority"); } }
		public string apiurl { get { return _configuration.GetValue<string>("apiurl"); } }
		

	}
}

