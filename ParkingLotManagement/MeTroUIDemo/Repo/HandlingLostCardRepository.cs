using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class HandlingLostCardRepository
    {
        private Paking_SlotEntities dbContext;

        public HandlingLostCardRepository()
        {
            dbContext = new Paking_SlotEntities();
        }
        public string sp_ValidateUnitLostCardId(string CardId)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ValidateUnitLostCardId";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@CardId", CardId));

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
        public void sp_InsertHandlingLostCard(
            string LostCardID ,string CardType ,string GuestName,string PlateNumber ,string LostDateTime ,
            string HandlingDateTime ,string HandlingAction ,float PenaltyAmount ,string AdditionalNotes ,string Birthday ,
            string Identity_number ,string Identity_date_issue , string Identity_plate_issue ,string permanent_residence ,string current_residence ,
            string phone_number,string Vehicle_Registration_Date , string Vehicle_Registration_Place ,string vehicleType ,string vehicleColor ,
            string Engine_number ,string Chassis_number,string Vehicle_owner ,string Location_of_lost_vehicle_card 
       )
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_InsertHandlingLostCard";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@LostCardID", LostCardID));
                command.Parameters.Add(new SqlParameter("@CardType", CardType));
                command.Parameters.Add(new SqlParameter("@GuestName", GuestName));
                command.Parameters.Add(new SqlParameter("@LostDateTime", LostDateTime));
                command.Parameters.Add(new SqlParameter("@PlateNumber", PlateNumber));
                command.Parameters.Add(new SqlParameter("@HandlingDateTime", HandlingDateTime));
                command.Parameters.Add(new SqlParameter("@HandlingAction", HandlingAction));
                command.Parameters.Add(new SqlParameter("@PenaltyAmount", PenaltyAmount));
                command.Parameters.Add(new SqlParameter("@AdditionalNotes", AdditionalNotes));
                command.Parameters.Add(new SqlParameter("@Birthday", Birthday));
                command.Parameters.Add(new SqlParameter("@Identity_number", Identity_number));
                command.Parameters.Add(new SqlParameter("@Identity_date_issue", Identity_date_issue));
                command.Parameters.Add(new SqlParameter("@Identity_plate_issue", Identity_plate_issue));
                command.Parameters.Add(new SqlParameter("@permanent_residence", permanent_residence));
                command.Parameters.Add(new SqlParameter("@current_residence", current_residence));
                command.Parameters.Add(new SqlParameter("@phone_number", phone_number));
                command.Parameters.Add(new SqlParameter("@Vehicle_Registration_Date", Vehicle_Registration_Date));
                command.Parameters.Add(new SqlParameter("@Vehicle_Registration_Place", Vehicle_Registration_Place));
                command.Parameters.Add(new SqlParameter("@vehicleType", vehicleType));
                command.Parameters.Add(new SqlParameter("@vehicleColor", vehicleColor));
                command.Parameters.Add(new SqlParameter("@Engine_number", Engine_number));
                command.Parameters.Add(new SqlParameter("@Chassis_number", Chassis_number));
                command.Parameters.Add(new SqlParameter("@Vehicle_owner", Vehicle_owner));
                command.Parameters.Add(new SqlParameter("@Location_of_lost_vehicle_card", Location_of_lost_vehicle_card));



                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_UpdateHandlingLostCard(
          string LostCardID, string CardType, string GuestName, string PlateNumber, string LostDateTime,
          string HandlingDateTime, string HandlingAction, float PenaltyAmount, string AdditionalNotes, string Birthday,
          string Identity_number, string Identity_date_issue, string Identity_plate_issue, string permanent_residence, string current_residence,
          string phone_number, string Vehicle_Registration_Date, string Vehicle_Registration_Place, string vehicleType, string vehicleColor,
          string Engine_number, string Chassis_number, string Vehicle_owner, string Location_of_lost_vehicle_card
     )
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_UpdateHandlingLostCard";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@LostCardID", LostCardID));
                command.Parameters.Add(new SqlParameter("@CardType", CardType));
                command.Parameters.Add(new SqlParameter("@GuestName", GuestName));
                command.Parameters.Add(new SqlParameter("@LostDateTime", LostDateTime));
                command.Parameters.Add(new SqlParameter("@PlateNumber", PlateNumber));
                command.Parameters.Add(new SqlParameter("@HandlingDateTime", HandlingDateTime));
                command.Parameters.Add(new SqlParameter("@HandlingAction", HandlingAction));
                command.Parameters.Add(new SqlParameter("@PenaltyAmount", PenaltyAmount));
                command.Parameters.Add(new SqlParameter("@AdditionalNotes", AdditionalNotes));
                command.Parameters.Add(new SqlParameter("@Birthday", Birthday));
                command.Parameters.Add(new SqlParameter("@Identity_number", Identity_number));
                command.Parameters.Add(new SqlParameter("@Identity_date_issue", Identity_date_issue));
                command.Parameters.Add(new SqlParameter("@Identity_plate_issue", Identity_plate_issue));
                command.Parameters.Add(new SqlParameter("@permanent_residence", permanent_residence));
                command.Parameters.Add(new SqlParameter("@current_residence", current_residence));
                command.Parameters.Add(new SqlParameter("@phone_number", phone_number));
                command.Parameters.Add(new SqlParameter("@Vehicle_Registration_Date", Vehicle_Registration_Date));
                command.Parameters.Add(new SqlParameter("@Vehicle_Registration_Place", Vehicle_Registration_Place));
                command.Parameters.Add(new SqlParameter("@vehicleType", vehicleType));
                command.Parameters.Add(new SqlParameter("@vehicleColor", vehicleColor));
                command.Parameters.Add(new SqlParameter("@Engine_number", Engine_number));
                command.Parameters.Add(new SqlParameter("@Chassis_number", Chassis_number));
                command.Parameters.Add(new SqlParameter("@Vehicle_owner", Vehicle_owner));
                command.Parameters.Add(new SqlParameter("@Location_of_lost_vehicle_card", Location_of_lost_vehicle_card));



                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_DeleteHandlingLostCard(
         string LostCardID
    )
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_DeleteHandlingLostCard";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@LostCardID", LostCardID));
               



                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

    }
}
