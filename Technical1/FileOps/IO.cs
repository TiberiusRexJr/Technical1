using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technical1.Model;

namespace Technical1.FileOps
{
    class Io
    {
        #region Constants

         Dictionary<string, string> _FieldTable = new Dictionary<string, string>();

        private readonly string  RPT_FILENAME = "BillFile";
        private readonly string CSV_FILENAME = "BillingReport";
        

        #endregion

        #region Constructors
        public Io()
        {
            _FieldTable.Add("2", "8203ACC7-2094-43CC-8F7A-B8F19AA9BDA2");
            _FieldTable.Add("5", "Count of IH records");
            _FieldTable.Add("6", "SUM of BILL_AMOUNT values ");
            _FieldTable.Add("JJ", "8E2FEA69-5D77-4D0F-898E-DFA25677D19E");
            _FieldTable.Add("OO", "5 days after the current date");
            _FieldTable.Add("PP", "3 days before the Due Date (MM)");
        }
        #endregion

        #region ReadFiles
        /// <summary>
        /// Accepts a filePath and a node name. Attemps to use a <type>FileStream</type> to open the file
        /// to Aquire a <type>XMlNodeList</type> of the provided <paramref name="nodeName"/>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="nodeName"></param>
        /// <returns>XmlNodeList</returns>
        /// <returns>Null</returns>
        public XmlNodeList GetXMLData(string filePath, string nodeName)
        {
            XmlDocument xml = new XmlDocument();
            XmlNodeList Bill_Headers = default;

            if (String.IsNullOrEmpty(filePath) || String.IsNullOrEmpty(nodeName))
            {
                return Bill_Headers;
            }
            try
            {

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                xml.Load(fs);

                Bill_Headers = xml.GetElementsByTagName(nodeName);
            }
            catch(IOException e)
            {
                return Bill_Headers;
            }
         

            

            return Bill_Headers;

        }
        

        #endregion

        #region WriteFile

