using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class StatictisSingleTicketRepository
    {
        private Paking_SlotEntities dbContext;

        public StatictisSingleTicketRepository()
        {
            dbContext = new Paking_SlotEntities();
        }

        public DataTable sp_statisticLineChartByDay(string procedure,string startDayStatictis, string endDayStatictis)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = procedure;
                command.CommandType = CommandType.StoredProcedure;

                if (procedure.Equals("sp_3D_Pie_Chart_ByMonthInYear"))
                {
                    command.Parameters.Add(new SqlParameter("@yearReport", startDayStatictis));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@startTimeReport", startDayStatictis));
                    command.Parameters.Add(new SqlParameter("@endTimeReport", endDayStatictis));
                }

               
              

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
