﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace V3DurableCore3CrmTemplate.Model
{
	/// <summary>
	/// you can append as much you want
	/// </summary>


	public class AccountModel // this could be any model 
	{

		public string PrimaryEntityId { get; set; }
		public string MessageName { get; set; }
		public string PrimaryEntityName { get; set; }
		public bool IsCreateMessage { get; set; }
		public bool IsUpdateMessage { get; set; }

		public bool IsDeleteMessage { get; set; }

		//PRE and PSOT image properties

		// (SAMPLE) propoerties just to give you idea how to parse  and make decsions on message types
		#region sample properties

		public string col_accountid2 { get; set; }
		public string col_accountid2Name { get; set; }
		public string title { get; set; }

		public int statuscode { get; set; }


		public string precol_accountid2 { get; set; }
		public string precol_accountid2Name { get; set; }

		public int prestatuscode { get; set; }

		public string pretitle { get; set; }

		#endregion

	}
}
