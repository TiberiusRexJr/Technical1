using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Technical1.Model;

namespace Technical1.Database
{
    class Db
    {
        #region Variables
        readonly string dbFile="Billing.mdb";
        private OleDbConnection con = default;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Db()
        {
            SetDbConnection();
        }
        #endregion
        #region Methods
        private void SetDbConnection()
        {
            var _ = AppDomain.CurrentDomain.BaseDirectory;
            string dbRoot = _.Replace(@"\bin\Debug\", "/App_Data/");

            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();

            builder.Provider = "Microsoft.ACE.OLEDB.12.0";
            builder.DataSource = dbRoot + dbFile;
            builder.PersistSecurityInfo = true;

            con = new OleDbConnection(builder.ConnectionString);
        }
        
        public bool PutData(List<BillHeader> dataList)
        {
            bool status = false;

            if (dataList==null)
            {
                return status;
            }

            
            try
            {
                con.Open();
                
            }
            catch (OleDbException e)
            {
                return status;
            }
            finally
            {
                con.Close();
            }

           

            return status;

        }

        public bool GetData(List<BillHeader> dataList)
        {
            bool status = false;

            if (dataList == null)
            {
                return status;
            }

           
            try
            {
                con.Open();
                
            }
            catch (OleDbException e)
            {
                return status;
            }
            finally
            {
                con.Close();
            }
           

            return status;
        }

        private ValueTuple<List<OleDbCommand>, List<OleDbCommand>> PrepareCommands(List<BillHeader> dataList)
        {
            List<OleDbCommand> CustomerCommands = new List<OleDbCommand>();
            List<OleDbCommand> BillCommands = new List<OleDbCommand>();


            string queryInsertIntoBill = "Insert into Bills(BillDate,BillNumber,BillAmount,FormatGUID,AccountBalance,DueDate,ServiceAddress,FirstEmailDate,SecondEmailDate) VALUES(?,?,?,?,?,?,?,?,?)";

            string queryInsertIntoCustomer = "Insert into Customer(CustomerName,AccountNumber,CustomerAddress,CustomerCity,CustomerState,CustomerZip) VALUES(?,?,?,?,?,?)";

            foreach(BillHeader b in dataList)
            {
                OleDbCommand billCommand = new OleDbCommand(queryInsertIntoBill, con);

                billCommand.Parameters.AddWithValue("@BillDate", b.Bill_Dt);
                billCommand.Parameters.AddWithValue("@BillNumber", b.Invoice_No);
                billCommand.Parameters.AddWithValue("@BillAmount", b.Class_BillInfo.Bill_Amount);
                billCommand.Parameters.AddWithValue("@FormatGUID", b.Class_BillInfo.FormatGUID);
                billCommand.Parameters.AddWithValue("@AccountBalance", b.Class_BillInfo.Balance_Due);
                billCommand.Parameters.AddWithValue("@DueDate", b.Due_Dt);
                billCommand.Parameters.AddWithValue("@ServiceAddress", b.SERVICE_ADDRESS);
                billCommand.Parameters.AddWithValue("@FirstEmailDate", b.Class_BillInfo.FirstEmailDate);
                billCommand.Parameters.AddWithValue("@SecondEmailDate", b.Class_BillInfo.SecondEmailDate);

                BillCommands.Add(billCommand);

                OleDbCommand customerCommand = new OleDbCommand(queryInsertIntoCustomer, con);

                customerCommand.Parameters.AddWithValue("@CustomerName", b.Customer_Name);
                customerCommand.Parameters.AddWithValue("@AccountNumber", b.Account_No);
                customerCommand.Parameters.AddWithValue("@CustomerAddress", b.Class_AddressInformation.Mailing_Address_1);
                customerCommand.Parameters.AddWithValue("@CustomerCity", b.Class_AddressInformation.City);
                customerCommand.Parameters.AddWithValue("@CustomerState", b.Class_AddressInformation.State);
                customerCommand.Parameters.AddWithValue("@CustomerZip", b.Class_AddressInformation.Zip);

                CustomerCommands.Add(customerCommand);
            }


            return (CustomerCommands, BillCommands);
        }

        #endregion

    }
}
