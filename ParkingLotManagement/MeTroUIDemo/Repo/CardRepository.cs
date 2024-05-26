using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class CardRepository
    {
        private Paking_SlotEntities dbContext;

        public CardRepository()
        {
            dbContext = new Paking_SlotEntities();
        }

        public DataTable sp_cardInfor()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_cardInfor";
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

        public string sp_ValidateCardId(string Id)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_ValidateCardId";
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
        public void sp_insertCardInfor(
        string cardInforId, int ticketTypeId, int packingSlotID, bool isUsing, bool isMonthlyCard, bool isLock, bool isCardWasCancel)
        {
          
            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_insertCardInfor";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@cardInforId", cardInforId));
                command.Parameters.Add(new SqlParameter("@ticketTypeId", ticketTypeId));
                command.Parameters.Add(new SqlParameter("@packingSlotID", packingSlotID));
                 command.Parameters.Add(new SqlParameter("@isUsing", isUsing));
                command.Parameters.Add(new SqlParameter("@isMonthlyCard", isMonthlyCard));
                command.Parameters.Add(new SqlParameter("@isLock", isLock));
                command.Parameters.Add(new SqlParameter("@isCardWasCancel", isCardWasCancel));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_updateCardInfor(
     string cardInforId, int ticketTypeId, int packingSlotID, bool isUsing, bool isMonthlyCard, bool isLock, bool isCardWasCancel)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_updateCardInfor";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@cardInforId", cardInforId));
                command.Parameters.Add(new SqlParameter("@ticketTypeId", ticketTypeId));
                command.Parameters.Add(new SqlParameter("@packingSlotID", packingSlotID));
                command.Parameters.Add(new SqlParameter("@isUsing", isUsing));
                command.Parameters.Add(new SqlParameter("@isMonthlyCard", isMonthlyCard));
                command.Parameters.Add(new SqlParameter("@isLock", isLock));
                command.Parameters.Add(new SqlParameter("@isCardWasCancel", isCardWasCancel));


                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

        public void sp_deleteCardInfor(
        string cardInforId)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_deleteCardInfor";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@cardInforId", cardInforId));

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();

                dbContext.Database.Connection.Close();
            }


        }

        public DataTable sp_packingSlotList()
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_packingSlotList";
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


    }
}