        /// <summary>
        /// Uses a <type>StreamWriter</type> instance to attempt to write the location provided by <paramref name="writeToDir"/>
        /// the data provided by <paramref name="writeData"/> and <paramref name="header"/> which are the Bill Line data and single
        /// Header of a RPT file.
        /// </summary>
        /// <param name="writeToDir"></param>
        /// <param name="header"></param>
        /// <param name="writeData"></param>
        /// <returns>bool</returns>
        public bool WriteToRPT(string writeToDir,string header,List<InvoiceBill> writeData)
        {
            bool status = false;

            if(string.IsNullOrEmpty(writeToDir)|string.IsNullOrEmpty(header)|writeData==null)
            {
                return status;    
            }

            string filePathComplete = writeToDir + "/" + this.RPT_FILENAME + "-" + DateTime.Today.ToString("mmddyyyy") + ".rpt";
            
            
            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePathComplete))
                {
                    outputFile.WriteLine(header);

                    foreach (InvoiceBill b in writeData)
                    {
                        outputFile.WriteLine(b.AddressLine);
                        outputFile.WriteLine(b.InvoiceLine);
                    }
                    

                    status = true;
                }

                    

            }
            catch (IOException e)
            {
                return status;
            }


            return status;
        }

        public bool WriteToCSV(string writeToDir,string header,List<string> writeData)
        {
            bool status = false;


            string filePathComplete = writeToDir + "/" + this.CSV_FILENAME + ".txt";

           

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePathComplete))
                {
                    outputFile.WriteLine(header);

                    foreach (string s in writeData)
                    {
                        outputFile.WriteLine(s);
                    }


                    status = true;
                }

            }
            catch (IOException e)
            {
                return status;
            }
            return status;
        }   

        #endregion


        #region Create Write Data

        

        #region Create Write Data CSV

        /// <summary>
        /// Creates the data to be written to a CSV File by passing the provided <paramref name="dataList"/> to helper functions
        /// <c>CreateLineDataCSV</c> and <c>CreateInvoiceHeaderCSV</c> respectivley. It then returns a Tuple containing both pairs of data
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns>Tuple</returns>
        public (string Header,List<string>LineData) CreateWriteDataCSV(List<BillHeader> dataList)
        {
            string Header = string.Empty;
            List<string> LineData = default;

            Header = CreateInvoiceHeaderCSV(dataList);

            if(Header==null)
            {
                return (Header, LineData);
            }

            LineData = CreateLineDataCSV(dataList);

            if (LineData == null)
            {
                return (Header, LineData);
            }
            
            return (Header, LineData);
        }

        /// <summary>
        /// A Helper Function to <see cref=">CreateWriteDataCSV"/> Uses the data provided by <paramref name="headerList"/> to 
        /// format a CSV Style line for every <type>BillHeader</type> Object in the List.
        /// </summary>
        /// <param name="headerList"></param>
        /// <returns>List<string></returns>
        private List<string> CreateLineDataCSV(List<BillHeader> headerList)
        {
            List<string> lineData = new List<string>();

            foreach(BillHeader b in headerList)
            {

               string line = b.Class_BillInfo.CustomerID.ToString() + "," + b.Customer_Name + "," + b.Account_No + "," + b.Class_AddressInformation.Mailing_Address_1 + "," + b.Class_AddressInformation.City + "," + b.Class_AddressInformation.State + "," + b.Class_AddressInformation.Zip + "," + b.Class_BillInfo.ID.ToString() + "," + b.Bill_Dt.ToString("MM/dd/yyyy") + "," + b.Class_BillInfo.BillNumber + "," + b.Class_BillInfo.Balance_Due.ToString() + "," + b.Due_Dt.ToString("MM/dd/yyyy") + "," + b.Class_BillInfo.Bill_Amount.ToString() + "," + b.Class_BillInfo.FormatGUID + "," + b.DateAdded.ToString("MM/dd/yyyy");

                lineData.Add(line);
            }

            return lineData;
        }

        /// <summary>
        /// A Helper Function
        /// Creates a a CSV formateed Header line for a CSV file.
        /// uses <see cref=">GetHeaderStatistics"/> to preform Calculations and then uses the returned values to
        /// create a CSV formated Header Line.
        /// </summary>
        /// <param name="billList"></param>
        /// <returns>String</returns>
        private String CreateInvoiceHeaderCSV(List<BillHeader> billList)
        {
            string line = string.Empty;

            var data = GetHeaderStatistics(billList);

            line = _FieldTable["2"] + "," + data.CurrentDate + "," + data.RecordCount.ToString() + "," + data.RecordInvoiceTotal.ToString();

            return line;
        }
        #endregion

        #region Create Write Data RPT
        /// <summary>
        /// Uses a list of <type>BillHeader</type> Objects to create the data that will be writtne to a RPT style
        /// document. Uses helper functions <see cref="CreateInvoiceRecordLine_Address"/> and <see cref="CreateInvoiceRecordLine_Invoice"/>
        /// and <see cref="CreateInvoiceHeaderRPT"/> to create the entirity of the data to be written.
        /// </summary>
        /// <param name="billHeaders"></param>
        /// <returns></returns>
        public (string Header, List<InvoiceBill>WriteData) CreateWriteDataRPT(List<BillHeader> billHeaders)
        {
            string Header = string.Empty;
            List<InvoiceBill> WriteData = new List<InvoiceBill>();

            Header = CreateInvoiceHeaderRPT(billHeaders);

            foreach(BillHeader bh in billHeaders)
            {
                InvoiceBill ib = new InvoiceBill();
                ib.AddressLine = CreateInvoiceRecordLine_Address(bh);
                ib.InvoiceLine = CreateInvoiceRecordLine_Invoice(bh);
                WriteData.Add(ib);
            }

            return (Header, WriteData);
        }

        /// <summary>
        /// Creates a "HH~" formated line using the <param name="header"></param> data.
        /// </summary>
        /// <param name="header"></param>
        /// <returns>String</returns>
        private string CreateInvoiceRecordLine_Address(BillHeader header)
        {
            
            string line = string.Empty;

            line = header.Account_No+"|"+header.Customer_Name+"|"+ header.Class_AddressInformation.Mailing_Address_1 + "|"+ header.Class_AddressInformation.Mailing_Address_2 + "|"+ header.Class_AddressInformation.City + "|"+ header.Class_AddressInformation.State + "|"+ header.Class_AddressInformation.Zip + "";

            return line;
        }
        /// <summary>
        /// Creates a "AA~" formated line using the <paramref name="header"/> data.
        /// </summary>
        /// <param name="header"></param>
        /// <returns>String</returns>

        private string CreateInvoiceRecordLine_Invoice(BillHeader header)
        {
            string line = string.Empty;

          


            string Current_Date = DateTime.Today.ToString("MM/dd/yyyy");

            string First_Notification_Date = DateTime.Today.AddDays(5).ToString("MM/dd/yyyy");

            string Second_Notifaction_Date = header.Due_Dt.AddDays(-3).ToString("MM/dd/yyyy");

            line = _FieldTable["JJ"]+"|"+header.Invoice_No+"|"+header.Bill_Dt.ToString("MM/dd/yyyy")+"|"+header.Due_Dt.ToString("MM/dd/yyyy")+"|"+header.Class_BillInfo.Bill_Amount+"|"+First_Notification_Date+"|"+Second_Notifaction_Date+"|"+header.Class_BillInfo.Balance_Due+"|"+Current_Date+"|"+header.Service_Address+"";


            return line;
        }

        /// <summary>
        /// Creates a header for a RPT formated document
        /// </summary>
        /// <param name="billList"></param>
        /// <returns></returns>
        private string CreateInvoiceHeaderRPT(List<BillHeader> billList)
        {
            string line = string.Empty;

            var data=GetHeaderStatistics(billList);
           
            line = _FieldTable["2"] + "|" + data.CurrentDate + "|" + data.RecordCount.ToString() + "|" + data.RecordInvoiceTotal.ToString() + "";

            return line;
        }

        #endregion

        #region Statistics
        /// <summary>
        /// Calculates the data that will be used for both a CSV and RPT header.
        /// </summary>
        /// <param name="billList"></param>
        /// <returns></returns>
          (int RecordCount,decimal RecordInvoiceTotal,string CurrentDate)  GetHeaderStatistics(List<BillHeader> billList)
        {

            string CurrentDate = DateTime.Today.ToString("MM/dd/yyyy");

            int RecordCount = billList.Count;

            decimal RecordInvoiceTotal = default;

            foreach (BillHeader b in billList)
            {
                RecordInvoiceTotal += b.Class_BillInfo.Bill_Amount;
            }
            return (RecordCount,RecordInvoiceTotal, CurrentDate);
        }
        #endregion

#endregion

    }


}
