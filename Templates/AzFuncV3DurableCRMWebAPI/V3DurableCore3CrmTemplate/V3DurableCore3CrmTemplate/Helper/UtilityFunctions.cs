
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace V3DurableCore3CrmTemplate.Helper
{
	public class UtilityFunctions
	{

	
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
