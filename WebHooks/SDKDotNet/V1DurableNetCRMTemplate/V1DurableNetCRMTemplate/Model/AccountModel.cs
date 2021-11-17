using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1DurableNetCRMTemplate.Model
{
	/// <summary>
	///  //Define your own attributes in model
	///  This is just an example
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
		public string new_proimageurl { get; set; }
		public double new_productprice { get; set; }
		public string new_productname { get; set; }
		public string ownerid { get; set; }
		public string ownername { get; set; }
		public int new_products { get; set; }


		public string prenew_proimageurl { get; set; }
		public double prenew_productprice { get; set; }
		public string prenew_productname { get; set; }

		public string preownerid { get; set; }
		public int prenew_products { get; set; }
		public string preownername { get; set; }

	}
}
