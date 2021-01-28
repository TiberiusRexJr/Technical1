using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.Model
{
    class BillHeader
    {
        #region SubClasses
        public class Bill
        {
            public double BillAmount { get; set; }
            public double BalanceDue { get; set; }
            public DateTime BillRunDate { get; set; }
            public int BillRunSeq { get; set; }
            public int BillRunTm { get; set; }
            public string BillTp { get; set; }

        }
        public class AddressInformation 
        { 
            public string MailingAddress_1 { get; set; }
            public string MailingAddress_2 { get; set; }
            public string City { get; set; }
            public string State { get; set;}
            public int Zip { get; set;  }

        }

        #endregion

        #region Properties
        public int InvoiceNo { get; set; }
        public string AccountNo { get; set; }
        public string CustomerName { get; set; }
        public string CycleCd { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string AccountClass { get; set; }
        #endregion

        #region Constructor

        #endregion


    }
}
