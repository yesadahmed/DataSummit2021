using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace V1DurableNetCRMTemplate.Helper
{
	public class UtilityFunctions
	{

		public static string GetCrmFaultExceptionAny(Exception ex)
		{
			
			StringBuilder error = new StringBuilder();
			error.AppendLine($"Error: {ex.Message}");

			if (ex is FaultException<OrganizationServiceFault>)
			{
				var exp = ex as FaultException<OrganizationServiceFault>;
				if (exp.Detail != null)
				{
					if (!string.IsNullOrWhiteSpace(exp.Detail.TraceText))
					{
						error.AppendLine($"FaultTrace: {exp.Detail.TraceText}");
					}
					if (exp.Detail.ErrorDetails != null && exp.Detail.ErrorDetails.ContainsKey("ApiOriginalExceptionKey"))
					{
						error.AppendLine($"FaultText: {Convert.ToString(exp.Detail.ErrorDetails["ApiOriginalExceptionKey"])}");
					}

				}
			}
			//all other are same					
			if (ex.InnerException != null)
			{
				error.AppendLine($"InnerException: {ex.InnerException }");
			}

			if (!string.IsNullOrWhiteSpace(ex.StackTrace))
			{
				error.AppendLine($"StackTrace: {ex.StackTrace }");
			}
			return error.ToString();

		}

		public static string GetDetailException(Exception ex)
		{

			StringBuilder error = new StringBuilder();
			error.AppendLine($"Error: {ex.Message}");

		
		
			if (ex.InnerException != null)
			{
				error.AppendLine($"InnerException: {ex.InnerException }");
			}

			if (!string.IsNullOrWhiteSpace(ex.StackTrace))
			{
				error.AppendLine($"StackTrace: {ex.StackTrace }");
			}

			//if needed you can trun on
			/*var baseExpAny = ex.GetBaseException(); //better to have if any
			if(baseExpAny!=null)
			{
				error.AppendLine($"Base Exception: {baseExpAny.Message}");

				if (baseExpAny.InnerException != null)
				{
					error.AppendLine($"Base InnerException: {baseExpAny.InnerException }");
				}

				if (!string.IsNullOrWhiteSpace(baseExpAny.StackTrace))
				{
					error.AppendLine($"StackTrace: {baseExpAny.StackTrace }");
				}

			}*/

			return error.ToString();

		}
	}
}
