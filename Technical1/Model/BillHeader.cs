using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Technical1.Model
{
    class BillHeader
    {


        #region Properties
        public string InvoiceNo { get; set; }
        public string AccountNo { get; set; }
        public string CustomerName { get; set; }
        public string CycleCd { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string AccountClass { get; set; }
        public Bill BillInfo { get; set; }
        public AddressInformation AddressInfo { get; set; }
        public string InvoiceFormat{get;set;}


        #endregion

        #region Constructor

        #endregion


    }
}
