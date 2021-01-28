using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technical1.Model;
namespace FileOps.IO
{
    class IO
    {
        #region Constants

        static Dictionary<string, string> _FieldTable = new Dictionary<string, string>();

        #endregion

        #region Constructors
        public IO()
        {
            _FieldTable.Add("2", "8203ACC7-2094-43CC-8F7A-B8F19AA9BDA2");
            _FieldTable.Add("5", "Count of IH records");
            _FieldTable.Add("6", "SUM of BILL_AMOUNT values ");
            _FieldTable.Add("JJ", "8E2FEA69-5D77-4D0F-898E-DFA25677D19E");
            _FieldTable.Add("OO", "5 days after the current date");
            _FieldTable.Add("PP", "3 days before the Due Date (MM)");
        }
        #endregion

        #region ReadFile
        public void ReadXML()
        {


        }
        public void ReadRPT()
        {


        }

        #endregion

        #region WriteFile

        public void WriteRPT()
        {

        }

        #endregion

        #region CreateHeaders

        public string CreateInvoiceHeader(int invoiceRecordCount, int invoiceRecordTotalAmount)
        {
            string line = string.Empty;

            string Customer_GUID = Guid.NewGuid().ToString();
            string Current_Date = DateTime.Today.ToString("MM/dd/yyyy");
            
            line += _FieldTable["2"] +"~"+Customer_GUID+ "|";
            line += Current_Date + "|";
            line += _FieldTable["5"] + "~" + invoiceRecordCount.ToString() + "|";
            line += _FieldTable["6"] + "~" + invoiceRecordTotalAmount.ToString();

            return line;
        }

        #endregion

        #region CreateLines
        public string CreateInvoiceRecordLine_Address(BillHeader header)
        {
            
            string line = string.Empty;
            line += header.AccountNo+"|";
            line += header.CustomerName + "|";
            line += header.AddressInfo.MailingAddress_1 + "|";
            line += header.AddressInfo.MailingAddress_2 + "|";
            line += header.AddressInfo.City + "|";
            line += header.AddressInfo.State + "|";
            line += header.AddressInfo.Zip + "|";
            return line;
        }
        public string CreateInvoiceRecordLine_Invoice(BillHeader header)
        {
            string line = string.Empty;

            DateTime dateTime = new DateTime();

            string Current_Date = DateTime.Today.ToString("MM/dd/yyyy");

            string First_Notification_Date = dateTime.AddDays(5).ToString("MM/dd/yyyy");

            string Second_Notifaction_Date = header.DueDate.AddDays(-3).ToString("MM/dd/yyyy");

            line += _FieldTable["JJ"]+ "~" + header.InvoiceFormat+"|";
            line += header.InvoiceNo + "|";
            line += header.BillDate + "|";
            line += header.DueDate + "|";
            line += header.BillInfo.BillAmount + "|";
            line += _FieldTable["OO"] + "~" + First_Notification_Date + "|";
            line += _FieldTable["PP"] + "~" + Second_Notifaction_Date + "|";
            line += header.BillInfo.BalanceDue + "|";
            line += Current_Date;
            line += header.AddressInfo;

            return line;
        }
        #endregion
    }
}
