using Microsoft.Azure.WebJobs.Host;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Helper;
using V1DurableNetCRMTemplate.Model;

namespace V1DurableNetCRMTemplate.Handlers
{
	/// <summary>
	/// Just domenstration how you can use CRM connection
	/// make your own handler for  CRM entity
	/// https://fakestoreapi.com/docs
	/// </summary>
	public class AccountHandler
	{

		#region Data members
		CrmServiceClient crmService { get; set; }
		TraceWriter log;
		#endregion


		#region Constructor
		public AccountHandler(CrmServiceClient CRMService, TraceWriter Log) ///per instance works  :)
		{
			crmService = CRMService;
			log = Log;
		}
		#endregion

		#region Public Fucntions


		public void UpdateAccountProduct(ProductModel product, Guid accountId)
		{

			try
			{
				var stringPayload = JsonConvert.SerializeObject(product);
				Entity account = new Entity("account", accountId);
				account["new_userid"] = product.id.ToString();
				account["new_userjson"] = stringPayload;
				crmService.Update(account);

			}
			catch (Exception ex)
			{
				var error = UtilityFunctions.GetCrmFaultExceptionAny(ex);
				log.Info($"(UpdateAccountProduct): error : {error}");
			}

		}


		public void WhoAmICallAndDisplayOrganizationId()
		{
			try
			{
				WhoAmIResponse response = ((WhoAmIResponse)crmService.Execute(new WhoAmIRequest()));
				if (response != null)
					log.Info($"OrganizationId:  {response.OrganizationId}");
			}
			catch (Exception ex)
			{
				var error = UtilityFunctions.GetCrmFaultExceptionAny(ex);
				log.Info($"(WhoAmICall): error : {error}");
			}
		}

		#endregion



		#region Private Fucntions

		#endregion
	}
}
