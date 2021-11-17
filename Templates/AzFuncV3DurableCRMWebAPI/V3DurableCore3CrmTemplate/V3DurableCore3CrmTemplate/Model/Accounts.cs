using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace V3DurableCore3CrmTemplate.Model
{

	/// <summary>
	/// model fo rbasic account entity
	/// </summary>
	public class Accounts
	{
		[JsonProperty("@odata.context")]
		public string OdataContext { get; set; }
		public List<AccountValue> value { get; set; }
	}

	public class AccountValue
	{

		public string accountid { get; set; }
		public string name { get; set; }
	}


}
