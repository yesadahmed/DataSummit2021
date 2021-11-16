using Microsoft.Azure.WebJobs.Host;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Helper;

namespace V1DurableNetCRMTemplate.Handlers
{
	/// <summary>
	/// Just domenstration how you can use CRM connection
	/// make your own handler for  CRM entity
	/// </summary>
	public class DummyHandler
	{

		#region Data members
		CrmServiceClient crmService { get; set; }
		TraceWriter log;
		#endregion


		#region Constructor
		public DummyHandler(CrmServiceClient CRMService, TraceWriter Log) ///per instance works  :)
		{
			crmService = CRMService;
			log = Log;
		}
		#endregion

		#region Public Fucntions

		public void WhoAmICallAndDisplayOrganizationId()
		{
			try
			{
				WhoAmIResponse response = ((WhoAmIResponse)crmService.Execute(new WhoAmIRequest()));
				if (response != null)
					log.Error($"OrganizationId:  {response.OrganizationId}");
			}
			catch (Exception ex)
			{
				var error = UtilityFunctions.GetCrmFaultExceptionAny(ex);
				log.Error($"(WhoAmICall): error : {error}");
			}
		}

		#endregion



		#region Private Fucntions

		#endregion
	}
}
