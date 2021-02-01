using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technical1.Model;
namespace Technical1.FileOps
{
    class Parsing
    {
        #region Constants


        #endregion


        #region Properties

        #endregion

        #region Constructors
        public Parsing()
        {
            
        }
        #endregion

        #region Parse
        public List<BillHeader> ParseXMLData(XmlNodeList nodeList)
        {

            List<BillHeader> invoiceRecords = new List<BillHeader>();

            if (nodeList==null)
            {
                return invoiceRecords;
            }


            foreach(XmlNode node in nodeList)
            {
                BillHeader b = new BillHeader();

                #region Node Bill_Header DATA

                b.Invoice_No = node[nameof(b.Invoice_No)].InnerText;
                b.Account_No = node[nameof(b.Account_No)].InnerText;
                b.Customer_Name = node[nameof(b.Customer_Name)].InnerText;
                b.Bill_Dt = DateTime.ParseExact(node[nameof(b.Bill_Dt)].InnerText, "MM/DD/YYYY", null);
                b.Due_Dt = DateTime.ParseExact(node[nameof(b.Due_Dt)].InnerText, "MM/DD/YYYY", null);

                #endregion

                #region Node Bill Data
                b.Class_BillInfo.Bill_Amount = Convert.ToDecimal(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Amount))?.InnerText);
                b.Class_BillInfo.Balance_Due = Convert.ToDecimal(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Balance_Due))?.InnerText);
                b.Class_BillInfo.Bill_Run_Dt = DateTime.ParseExact((node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Balance_Due))?.InnerText),"MM/DD/YYYY",null);
                b.Class_BillInfo.Bill_Run_Seq = Convert.ToInt32(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Run_Seq))?.InnerText);
                b.Class_BillInfo.Bill_Run_Tm= Convert.ToInt32(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Run_Tm))?.InnerText);
                b.Class_BillInfo.Bill_Tp =node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Tp))?.InnerText;

                #endregion

                #region Node Address Data
                b.Class_AddressInformation.Mailing_Address_1= node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.Mailing_Address_1))?.InnerText;
                b.Class_AddressInformation.Mailing_Address_2= node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.Mailing_Address_2))?.InnerText;
                b.Class_AddressInformation.City = node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.City))?.InnerText;
                b.Class_AddressInformation.Zip=node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.Zip))?.InnerText;


                #endregion

                invoiceRecords.Add(b);
            }

            return invoiceRecords;
            
        }

        public List<BillHeader> ParseRPT(string filePath)
        {
            List<BillHeader> dataList = new List<BillHeader>();

            if(string.IsNullOrEmpty(filePath))
            {
                return dataList;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {

                string header=reader.ReadLine();

                //---->if does not contain("1~FR") aka header, return empty
                if(header.IndexOf("1~FR",0)==-1)
                {
                    return dataList;
                }

                string line = string.Empty;

                //every string objeect in "rowData" is a Line Read from the file
                List<string> rowData = new List<string>();

                while((line=reader.ReadLine())!=null)
                {
                    rowData.Add(line);

                    //When there are 2 items (AA,HH) in RowData Send off the pair to the helper function for data extraction
                    if(rowData.Count==2)
                    {
                       BillHeader b= ParseRPTHelper(rowData);
                        if(b!=null)
                        {

                            dataList.Add(b);
                        }

                        rowData.Clear();
                    }

                }

            }

                return dataList;
        }

        private BillHeader ParseRPTHelper(List<string> rowData)
        {
            BillHeader bill = new BillHeader();
            List<string> valuedData = new List<string>();

            
            string StopAt = "|";

            if(rowData==null||rowData.Count!=2)
            {
                return bill;
            }

            #region SeperateData
            foreach (string s in rowData)
            {
                string stringLine = s;
                
                while(!string.IsNullOrEmpty(stringLine))
                {
                    //find location of first "|" on each iteration of While
                    int breakPoint = stringLine.IndexOf(StopAt, StringComparison.Ordinal);
                    string valuedStringData= string.Empty;

                    //case "|" is found then
                    if(breakPoint != -1)
                    {
                        //Get The Data Up until the "|"
                        valuedStringData = stringLine.Substring(0, breakPoint);

                        valuedData.Add(valuedStringData);

                        //re-asign the StringLine, "cutting out"  "|" Character just encountered
                        stringLine = stringLine.Substring(breakPoint + 1);
                    }
                    //case -1, "|" not found, End of Line
                    else
                    {
                        //add whats left of string break out of While loop continue iteration of Foreach
                        valuedData.Add(stringLine);
                        break;
                    }
                }
            }

            string filterChar = "~";
            #endregion

            #region FilterData
            //Filter and Remove UnFilled Key Values,e.g VV~,CC~

            for (int i=0; i<=valuedData.Count; i++)
            {
                //Find First Instance of "~"
                int breakpoint = valuedData[i].IndexOf(filterChar, StringComparison.Ordinal);

                string filteredString = string.Empty;

                //Get string up until "~"
                string key = valuedData[i].Substring(0, breakpoint);

                //If key's length is not greater then 3, it is a UnFilled Key (VV~,CC~) and will be removed
                if (!(key.Length > 3))
                {
                    //Get the String Data Past the "~", ignoring e.g vv~,CC~,~AA
                    filteredString = valuedData[i].Substring(breakpoint);
                }
                //any other case, the data before the "~" is greater then 3 thus it has been filled in,
                //copy the whole string.
                valuedData[i] = valuedData[i];
                //stick it in a container now.
            }

            #endregion

            #region CreateReturnData

            //AA line data
            bill.Account_No = valuedData[1];
            bill.Customer_Name = valuedData[2];
           bill.Class_AddressInformation.Mailing_Address_1 = valuedData[3];
            bill.Class_AddressInformation.Mailing_Address_2 = valuedData[4];
            bill.Class_AddressInformation.City = valuedData[5];
            bill.Class_AddressInformation.State=valuedData[6];
            bill.Class_AddressInformation.Zip = valuedData[7];

            //HH line data
            bill.InvoiceFormat = valuedData[11];
            bill.Invoice_No = valuedData[12];
            bill.Bill_Dt = DateTime.ParseExact(valuedData[13], "MM/DD/YYYY", null);
            bill.Due_Dt= DateTime.ParseExact(valuedData[14], "MM/DD/YYYY", null);
            bill.Class_BillInfo.Bill_Amount = Convert.ToDecimal(valuedData[15]);
            bill.Class_BillInfo.FirstEmailDate= DateTime.ParseExact(valuedData[16], "MM/DD/YYYY", null);
            bill.Class_BillInfo.SecondEmailDate= DateTime.ParseExact(valuedData[17], "MM/DD/YYYY", null);
            bill.Class_BillInfo.Balance_Due = Convert.ToDecimal(valuedData[18]);
            bill.DateAdded= DateTime.ParseExact(valuedData[19], "MM/DD/YYYY", null);
            bill.SERVICE_ADDRESS = valuedData[20];

            #endregion


            return bill;
        }

        #endregion
    }
}
