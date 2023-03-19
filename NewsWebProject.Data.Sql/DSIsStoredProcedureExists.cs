using NewsWebProject.Dal;
using System.Data.SqlClient;
using Utilities.Logger;

namespace NewsWebProject.Data.Sql
{
    public class DSIsStoredProcedureExists : BaseDataSql, ISetData
    {
        public DSIsStoredProcedureExists(Logger looger) : base(looger) 
        {
            SetSql = new DatabaseOperations();
        }
        public DatabaseOperations SetSql { get; set; }
        public DSIsStoredProcedureExists IfExists { get; set; }

        public string Insert { get; set; } = "SELECT COUNT(*) FROM sys.objects WHERE type = 'P' AND name = @storedProcedureName";

        public object InsertToDataBase(SqlCommand command, params object[] argv)
        {
            try
            {
                int count = 0;

                if (argv != null)
                {
                    string storedProcedureName = argv.OfType<string>().FirstOrDefault();
                    command.Parameters.AddWithValue("@storedProcedureName", storedProcedureName);
                    count = (int)command.ExecuteScalar();
                }

                return (count > 0);
            }
            catch (Exception Ex)
            {

                throw;
            }
        }
        


        public object SetData(object value)
        {
            bool flag = false;

            if (value != null)
            {
                try
                {
                    if (value is string)
                    {
                         object objectFlag = SetSql.RunCommand(Insert, InsertToDataBase, value);

                        if (objectFlag != null && objectFlag is bool)
                        {
                            flag = (bool)objectFlag;
                        }
                    }

                    return flag;

                }
                catch (Exception EX) { }
            }

            return flag;

        }
    }
}
