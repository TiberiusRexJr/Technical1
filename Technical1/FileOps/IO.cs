﻿using System;
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
        public void ReadRPT()
        {


        }

        #endregion

        #region WriteFile

        public bool WriteToRPT(string writeToDir,string header,List<InvoiceBill> writeData)
        {
            bool status = false;

            if(string.IsNullOrEmpty(writeToDir)|string.IsNullOrEmpty(header)|writeData==null)
            {
                return status;    
            }

            string filePathComplete = writeToDir + "/" + this.RPT_FILENAME + "-" + DateTime.Today.ToString("mm/dd/yyyy") + ".rpt";
            
            
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

        private string CreateInvoiceHeader(List<BillHeader> billList)
        {
            string line = string.Empty;

            string Customer_GUID = Guid.NewGuid().ToString();
            string Current_Date = DateTime.Today.ToString("MM/dd/yyyy");

            int invoiceRecordCount = billList.Count;
            decimal invoiceRecordTotalAmount = default;

            foreach (BillHeader b in billList)
            {
                invoiceRecordTotalAmount += b.Class_BillInfo.Bill_Amount;
            }

            line += _FieldTable["2"] + "~" + Customer_GUID + "|";
            line += Current_Date + "|";
            line += _FieldTable["5"] + "~" + invoiceRecordCount.ToString() + "|";
            line += _FieldTable["6"] + "~" + invoiceRecordTotalAmount.ToString();

            return line;
        }

        #region Create Write Data CSV

        public (string Header,List<string>LineData) CreateWriteDataCSV(List<BillHeader> dataList)
        {
            string Header = string.Empty;
            List<string> LineData = default;

            Header = CreateInvoiceHeader(dataList);

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
        private List<string> CreateLineDataCSV(List<BillHeader> headerList)
        {
            List<string> lineData = new List<string>();

            foreach(BillHeader b in headerList)
            {

               string line = b.Class_BillInfo.CustomerID.ToString() + "," + b.Customer_Name + "," + b.Account_No + "," + b.Class_AddressInformation.Mailing_Address_1 + "," + b.Class_AddressInformation.City + "," + b.Class_AddressInformation.State + "," + b.Class_AddressInformation.Zip + "," + b.Class_BillInfo.ID.ToString() + "," + b.Bill_Dt.ToString() + "," + b.Class_BillInfo.BillNumber + "," + b.Class_BillInfo.Balance_Due.ToString() + "," + b.Due_Dt.ToString() + "," + b.Class_BillInfo.Bill_Amount.ToString() + "," + b.Class_BillInfo.FormatGUID + "," + b.DateAdded.ToString();

                lineData.Add(line);
            }

            return lineData;
        }
        #endregion

        #region Create Write Data RPT
        public (string Header, List<InvoiceBill>WriteData) CreateWriteDataRPT(List<BillHeader> billHeaders)
        {
            string Header = string.Empty;
            List<InvoiceBill> WriteData = default;

            Header = CreateInvoiceHeader(billHeaders);

            foreach(BillHeader bh in billHeaders)
            {
                InvoiceBill ib = new InvoiceBill();
                ib.AddressLine = CreateInvoiceRecordLine_Address(bh);
                ib.InvoiceLine = CreateInvoiceRecordLine_Invoice(bh);
                WriteData.Add(ib);
            }

            return (Header, WriteData);
        }

        private string CreateInvoiceRecordLine_Address(BillHeader header)
        {
            
            string line = string.Empty;
            line += header.Account_No + "|";
            line += header.Customer_Name + "|";
            line += header.Class_AddressInformation.Mailing_Address_1 + "|";
            line += header.Class_AddressInformation.Mailing_Address_2 + "|";
            line += header.Class_AddressInformation.City + "|";
            line += header.Class_AddressInformation.State + "|";
            line += header.Class_AddressInformation.Zip + "|";

            return line;
        }

        private string CreateInvoiceRecordLine_Invoice(BillHeader header)
        {
            string line = string.Empty;

            DateTime dateTime = new DateTime();

            string Current_Date = DateTime.Today.ToString("MM/dd/yyyy");

            string First_Notification_Date = dateTime.AddDays(5).ToString("MM/dd/yyyy");

            string Second_Notifaction_Date = header.Due_Dt.AddDays(-3).ToString("MM/dd/yyyy");

            line += _FieldTable["JJ"] + "~" + header.InvoiceFormat + "|";
            line += header.Invoice_No + "|";
            line += header.Bill_Dt + "|";
            line += header.Due_Dt + "|";
            line += header.Class_BillInfo.Bill_Amount + "|";
            line += _FieldTable["OO"] + "~" + First_Notification_Date + "|";
            line += _FieldTable["PP"] + "~" + Second_Notifaction_Date + "|";
            line += header.Class_BillInfo.Balance_Due + "|";
            line += Current_Date;
            line += header.SERVICE_ADDRESS;

            return line;
        }

        #endregion

        #endregion

    }


}
