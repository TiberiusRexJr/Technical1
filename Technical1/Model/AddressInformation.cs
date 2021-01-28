using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.Model
{
    class AddressInformation
    {
        public string Mailing_Address_1 { get; set; }
        public string Mailing_Address_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }

        #region Xmlproperties
        public readonly string NodeName = "Address_Information";
        public readonly string ParentNodeName = "BILL_HEADER";
        #endregion
    }
}
