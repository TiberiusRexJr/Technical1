using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.Model
{
    class Bill
    {
        public double BillAmount { get; set; }
        public double BalanceDue { get; set; }
        public DateTime BillRunDate { get; set; }
        public int BillRunSeq { get; set; }
        public int BillRunTm { get; set; }
        public string BillTp { get; set; }
    }
}
