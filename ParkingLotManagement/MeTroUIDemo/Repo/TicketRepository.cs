using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class TicketRepository
    {
        private Paking_SlotEntities dbContext;

        public TicketRepository()
        {
            dbContext = new Paking_SlotEntities();
        }
        

         public DataTable sp_TicketPriceManagementSubmenu(int TicketTypeID, string DayApply)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_TicketPriceManagementSubmenu";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));
                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));


                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }
        public DataTable sp_TicketPriceManagement()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_TicketPriceManagement";
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

        public DataTable sp_carTypeList_DataSetting()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_carTypeList_DataSetting";
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

        public DataTable sp_ticketTypeList()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ticketTypeList";
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

        public DataTable sp_shiftList()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_shiftList";
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

        public DataTable sp_methodCalPrice_DataSetting()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_methodCalPrice_DataSetting";
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

        public void sp_updateTicketType(
            string TicketTypeName ,
            string DayApply ,
            int carTypeID ,
            float monthlyFree ,
            int hourMin ,
            int method ,
            int ReminderCardExpiration ,
            int TicketTypeID )
        {
          
            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_updateTicketType";
                command.CommandType = CommandType.StoredProcedure;

              
                command.Parameters.Add(new SqlParameter("@TicketTypeName", TicketTypeName));
                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));   
                command.Parameters.Add(new SqlParameter("@carTypeID", carTypeID));
                command.Parameters.Add(new SqlParameter("@monthlyFree", monthlyFree));
                command.Parameters.Add(new SqlParameter("@hourMin", hourMin));
                command.Parameters.Add(new SqlParameter("@method", method));   
                command.Parameters.Add(new SqlParameter("@ReminderCardExpiration", ReminderCardExpiration));
                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));
  




                dbContext.Database.Connection.Open();
             command.ExecuteNonQuery();

               
                dbContext.Database.Connection.Close();
            }

          
        }

        public void sp_insertTicketType(
           string TicketTypeName,
           string DayApply,
           int carTypeID,
           float monthlyFree,
           int hourMin,
           int method,
           int ReminderCardExpiration,
           int TicketTypeID)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_insertTicketType";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@TicketTypeName", TicketTypeName));
                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));
                command.Parameters.Add(new SqlParameter("@carTypeID", carTypeID));
                command.Parameters.Add(new SqlParameter("@monthlyFree", monthlyFree));
                command.Parameters.Add(new SqlParameter("@hourMin", hourMin));
                command.Parameters.Add(new SqlParameter("@method", method));
                command.Parameters.Add(new SqlParameter("@ReminderCardExpiration", ReminderCardExpiration));
                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));





                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_insertTicketPricing(
         int TicketTypeID , int ShiftID , float Price ,float minPrice,float generalPriceForTimeSlot ,
         float overtimePay, string DayApply)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_insertTicketPricing";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));
                command.Parameters.Add(new SqlParameter("@ShiftID", ShiftID));
                command.Parameters.Add(new SqlParameter("@Price", Price));
                command.Parameters.Add(new SqlParameter("@minPrice", minPrice));
                command.Parameters.Add(new SqlParameter("@generalPriceForTimeSlot", generalPriceForTimeSlot));
                command.Parameters.Add(new SqlParameter("@overtimePay", overtimePay));
                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));
              
                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_updateTicketPricing(
       int TicketTypeID, int ShiftID, float Price, float minPrice, float generalPriceForTimeSlot,
       float overtimePay, string DayApply)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_updateTicketPricing";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));
                command.Parameters.Add(new SqlParameter("@ShiftID", ShiftID));
                command.Parameters.Add(new SqlParameter("@Price", Price));
                command.Parameters.Add(new SqlParameter("@minPrice", minPrice));
                command.Parameters.Add(new SqlParameter("@generalPriceForTimeSlot", generalPriceForTimeSlot));
                command.Parameters.Add(new SqlParameter("@overtimePay", overtimePay));
                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_deleteTicketType(
        string DayApply,
        int TicketTypeID)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_deleteTicketType";
                command.CommandType = CommandType.StoredProcedure;


               
                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));
                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));





                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_deleteTicketPricing(
       int ShiftID,
       string DayApply,
       int TicketTypeID)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_deleteTicketPricing";
                command.CommandType = CommandType.StoredProcedure;



                command.Parameters.Add(new SqlParameter("@DayApply", DayApply));
                command.Parameters.Add(new SqlParameter("@TicketTypeID", TicketTypeID));
                command.Parameters.Add(new SqlParameter("@ShiftID", ShiftID));





                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public DataTable sp_ShiftManagementSubmenu()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ShiftManagementSubmenu";
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
      
        public void sp_ShiftManagementSubmenuInsert(
       int ShiftID, string ShiftName, string startPackingShifts, string endPackingShifts)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ShiftManagementSubmenuInsert";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@ShiftID", ShiftID));
                command.Parameters.Add(new SqlParameter("@ShiftName", ShiftName));
                command.Parameters.Add(new SqlParameter("@startPackingShifts", startPackingShifts));
                command.Parameters.Add(new SqlParameter("@endPackingShifts", endPackingShifts));
               
                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_ShiftManagementSubmenuUpdate(
     int ShiftID, string ShiftName, string startPackingShifts, string endPackingShifts)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ShiftManagementSubmenuUpdate";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@ShiftID", ShiftID));
                command.Parameters.Add(new SqlParameter("@ShiftName", ShiftName));
                command.Parameters.Add(new SqlParameter("@startPackingShifts", startPackingShifts));
                command.Parameters.Add(new SqlParameter("@endPackingShifts", endPackingShifts));

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_ShiftManagementSubmenuDelete(
    int ShiftID)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ShiftManagementSubmenuDelete";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@ShiftID", ShiftID));
              
                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }
    }
}
