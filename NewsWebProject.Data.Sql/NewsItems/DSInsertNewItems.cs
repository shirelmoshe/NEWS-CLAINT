using NewsWebProject.Dal;
using System.Data;
using System.Data.SqlClient;
using Utilities.Logger;

namespace NewsWebProject.Data.Sql.NewsItems
{
    public class DSInsertNewItems : BaseDataSql, ISetData
    {
        public DSInsertNewItems(Logger logger):base(logger) 
        {
           SetSql = new DatabaseOperations();

           IfExists = new DSIsStoredProcedureExists(logger);
        }
        public DatabaseOperations SetSql { get; set; }

        public DSIsStoredProcedureExists IfExists { get; set; }

        public string Insert { get; set; } = "AddCategoriesNewsItems";

        public object InsertToDataBase(SqlCommand command, params object[] argv)
        {
            if (command!= null&& argv.Length > 0)
            {
                try
                {
                    DataTable DataTable = argv.OfType<DataTable>().FirstOrDefault();

                    if (DataTable != null)
                    {
                        // Execute the stored procedure and pass the DataTable as a parameter
                        command.CommandType = CommandType.StoredProcedure;

                        //Logger.LogEvent("Running the procedure that performs the test process, inserts the appropriate tweets");

                        command.Parameters.AddWithValue("@TempItemTable", DataTable);

                        int val = command.ExecuteNonQuery();

                        return val;
                    }
                    //Logger.LogEvent($"End PostTracking function, The procedure returns: {val}");
                }
                catch (Exception EX){ }
            }

            return null;

        }


        public object SetData(object value)
        {
            if (value != null) 
            {
                try
                {
                    bool flag = false;

                    object objectFlag = IfExists.SetData(Insert);

                    if (objectFlag != null && objectFlag is bool)
                    {
                        flag = (bool)objectFlag;

                        if (flag)
                        {

                            if (value is DataTable)
                            {
                                DataTable DataTablel = (DataTable)value;

                                SetSql.RunCommand(Insert, InsertToDataBase, DataTablel);
                            }
                        }
                    }

                    return null;

                }
                catch (Exception EX) { }
            }

            return null;

        }
    }
}
