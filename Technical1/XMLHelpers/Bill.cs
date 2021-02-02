using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.XMLHelpers
{
    class Bill
    {
        #region Constructor
        private Bill(string value) { Value = value; }
        #endregion
        #region Variables
        public string Value { get; set; }
        #endregion

        #region Properties
        public static Bill Success { get { return new Bill("Success"); } }
        public static Bill Failure { get { return new Bill("Failure"); } }
        public static Bill CallToAction { get { return new Bill("CallToAction"); } }
        public static Bill Status { get { return new Bill("Status"); } }

        #endregion
    }
}
