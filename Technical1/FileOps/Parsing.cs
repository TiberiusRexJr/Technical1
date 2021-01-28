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
        public ValueTuple<string,List<string[]>> ParseXML(XmlNodeList nodeList,BillHeader bill)
        {
            string InvoiceHeader = string.Empty;

            string InvoiceRowBill = string.Empty;
            string InvoiceRowAddress = string.Empty;

            List<string[]> InvoiceRecordRows = new List<string[]>();

            foreach(XmlNode node in nodeList)
            {

            }

            return (InvoiceHeader, InvoiceRecordRows);
            
        }

        #endregion
    }
}
