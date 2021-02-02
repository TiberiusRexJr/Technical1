using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.XMLHelpers
{
    class BILL_HEADER
    {
        #region Constructor
        private BILL_HEADER(string value) { Value = value; }
        #endregion
        #region Variables
        public string Value { get; set; }
        #endregion

        #region Properties
        public static BILL_HEADER Bill_Header { get { return new BILL_HEADER("Bill_Header"); } }
        public static BILL_HEADER Invoice_No { get { return new BILL_HEADER("Invoice_No"); } }
        public static BILL_HEADER Customer_Name { get { return new BILL_HEADER("Customer_Name"); } }
        public static BILL_HEADER Cycle_Cd { get { return new BILL_HEADER("Cycle_Cd"); } }

        public static BILL_HEADER Bill_Dt { get { return new BILL_HEADER("Bill_Dt"); } }
        public static BILL_HEADER Due_Dt { get { return new BILL_HEADER("Due_Dt"); } }
        public static BILL_HEADER Account_Class { get { return new BILL_HEADER("Account_Class"); } }



        #endregion
    }
}
