using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technical1.Model;

namespace Technical1.FileOps
{
    class FileOpsUtil
    {
        public List<BillHeader> FillInExtraData(List<BillHeader> dataList)
        {
            foreach(BillHeader b in dataList)
            {
               
                b.Service_Address = b.SERVICE_ADDRESS;
                Guid guid = new Guid();
                b.Customer_GUID = guid.ToString();

            }

            return dataList;
        }
    }
}
