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

            string queryIdentity = "Select @@Identity";
            OleDbCommand queryIdentityCommand = new OleDbCommand(queryIdentity, con);

            if (dataList==null)
            {
                return status;
            }

            try
            {
                con.Open();

               foreach(BillHeader b in dataList)
                {
                    OleDbCommand customerCommand = PrepareCustomerCommand(b);
                    
                    if(customerCommand.ExecuteNonQuery()==1)
                    {
                        b.Class_BillInfo.CustomerID = Convert.ToInt32(queryIdentityCommand.ExecuteScalar());

                        OleDbCommand billCommand = PrepareBillCommand(b);

                        if(billCommand.ExecuteNonQuery()==1)
                        {
                            status = true;
                        }
                        else
                        {
                            return status=false;
                        }

                    }
                    else
                    {
                        return status=false;
                    }
                }

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

        public List<BillHeader> GetData()
        {
            List<BillHeader> returnData = new List<BillHeader>();


            string getDataQuery = "SELECT c.CustomerId,c.CustomerName,c.AccountNumber,c.CustomerAddress,c.CustomerCity,c.CustomerState,c.CustomerZip,c.DateAdded   FROM Customer";
            try
            {
                con.Open();
                
            }
            catch (OleDbException e)
            {
                return returnData;
            }
            finally
            {
                con.Close();
            }
           

            return returnData;
        }

        private OleDbCommand PrepareCustomerCommand(BillHeader b)
        {
            string queryInsertIntoCustomer = "Insert into Customer(CustomerName,AccountNumber,CustomerAddress,CustomerCity,CustomerState,CustomerZip) VALUES(?,?,?,?,?,?)";

                OleDbCommand customerCommand = new OleDbCommand(queryInsertIntoCustomer, con);

                customerCommand.Parameters.AddWithValue("@CustomerName", b.Customer_Name);
                customerCommand.Parameters.AddWithValue("@AccountNumber", b.Account_No);
                customerCommand.Parameters.AddWithValue("@CustomerAddress", b.Class_AddressInformation.Mailing_Address_1);
                customerCommand.Parameters.AddWithValue("@CustomerCity", b.Class_AddressInformation.City);
                customerCommand.Parameters.AddWithValue("@CustomerState", b.Class_AddressInformation.State);
                customerCommand.Parameters.AddWithValue("@CustomerZip", b.Class_AddressInformation.Zip);

            
            return customerCommand;
        }

        private OleDbCommand PrepareBillCommand(BillHeader b)
        {

            string queryInsertIntoBill = "Insert into Bills(BillDate,BillAmount,FormatGUID,AccountBalance,DueDate,ServiceAddress,FirstEmailDate,SecondEmailDate,CustomerID) VALUES(?,?,?,?,?,?,?,?,?)";
            
                OleDbCommand billCommand = new OleDbCommand(queryInsertIntoBill, con);

                billCommand.Parameters.AddWithValue("@BillDate", b.Bill_Dt);
                billCommand.Parameters.AddWithValue("@BillAmount", b.Class_BillInfo.Bill_Amount);
                billCommand.Parameters.AddWithValue("@FormatGUID", b.Class_BillInfo.FormatGUID);
                billCommand.Parameters.AddWithValue("@AccountBalance", b.Class_BillInfo.Balance_Due);
                billCommand.Parameters.AddWithValue("@DueDate", b.Due_Dt);
                billCommand.Parameters.AddWithValue("@ServiceAddress", b.SERVICE_ADDRESS);
                billCommand.Parameters.AddWithValue("@FirstEmailDate", b.Class_BillInfo.FirstEmailDate);
                billCommand.Parameters.AddWithValue("@SecondEmailDate", b.Class_BillInfo.SecondEmailDate);
                billCommand.Parameters.AddWithValue("@CustomerID", b.Class_BillInfo.CustomerID);

            return billCommand;
        }

        #endregion

    }
}
