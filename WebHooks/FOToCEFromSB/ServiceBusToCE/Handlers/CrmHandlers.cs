using Microsoft.Azure.WebJobs.Host;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using ServiceBusToCE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusToCE.Handlers
{
	public class CrmHandlers
	{
		#region Class Members
		public   CrmServiceClient crmService { get; set; }
		TraceWriter log;
		#endregion


		#region constructor
		public CrmHandlers(TraceWriter Log)
		{
			log = Log;
			
		}
		#endregion


		#region Public Functions

		public void InitializeCrmService()
		{
			string crmConnectionString = System.Environment.GetEnvironmentVariable("CrmConnection");

			try
			{
				if (!string.IsNullOrEmpty(crmConnectionString))
				{
					crmService = new CrmServiceClient(crmConnectionString);
					if (!crmService.IsReady)
					{
						if (crmService.LastCrmException != null)
							throw crmService.LastCrmException;
						throw new InvalidOperationException(crmService.LastCrmError);
					}

					#region Optimize Connection settings

					//Change max connections from .NET to a remote service default: 2
					System.Net.ServicePointManager.DefaultConnectionLimit = 65000;
					//Bump up the min threads reserved for this app to ramp connections faster - minWorkerThreads defaults to 4, minIOCP defaults to 4
					System.Threading.ThreadPool.SetMinThreads(100, 100);
					//Turn off the Expect 100 to continue message - 'true' will cause the caller to wait until it round-trip confirms a connection to the server
					System.Net.ServicePointManager.Expect100Continue = false;
					//Can decreas overall transmission overhead but can cause delay in data packet arrival
					System.Net.ServicePointManager.UseNagleAlgorithm = false;

					#endregion Optimize Connection settings

				}
				else
				{
					log.Error("CRMConnection string empty. ENDING Application");
				}

			}
			catch (Exception ex)
			{
				log.Error($"CrmManager.Error:InitializeCrmService {ex.Message}");
			}

		}


		public void CreteOrders(Orders order)
		{
			Random random = new Random();

			try
			{
				var accountId = GetCustomer(order.VendorAccount);
				Entity salesOrder = new Entity("salesorder", accountId);
				salesOrder["totalamount"] = new Money(order.TransactionCurrencyAmount);
				salesOrder["customerid"] = new EntityReference("account", accountId);
				salesOrder["ordernumber"] = order.PurchaseOrderNumber;
				salesOrder["name"] = order.PurchaseJournal;
				salesOrder["pricelevelid"] = new EntityReference("pricelevel", new Guid("65029c08-f01f-eb11-a812-000d3a33e825"));
				salesOrder["description"] = "order for " + order.VendorAccount;
				salesOrder["requestdeliveryby"] = DateTime.Now.AddDays(2);
				salesOrder["datefulfilled"] = DateTime.Now.AddDays(8);

				

				salesOrder["shippingmethodcode"] = new OptionSetValue(2);
				salesOrder["paymenttermscode"] = new OptionSetValue(1);
				salesOrder["freighttermscode"] = new OptionSetValue(1);

				var salesOrderId = crmService.Create(salesOrder);
				////

				Entity salesOrderDetail = new Entity("salesorderdetail");
				salesOrderDetail["productid"] = new EntityReference("product", new Guid("8d197fd0-f71f-eb11-a813-000d3a33f3b4"));
				salesOrderDetail["salesorderid"] = new EntityReference("salesorder", salesOrderId);
				salesOrderDetail["uomid"] = new EntityReference("uom", new Guid("a23fa8b6-0367-4023-9f8a-cab96d40adf0"));
				salesOrderDetail["priceperunit"] = order.TransactionCurrencyAmount;
				salesOrderDetail["quantity"] = 1m;
				salesOrderDetail["extendedamount"] = order.TransactionCurrencyAmount;
				var salesOrderdetailId = crmService.Create(salesOrderDetail);
			}
			catch (Exception ex)
			{

				throw;
			}

		}


		Guid GetCustomer(string accountnumber)
		{
			Guid accountId = Guid.Empty;

			string fetchxml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
								  <entity name='account'>
									<attribute name='name' />
									<attribute name='primarycontactid' />
									<attribute name='telephone1' />
									<attribute name='accountid' />
									<order attribute='name' descending='false' />
									<filter type='and'>
									  <condition attribute='accountnumber' operator='eq' value='{accountnumber}' />
									</filter>
								  </entity>
								</fetch>";


			var result = crmService.RetrieveMultiple(new FetchExpression(fetchxml));
			if (result != null && result.Entities.Count > 0)
			{
				var record = result.Entities.FirstOrDefault();
				accountId = record.Id;
			}
			return accountId;

		}
		#endregion

		#region  Private Functions


		#endregion



	}
}
