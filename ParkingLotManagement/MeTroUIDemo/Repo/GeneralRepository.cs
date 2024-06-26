﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class GeneralRepository
    {
        private Paking_SlotEntities dbContext;

        public GeneralRepository()
        {
            dbContext = new Paking_SlotEntities();
        }

        public DataTable ExecDataTable()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "spSelectTest";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@MachineName", "Hello"));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
            }

            return dataTable;
        }

        public DataTable sp_exportSingleUseTicketRevenueStatistics(string formattedDateTimeFrom, string formattedDateTimeTo)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_exportSingleUseTicketRevenueStatistics";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@startTimeReport", formattedDateTimeFrom));
                command.Parameters.Add(new SqlParameter("@endTimeReport", formattedDateTimeTo));

                using (var adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    // Accessing an array of DataTables from the DataSet
                    DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
                    dataSet.Tables.CopyTo(dataTables, 0);
                    return dataTables[0];
                    // Now you have an array of DataTables in dataTables variable
                    // You can iterate through each DataTable and process them as needed
                    foreach (DataTable table in dataTables)
                    {
                        // Process each DataTable as needed
                        foreach (DataRow row in table.Rows)
                        {
                            // Access values from the current DataRow
                            // Example: int id = (int)row["ID"];
                        }
                    }
                }
            }

            return dataTable;
        }

        public DataTable sp_export_statictisGrossSale(string formattedDateTimeFrom, string formattedDateTimeTo)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_export_statictisGrossSale";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@startTimeReport", formattedDateTimeFrom));
                command.Parameters.Add(new SqlParameter("@endTimeReport", formattedDateTimeTo));

                using (var adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    // Accessing an array of DataTables from the DataSet
                    DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
                    dataSet.Tables.CopyTo(dataTables, 0);
                    return dataTables[0];
                    // Now you have an array of DataTables in dataTables variable
                    // You can iterate through each DataTable and process them as needed
                    foreach (DataTable table in dataTables)
                    {
                        // Process each DataTable as needed
                        foreach (DataRow row in table.Rows)
                        {
                            // Access values from the current DataRow
                            // Example: int id = (int)row["ID"];
                        }
                    }
                }
            }

            return dataTable;
        }
        public DataTable sp_reportRevenueByMonthlyTicket(string formattedDateTimeFrom, string formattedDateTimeTo)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_reportRevenueByMonthlyTicket";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@startTimeReport", formattedDateTimeFrom));
                command.Parameters.Add(new SqlParameter("@endTimeReport", formattedDateTimeTo));

                using (var adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    // Accessing an array of DataTables from the DataSet
                    DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
                    dataSet.Tables.CopyTo(dataTables, 0);
                    return dataTables[0];
                    // Now you have an array of DataTables in dataTables variable
                    // You can iterate through each DataTable and process them as needed
                    foreach (DataTable table in dataTables)
                    {
                        // Process each DataTable as needed
                        foreach (DataRow row in table.Rows)
                        {
                            // Access values from the current DataRow
                            // Example: int id = (int)row["ID"];
                        }
                    }
                }
            }

            return dataTable;
        }
        public DataTable sp_SelectStatisticsByTimeSlot(string formattedDateTimeFrom, string formattedDateTimeTo)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_SelectStatisticsByTimeSlot";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@startTimeReport", formattedDateTimeFrom));
                command.Parameters.Add(new SqlParameter("@endTimeReport", formattedDateTimeTo));

                using (var adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    // Accessing an array of DataTables from the DataSet
                    DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
                    dataSet.Tables.CopyTo(dataTables, 0);
                    return dataTables[0];
                    // Now you have an array of DataTables in dataTables variable
                    // You can iterate through each DataTable and process them as needed
                    foreach (DataTable table in dataTables)
                    {
                        // Process each DataTable as needed
                        foreach (DataRow row in table.Rows)
                        {
                            // Access values from the current DataRow
                            // Example: int id = (int)row["ID"];
                        }
                    }
                }
            }

            return dataTable;
        }

        public DataTable sp_exportMonthlyTicketRevenueStatistics(string formattedDateTimeFrom, string formattedDateTimeTo)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_exportMonthlyTicketRevenueStatistics";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@startTimeReport", formattedDateTimeFrom));
                command.Parameters.Add(new SqlParameter("@endTimeReport", formattedDateTimeTo));

                using (var adapter = new SqlDataAdapter((SqlCommand)command))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    // Accessing an array of DataTables from the DataSet
                    DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
                    dataSet.Tables.CopyTo(dataTables, 0);
                    return dataTables[0];
                    // Now you have an array of DataTables in dataTables variable
                    // You can iterate through each DataTable and process them as needed
                    foreach (DataTable table in dataTables)
                    {
                        // Process each DataTable as needed
                        foreach (DataRow row in table.Rows)
                        {
                            // Access values from the current DataRow
                            // Example: int id = (int)row["ID"];
                        }
                    }
                }
            }

            return dataTable;
        }
        public DataTable ExecDataTableHistoryInOut(string formattedDateTimeFrom, string formattedDateTimeTo, string isOut, string isMonthLy, string isIn, string isSingleTiceket)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_historyinout";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@startTimeReport", formattedDateTimeFrom));
                command.Parameters.Add(new SqlParameter("@endTimeReport", formattedDateTimeTo));
                command.Parameters.Add(new SqlParameter("@veHicleIn", isIn));
                command.Parameters.Add(new SqlParameter("@veHicleOut", isOut));
                command.Parameters.Add(new SqlParameter("@monthltTicket", isMonthLy));
                command.Parameters.Add(new SqlParameter("@temporary", isSingleTiceket));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }

        public DataTable sp_LongTermParking(bool monthltTicket ,bool temporary , int numberOfDayParked)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_LongTermParking";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@monthltTicket", monthltTicket));
                command.Parameters.Add(new SqlParameter("@temporary", temporary));
                command.Parameters.Add(new SqlParameter("@numberOfDayParked", numberOfDayParked));
               

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }

        public DataTable sp_HandlingLostCard()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_HandlingLostCard";
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

        public DataTable sp_HandlingLostVehicle()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_HandlingLostVehicle";
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

        public DataTable sp_ExportReportOfLostVehicleCard(string LostCardID)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_ExportReportOfLostVehicleCard";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@LostCardID", LostCardID));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }

        public DataTable sp_ExportReportHandlingLostVehicle(string IncidentID)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                command.CommandText = "sp_ExportReportHandlingLostVehicle";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@IncidentID", IncidentID));

                dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                // Load the result set into the DataSet
                // Load the result set into the DataSet, specifying the columns
                dataTable.Load(reader);
                dbContext.Database.Connection.Close();
            }

            return dataTable;
        }
    }
}
