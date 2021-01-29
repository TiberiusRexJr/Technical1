using System;
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
        public List<BillHeader> ParseXML(XmlNodeList nodeList,BillHeader bill)
        {

            List<BillHeader> invoiceRecords = new List<BillHeader>();

            if (nodeList==null || bill==null)
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
                b.Class_BillInfo.Bill_Amount = Convert.ToDouble(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Amount))?.InnerText);
                b.Class_BillInfo.Balance_Due = Convert.ToDouble(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Balance_Due))?.InnerText);
                b.Class_BillInfo.Bill_Run_Dt = DateTime.ParseExact((node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Balance_Due))?.InnerText),"MM/DD/YYYY",null);
                b.Class_BillInfo.Bill_Run_Seq = Convert.ToInt32(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Run_Seq))?.InnerText);
                b.Class_BillInfo.Bill_Run_Tm= Convert.ToInt32(node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Run_Tm))?.InnerText);
                b.Class_BillInfo.Bill_Tp =node.SelectSingleNode("//" + nameof(b.Class_BillInfo.NodeName) + "/" + nameof(b.Class_BillInfo.Bill_Tp))?.InnerText;

                #endregion

                #region Node Address Data
                b.Class_AddressInformation.Mailing_Address_1= node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.Mailing_Address_1))?.InnerText;
                b.Class_AddressInformation.Mailing_Address_2= node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.Mailing_Address_2))?.InnerText;
                b.Class_AddressInformation.City = node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.City))?.InnerText;
                b.Class_AddressInformation.Zip= Convert.ToInt32(node.SelectSingleNode("//" + nameof(b.Class_AddressInformation.NodeName) + "/" + nameof(b.Class_AddressInformation.Zip))?.InnerText);


                #endregion

                invoiceRecords.Add(b);
            }

            return invoiceRecords;
            
        }

        #endregion
    }
}
