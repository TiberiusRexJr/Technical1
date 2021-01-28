using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Technical1.Model
{
    class BillHeader
    {


        #region Properties
        public string Invoice_No { get; set; }
        public string Account_No { get; set; }
        public string Customer_Name { get; set; }
        public string Cycle_Cd { get; set; }
        public DateTime Bill_Dt { get; set; }
        public DateTime Due_Dt { get; set; }
        public string Account_Class { get; set; }
        public Bill Class_BillInfo { get; set; }
        public AddressInformation Class_AddressInformation { get; set; }
        public string InvoiceFormat{get;set;}
        public string SERVICE_ADDRESS { get; set; }

        #endregion

        #region Xmlproperties
        public readonly string NodeName = "BILL_HEADER";
        public readonly string ParentNodeName = "BILL_HEADER_Dataset";
        #endregion

        #region Constructor

        #endregion


    }
}
