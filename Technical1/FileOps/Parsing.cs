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
        #region PuesdoEnumerations

        #endregion
        #region Properties

        #endregion

        #region Constructors
        public Parsing()
        {
            
        }
        #endregion

        #region Parse
        public List<BillHeader> ParseXML(XmlNodeList nodeList)
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

                string line = string.Empty;
                List<string> rowData = new List<string>();
                while((line=reader.ReadLine())!=null)
                {
                    rowData.Add(line);

                    if(rowData.Count==2)
                    {
                       BillHeader b= ParseRPT(rowData);
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

        private BillHeader ParseRPT(List<string> rowData)
        {
            BillHeader bill = new BillHeader();
            List<string> valuedData = new List<string>();
            string StopAt = "|";

            if(rowData==null||rowData.Count!=2)
            {
                return bill;
            }

            foreach(string s in rowData)
            {
                string str = s;
                while(!string.IsNullOrEmpty(str))
                {
                    int StopChar = str.IndexOf(StopAt, StringComparison.Ordinal);
                    var _= str.Substring(0,StopChar);
                    valuedData.Add(_);

                    str = str.Substring(StopChar+1);


                }
                    
            }




            return bill;
        }

        #endregion
    }
}
