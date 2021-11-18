using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusToCE
{
	public class AppConfig
	{

		public static string SBConnection { get { return System.Environment.GetEnvironmentVariable("SBConnection"); } }

		public static string Topic { get { return System.Environment.GetEnvironmentVariable("Topic"); } }

		public static string TopicSub { get { return System.Environment.GetEnvironmentVariable("TopicSub"); } }

		public static string CrmConnection { get { return System.Environment.GetEnvironmentVariable("CRMConnection"); } }

	}
}
