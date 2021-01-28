using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.Model
{
    class Bill
    {
        #region Properties
        public double Bill_Amount { get; set; }
        public double Balance_Due { get; set; }
        public DateTime Bill_Run_Dt { get; set; }
        public int Bill_Run_Seq { get; set; }
        public int Bill_Run_Tm { get; set; }
        public string Bill_Tp { get; set; }

        #endregion

        #region Xmlproperties
        public readonly string NodeName = "Bill";
        public readonly string ParentNodeName="BILL_HEADER";
        #endregion

    }
}
