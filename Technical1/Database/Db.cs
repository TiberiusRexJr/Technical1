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


            string getDataQuery = "SELECT c.CustomerId,c.CustomerName,c.AccountNumber,c.CustomerAddress,c.CustomerCity,c.CustomerState,c.CustomerZip,b.ID,b.BillDate,b.BillNumber,b.AccountBalance,b.DueDate,b.BillAmount,b.FormatGUID,c.DateAdded FROM Customer AS c INNER JOIN Bills as b ON c.ID=b.CustomerID";

            OleDbCommand getDataCommand = new OleDbCommand(getDataQuery, con);

            try
            {
                con.Open();
                OleDbDataReader reader = getDataCommand.ExecuteReader();

                if(reader.HasRows)
                {
                   


                    BillHeader b = new BillHeader();

                    b.Class_BillInfo.CustomerID = Convert.ToInt32(reader["CustomerID"].ToString()) ;
                    b.Customer_Name = reader["CustomerName"].ToString();
                    b.Account_No = reader["AccountNumber"].ToString();
                    b.Class_AddressInformation.Mailing_Address_1 = reader["CustomerAddress"].ToString();
                    b.Class_AddressInformation.City = reader["CustomerCity"].ToString();
                    b.Class_AddressInformation.State = reader["CustomerState"].ToString();
                    b.Class_AddressInformation.Zip = reader["CustomerZip"].ToString();
                    b.Class_BillInfo.ID = Convert.ToInt32(reader["ID"].ToString());
                    b.Class_BillInfo.Bill_Run_Dt = DateTime.ParseExact((reader["BillDate"].ToString()), "MM/DD/YYYY", null);
                    b.Class_BillInfo.BillNumber = reader["BillNumber"].ToString();
                    b.Class_BillInfo.Balance_Due = Convert.ToDecimal(reader["AccountBalance"].ToString());
                    b.Due_Dt = DateTime.ParseExact((reader["DueDate"].ToString()), "MM/DD/YYYY", null);
                    b.Class_BillInfo.Bill_Amount= Convert.ToDecimal(reader["BillAmount"].ToString());
                    b.Class_BillInfo.FormatGUID= reader["FormatGUID"].ToString();
                    b.DateAdded = DateTime.ParseExact((reader["DateAdded"].ToString()), "MM/DD/YYYY", null);

                    returnData.Add(b);
                }
                
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
            string queryInsertIntoCustomer = "Insert into Customer(CustomerName,AccountNumber,CustomerAddress,CustomerCity,CustomerState,CustomerZip,DateAdded) VALUES(?,?,?,?,?,?,?)";

                OleDbCommand customerCommand = new OleDbCommand(queryInsertIntoCustomer, con);

            b.DateAdded = DateTime.Today;

                customerCommand.Parameters.AddWithValue("@CustomerName", b.Customer_Name);
                customerCommand.Parameters.AddWithValue("@AccountNumber", b.Account_No);
                customerCommand.Parameters.AddWithValue("@CustomerAddress", b.Class_AddressInformation.Mailing_Address_1);
                customerCommand.Parameters.AddWithValue("@CustomerCity", b.Class_AddressInformation.City);
                customerCommand.Parameters.AddWithValue("@CustomerState", b.Class_AddressInformation.State);
                customerCommand.Parameters.AddWithValue("@CustomerZip", b.Class_AddressInformation.Zip);
                customerCommand.Parameters.AddWithValue("@CustomerZip", b.DateAdded);


            return customerCommand;
        }

        private OleDbCommand PrepareBillCommand(BillHeader b)
        {

            string queryInsertIntoBill = "Insert into Bills(BillDate,BillNumber,BillAmount,FormatGUID,AccountBalance,DueDate,ServiceAddress,FirstEmailDate,SecondEmailDate,CustomerID) VALUES(?,?,?,?,?,?,?,?,?,?)";
            
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
                billCommand.Parameters.AddWithValue("@CustomerID", b.Class_BillInfo.CustomerID);

            return billCommand;
        }

        #endregion

    }
}
