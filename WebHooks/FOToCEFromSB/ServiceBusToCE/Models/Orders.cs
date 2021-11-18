using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusToCE.Models
{
	public class Orders
	{
		public string BusinessEventId { get; set; }
		public long ControlNumber { get; set; }
		public string EventId { get; set; }
		public DateTime EventTime { get; set; }
		public string LegalEntity { get; set; }
		public int MajorVersion { get; set; }
		public int MinorVersion { get; set; }
		public string PurchaseJournal { get; set; }
		public DateTime PurchaseOrderDate { get; set; }
		public string PurchaseOrderNumber { get; set; }
		public string PurchaseType { get; set; }
		public decimal TransactionCurrencyAmount { get; set; }
		public string TransactionCurrencyCode { get; set; }
		public string VendorAccount { get; set; }

		//public string BusinessEventId { get; set; }

		//public int ControlNumber { get; set; }//

		//public string EventId { get; set; }

		//public DateTime EventTime { get; set; }

		//public DateTime PurchaseOrderDate { get; set; }//
		//public decimal TransactionCurrencyAmount { get; set; }//totalamount
		//public string VendorAccount { get; set; }///customer
		//public string PurchaseType { get; set; }

		//public string PurchaseOrderNumber { get; set; }//ordernumber

		//public string PurchaseJournal { get; set; }//name

		//"BusinessEventId": "PurchaseOrderConfirmedBusinessEvent",
		//"ControlNumber": 5637144576,
		//"EventId": "55A81094-A14E-46A6-A12C-07ED5C5990C6",
		//"EventTime": "/Date(1637246655000)/",
		//"LegalEntity": "brmf",
		//"MajorVersion": 0,
		//"MinorVersion": 0,
		//"PurchaseJournal": "BRMF-000018-2",
		//"PurchaseOrderDate": "/Date(1637193600000)/",
		//"PurchaseOrderNumber": "BRMF-000018",
		//"PurchaseType": "Purch",
		//"TransactionCurrencyAmount": 90055.6,
		//"TransactionCurrencyCode": "BRL",
		//"VendorAccount": "BRMF-000004"
	}

}
