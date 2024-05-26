using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Repo
{
    class SettingParamRepo
    {
        private Paking_SlotEntities dbContext;

        public SettingParamRepo()
        {
            dbContext = new Paking_SlotEntities();
        }

        public string sp_SelectSetting(string name_setting)
        {
            // Create a DataTable to hold the result
            DataTable dataTable = new DataTable();

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_SelectSetting";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@name_setting", name_setting));
                
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



        public void sp_UpdateSettingParam(string name_setting, string value_setting)
        {

            // You can use a SQLDataAdapter to fill the DataSet
            using (var command = dbContext.Database.Connection.CreateCommand())
            {
                //command.CommandText = "sp_statisticLineChartByDay";
                command.CommandText = "sp_UpdateSettingParam";
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@name_setting", name_setting));
                command.Parameters.Add(new SqlParameter("@value_setting", value_setting));

                dbContext.Database.Connection.Open();
                command.ExecuteNonQuery();


                dbContext.Database.Connection.Close();
            }


        }

    }
}
