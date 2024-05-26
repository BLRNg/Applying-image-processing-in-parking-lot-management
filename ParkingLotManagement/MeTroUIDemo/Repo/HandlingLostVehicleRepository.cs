using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class HandlingLostVehicleRepository
    {
        private Paking_SlotEntities dbContext;

        public HandlingLostVehicleRepository()
        {
            dbContext = new Paking_SlotEntities();
        }
        public string sp_ValidateUnitLostVehicleId(string Id)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ValidateUnitLostVehicleId";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", Id));

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
        public void sp_InsertHandlingLostVehicle(
             int IncidentID ,string CustomerName ,string CustomerAddress ,string CustomerPhoneNumber ,string PlateNumber ,
            string VehicleType,string VehicleColor,string ParkingTime , string ParkingLocation ,string IncidentTime ,
             string IncidentLocation ,string InvestigationResult ,float CompensationAmount ,bool ReportToAuthorities, string ResponseToCustomer,
            string Agency_resolving_the_incident ,string birthDay , string placeOfOrigin,string currentOccupation ,string Identity_Number,
            string Identity_date_issue , string Identity_place_issue ,string Cause_of_vehicle_loss 
        )
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_InsertHandlingLostVehicle";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@IncidentID", IncidentID));
                command.Parameters.Add(new SqlParameter("@CustomerName", CustomerName));
                command.Parameters.Add(new SqlParameter("@CustomerAddress", CustomerAddress));
                command.Parameters.Add(new SqlParameter("@CustomerPhoneNumber", CustomerPhoneNumber));
                command.Parameters.Add(new SqlParameter("@PlateNumber", PlateNumber));
                command.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                command.Parameters.Add(new SqlParameter("@VehicleColor", VehicleColor));
                command.Parameters.Add(new SqlParameter("@ParkingTime", ParkingTime));
                command.Parameters.Add(new SqlParameter("@ParkingLocation", ParkingLocation));
                command.Parameters.Add(new SqlParameter("@IncidentTime", IncidentTime));
                command.Parameters.Add(new SqlParameter("@IncidentLocation", IncidentLocation));
                command.Parameters.Add(new SqlParameter("@InvestigationResult", InvestigationResult));
                command.Parameters.Add(new SqlParameter("@CompensationAmount", CompensationAmount));
                command.Parameters.Add(new SqlParameter("@ReportToAuthorities", ReportToAuthorities)); 
                command.Parameters.Add(new SqlParameter("@ResponseToCustomer", ResponseToCustomer));
                command.Parameters.Add(new SqlParameter("@Agency_resolving_the_incident", Agency_resolving_the_incident));
                command.Parameters.Add(new SqlParameter("@birthDay", birthDay));
                command.Parameters.Add(new SqlParameter("@placeOfOrigin", placeOfOrigin));
                command.Parameters.Add(new SqlParameter("@currentOccupation", currentOccupation));
                command.Parameters.Add(new SqlParameter("@Identity_Number", Identity_Number));
                command.Parameters.Add(new SqlParameter("@Identity_date_issue", Identity_date_issue));
                command.Parameters.Add(new SqlParameter("@Identity_place_issue", Identity_place_issue));
                command.Parameters.Add(new SqlParameter("@Cause_of_vehicle_loss", Cause_of_vehicle_loss));
              


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_UpdateHandlingLostVehicle(
            int IncidentID, string CustomerName, string CustomerAddress, string CustomerPhoneNumber, string PlateNumber,
           string VehicleType, string VehicleColor, string ParkingTime, string ParkingLocation, string IncidentTime,
            string IncidentLocation, string InvestigationResult, float CompensationAmount, bool ReportToAuthorities, string ResponseToCustomer,
           string Agency_resolving_the_incident, string birthDay, string placeOfOrigin, string currentOccupation, string Identity_Number,
           string Identity_date_issue, string Identity_place_issue, string Cause_of_vehicle_loss
       )
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_UpdateHandlingLostVehicle";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@IncidentID", IncidentID));
                command.Parameters.Add(new SqlParameter("@CustomerName", CustomerName));
                command.Parameters.Add(new SqlParameter("@CustomerAddress", CustomerAddress));
                command.Parameters.Add(new SqlParameter("@CustomerPhoneNumber", CustomerPhoneNumber));
                command.Parameters.Add(new SqlParameter("@PlateNumber", PlateNumber));
                command.Parameters.Add(new SqlParameter("@VehicleType", VehicleType));
                command.Parameters.Add(new SqlParameter("@VehicleColor", VehicleColor));
                command.Parameters.Add(new SqlParameter("@ParkingTime", ParkingTime));
                command.Parameters.Add(new SqlParameter("@ParkingLocation", ParkingLocation));
                command.Parameters.Add(new SqlParameter("@IncidentTime", IncidentTime));
                command.Parameters.Add(new SqlParameter("@IncidentLocation", IncidentLocation));
                command.Parameters.Add(new SqlParameter("@InvestigationResult", InvestigationResult));
                command.Parameters.Add(new SqlParameter("@CompensationAmount", CompensationAmount));
                command.Parameters.Add(new SqlParameter("@ReportToAuthorities", ReportToAuthorities));
                command.Parameters.Add(new SqlParameter("@ResponseToCustomer", ResponseToCustomer));
                command.Parameters.Add(new SqlParameter("@Agency_resolving_the_incident", Agency_resolving_the_incident));
                command.Parameters.Add(new SqlParameter("@birthDay", birthDay));
                command.Parameters.Add(new SqlParameter("@placeOfOrigin", placeOfOrigin));
                command.Parameters.Add(new SqlParameter("@currentOccupation", currentOccupation));
                command.Parameters.Add(new SqlParameter("@Identity_Number", Identity_Number));
                command.Parameters.Add(new SqlParameter("@Identity_date_issue", Identity_date_issue));
                command.Parameters.Add(new SqlParameter("@Identity_place_issue", Identity_place_issue));
                command.Parameters.Add(new SqlParameter("@Cause_of_vehicle_loss", Cause_of_vehicle_loss));



                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_DeleteHandlingLostVehicle(
           int IncidentID
      )
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_DeleteHandlingLostVehicle";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@IncidentID", IncidentID));
                

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

    }
}
