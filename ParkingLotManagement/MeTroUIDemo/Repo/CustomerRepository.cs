using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class CustomerRepository
    {
        private Paking_SlotEntities dbContext;

        public CustomerRepository()
        {
            dbContext = new Paking_SlotEntities();
        }
        public DataTable sp_ticketRegistInforManagement(int customerID)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ticketRegistInforManagement";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@customerID", customerID));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }
        public DataTable sp_CustomerRegister()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_CustomerRegister";
                command.CommandType = CommandType.StoredProcedure;


                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }
        public string sp_ValidateCustomerId(string CustomerId)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ValidateCustomerId";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@CustomerId", CustomerId));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }
            if (dataTable.Rows.Count > 0)
            {
                DataRow r = dataTable.Rows[0];

                return r[0].ToString();
            }

            return "";
        }
        public void sp_insertcustomer(
            int customerID , string identitycard , string customerName)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_insertcustomer";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@customerID", customerID));
                command.Parameters.Add(new SqlParameter("@identitycard", identitycard));
                command.Parameters.Add(new SqlParameter("@customerName", customerName));
                

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_updatecustomer(
           int customerID, string identitycard, string customerName)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_updatecustomer";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@customerID", customerID));
                command.Parameters.Add(new SqlParameter("@identitycard", identitycard));
                command.Parameters.Add(new SqlParameter("@customerName", customerName));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_deletecustomer(
          int customerID)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_deletecustomer";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@customerID", customerID));

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_insertticketRegistInfor(
            string drivingLicenseNumer, string timeRegister, int customerID, string licensePlateNumber,
 string expirationDate, float totalAmount, int numberRegisterMonth, string cardInfroID)
        {
         

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_insertticketRegistInfor";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@customerID", customerID));
                command.Parameters.Add(new SqlParameter("@drivingLicenseNumer", drivingLicenseNumer));
                command.Parameters.Add(new SqlParameter("@timeRegister", timeRegister));
                command.Parameters.Add(new SqlParameter("@licensePlateNumber", licensePlateNumber));
                command.Parameters.Add(new SqlParameter("@expirationDate", expirationDate));
                command.Parameters.Add(new SqlParameter("@totalAmount", totalAmount));
                command.Parameters.Add(new SqlParameter("@numberRegisterMonth", numberRegisterMonth));
                command.Parameters.Add(new SqlParameter("@cardInfroID", cardInfroID));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_updateticketRegistInfor(
           int registerInforId, string drivingLicenseNumer, string timeRegister, int customerID, string licensePlateNumber,
 string expirationDate, float totalAmount, int numberRegisterMonth, string cardInfroID)
        {


            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_updateticketRegistInfor";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@registerInforId", registerInforId));
                command.Parameters.Add(new SqlParameter("@customerID", customerID));
                command.Parameters.Add(new SqlParameter("@drivingLicenseNumer", drivingLicenseNumer));
                command.Parameters.Add(new SqlParameter("@timeRegister", timeRegister));
                command.Parameters.Add(new SqlParameter("@licensePlateNumber", licensePlateNumber));
                command.Parameters.Add(new SqlParameter("@expirationDate", expirationDate));
                command.Parameters.Add(new SqlParameter("@totalAmount", totalAmount));
                command.Parameters.Add(new SqlParameter("@numberRegisterMonth", numberRegisterMonth));
                command.Parameters.Add(new SqlParameter("@cardInfroID", cardInfroID));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_deleteticketRegistInfor(
          int registerInforId)
        {


            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_deleteticketRegistInfor";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@registerInforId", registerInforId));
              

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }


        public string sp_totalAmountBycardID(int numberRegisterMonth, string cardInfroID)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_totalAmountBycardID";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@cardInfroID", cardInfroID));
                command.Parameters.Add(new SqlParameter("@numberRegisterMonth", numberRegisterMonth));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }
            if(dataTable.Rows.Count > 0)
            {
                DataRow r = dataTable.Rows[0];

                return r[0].ToString();
            }

            return "";
        }
    }
}
