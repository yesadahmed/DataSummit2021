using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace V3DurableCore3CrmTemplate.Model
{
	/// <summary>
	/// you can append as much you want
	/// </summary>
	public class AccountValue
	{

		public string accountid { get; set; }
		public string name { get; set; }
	}

	public class AccountModel
	{
		[JsonProperty("@odata.context")]
		public string OdataContext { get; set; }
		public List<AccountValue> value { get; set; }
	}

}
