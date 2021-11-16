using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1DurableNetCRMTemplate.Helper
{
	public class AppConfig
	{
		
		public static string CRMConnection { get { return System.Environment.GetEnvironmentVariable("CRMConnection"); } }
		//Define more here
		
	}
}
