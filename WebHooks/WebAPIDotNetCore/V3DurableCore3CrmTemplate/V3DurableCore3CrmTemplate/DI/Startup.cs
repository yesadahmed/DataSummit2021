using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using V3DurableCore3CrmTemplate;
using V3DurableCore3CrmTemplate.APIClients;
using V3DurableCore3CrmTemplate.DI;
using V3DurableCore3CrmTemplate.Helpers;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{


			builder.Services.AddSingleton<AppConfigurations>();
			builder.Services.AddSingleton<ICrmTokenService, CrmTokenService>();
			builder.Services.AddTransient<DelegationHandler>();

			builder.Services.AddTransient<IAccountClient, AccountClient>();

			builder.Services.AddHttpClient("crmclient", c =>
			{
				//c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				c.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
				c.DefaultRequestHeaders.Add("OData-Version", "4.0");
				c.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");
			}
		).AddHttpMessageHandler<DelegationHandler>();
		}

		//YOUR OWN CRM/OTHER logger CAN BE HERE '
		
	}
}
