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

        /// <summary>
        /// Parses the provided .xml file and extracts the data into a list of <type>BillHeader</type> objects. One object per each Parent Node.
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public List<BillHeader> ParseXMLData(XmlNodeList nodeList)
        {

            List<BillHeader> invoiceRecords = new List<BillHeader>();

            if (nodeList==null)
            {
                return invoiceRecords;
            }


            foreach (XmlNode node in nodeList)
            {
                BillHeader b = new BillHeader();

                #region Node Bill_Header DATA

                b.Invoice_No = node["Invoice_No"].InnerText;
                b.Account_No = node["Account_No"].InnerText;
                b.Customer_Name = node["Customer_Name"].InnerText;

                try
                {
                    b.Bill_Dt= DateTime.ParseExact(node["Bill_Dt"].InnerText, "MMM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch(Exception ex)
                {
                    if (ex is FormatException || ex is ArgumentNullException)
                    {
                        b.Bill_Dt = default;
                    }
                }

                try
                {
                    b.Due_Dt = DateTime.ParseExact(node["Due_Dt"].InnerText, "MMM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is ArgumentNullException)
                    {
                        b.Bill_Dt = default;
                    }
                }

                #endregion

                #region Node Bill Data

                XmlNodeList billNodechildren = node["Bill"].ChildNodes;

                b.Class_BillInfo.Bill_Amount = Convert.ToDecimal(billNodechildren.Item(0).InnerText);
                b.Class_BillInfo.Balance_Due = Convert.ToDecimal(billNodechildren.Item(1).InnerText);
              
                #endregion

                #region Node Address Data

                XmlNodeList addressNodeChildren = node["Address_Information"].ChildNodes;

                b.Class_AddressInformation.Mailing_Address_1= addressNodeChildren.Item(0).InnerText;
                b.Class_AddressInformation.Mailing_Address_2=addressNodeChildren.Item(1).InnerText;
                b.Class_AddressInformation.City =addressNodeChildren.Item(2).InnerText;
                b.Class_AddressInformation.State = addressNodeChildren.Item(3).InnerText;
                b.Class_AddressInformation.Zip=addressNodeChildren.Item(4).InnerText;


                #endregion

                invoiceRecords.Add(b);
            }

            return invoiceRecords;
            
        }

        /// <summary>
        /// Parses through a RPT formated Document and Extracts the data into a <type>List<BillHeader</type> List.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<BillHeader> ParseRPT(string filePath)
        {
            List<BillHeader> dataList = new List<BillHeader>();

            if(string.IsNullOrEmpty(filePath))
            {
                return dataList;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {

                    string header = reader.ReadLine();

                    //---->if does not contain("1~FR") aka header, return empty
                    if (string.IsNullOrEmpty(header))
                    {
                        return dataList;
                    }

                    string line = string.Empty;

                    //every string objeect in "rowData" is a Line Read from the file
                    List<string> rowData = new List<string>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        rowData.Add(line);

                        //When there are 2 items (AA,HH) in RowData Send off the pair to the helper function for data extraction
                        if (rowData.Count == 2)
                        {
                            BillHeader b = ParseRPTHelper(rowData);
                            if (b != null)
                            {

                                dataList.Add(b);
                            }

                            rowData.Clear();
                        }

                    }

                }
            }
            catch(Exception e)
            {
                if(e is IOException)
                {
                    return dataList = default;
                }
            }

                return dataList;
        }

        /// <summary>
        /// A Helper function for <see cref="ParseRPT(string)"/>
        /// Further Seperates the embeded data and returns a single <type>BillHeader</type> Object that containes the extracted
        /// data.
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
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

                        if (string.Equals(StopAt, stringLine[breakPoint + 1].ToString()))
                        {
                            stringLine = stringLine.Substring(breakPoint + 2);
                            valuedData.Add(string.Empty);
                        }
                        else
                        {
                            stringLine = stringLine.Substring(breakPoint + 1);
                        }
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

            #endregion

            //#region FilterData
            //string filterChar = "~";
            ////Filter and Remove UnFilled Key Values,e.g VV~,CC~

            //for (int i=0; i<=valuedData.Count; i++)
            //{
            //    //Find First Instance of "~"
            //    int breakpoint = valuedData[i].IndexOf(filterChar, StringComparison.Ordinal);

            //    string filteredString = string.Empty;

            //    //Get string up until "~"
            //    string key = valuedData[i].Substring(0, breakpoint);

            //    //If key's length is not greater then 3, it is a UnFilled Key (VV~,CC~) and will be removed
            //    if (!(key.Length > 3))
            //    {
            //        //Get the String Data Past the "~", ignoring e.g vv~,CC~,~AA
            //        filteredString = valuedData[i].Substring(breakpoint);
            //    }
            //    //any other case, the data before the "~" is greater then 3 thus it has been filled in,
            //    //copy the whole string.
            //    valuedData[i] = valuedData[i];
            //    //stick it in a container now.
            //}

            //#endregion

            #region CreateReturnData

            //AA line data
            bill.Account_No = valuedData[0];
            bill.Customer_Name = valuedData[1];
           bill.Class_AddressInformation.Mailing_Address_1 = valuedData[2];
            bill.Class_AddressInformation.Mailing_Address_2 = valuedData[3];
            bill.Class_AddressInformation.City = valuedData[4];
            bill.Class_AddressInformation.State=valuedData[5];
            bill.Class_AddressInformation.Zip = valuedData[6];

            //HH line data
            bill.FormatGUID = valuedData[7];
            bill.Invoice_No = valuedData[8];
            bill.Bill_Dt = DateTime.ParseExact(valuedData[9], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bill.Due_Dt = DateTime.ParseExact(valuedData[10], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bill.Class_BillInfo.Bill_Amount = Convert.ToDecimal(valuedData[11]);
            bill.Class_BillInfo.FirstEmailDate = DateTime.ParseExact(valuedData[12], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bill.Class_BillInfo.SecondEmailDate = DateTime.ParseExact(valuedData[13], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bill.Class_BillInfo.Balance_Due = Convert.ToDecimal(valuedData[14]);
            bill.DateAdded = DateTime.ParseExact(valuedData[15], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bill.Service_Address = valuedData[16];

            #endregion


            return bill;
        }

        #endregion
    }
}
