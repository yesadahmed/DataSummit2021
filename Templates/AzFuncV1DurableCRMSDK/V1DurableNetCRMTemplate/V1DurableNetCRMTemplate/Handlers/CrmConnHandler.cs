using Microsoft.Azure.WebJobs.Host;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V1DurableNetCRMTemplate.Helper;

namespace V1DurableNetCRMTemplate.Handlers
{
	public class CrmConnHandler
	{

		#region Data members
		public CrmServiceClient crmService { get; set; }
		public bool CrmServiceReady { get; set; }
		TraceWriter log;
		#endregion


		#region Public Functions
		public void InitializeCrmService(TraceWriter _log)
		{
			string crmConnectionString = AppConfig.CRMConnection;
			log = _log;
			try
			{
				if (!string.IsNullOrEmpty(crmConnectionString))
				{
					crmService = new CrmServiceClient(crmConnectionString);
					if (!crmService.IsReady)
					{
						if (crmService.LastCrmException != null)
						{
							log.Error($"CRM Connection error: {crmService.LastCrmError}");
							throw crmService.LastCrmException;
						}

						throw new InvalidOperationException(crmService.LastCrmError);
					}
					CrmServiceReady = crmService.IsReady;


					#region Optimize Connection settings (Per microsft)

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
					log.Info("connection string should not be empty");
				}

			}
			catch (Exception ex)
			{
				string error = UtilityFunctions.GetDetailException(ex);
				log.Error($"Error:InitializeCrmService {error}");
			}
		}
		#endregion
	}
}
