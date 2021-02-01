using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Technical1.Model
{
    class BillHeader
    {
        #region Constatns
        public readonly string INVOICE_FORMAT="xml";
        public readonly string SERVICE_ADDRESS = "1655 Ruben M Torres Blvd STE 101, Brownsville, TX 78526";
        #endregion

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

        #endregion

        #region Xmlproperties
        public readonly string NodeName = "BILL_HEADER";
        public readonly string ParentNodeName = "BILL_HEADER_Dataset";
        #endregion

        #region DataTable Properties
        public DateTime DateAdded { get; set; }
        #endregion

        #region ForRPT
        public string Service_Address { get; set; }
        public string InvoiceFormat{get;set;}
        public string Customer_GUID { get; set; }
        #endregion

        #region Constructor

        #endregion


    }
}
