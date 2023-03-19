using NewsWebProject.Dal;
using System.Data.SqlClient;
using Utilities.Logger;

namespace NewsWebProject.Data.Sql
{
    public class BaseDataSql
    {
        public Logger Logger;
        public BaseDataSql(Logger logger)
        {
            Logger = logger;

            try
            {
                if (Logger != null)
                {
                    Logger.LogEvent("The Logger initialization operation was performed successfully");

                    Logger.LogEvent("In the BaseDataSql constructor");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
    interface ISetData
    {
        /// <summary>
        /// 
        /// </summary>
        public DatabaseOperations SetSql { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DSIsStoredProcedureExists IfExists { get; set; }

        /// <summary>
        /// Sql query
        /// </summary>
        public string Insert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="argv"></param>
        public object InsertToDataBase(SqlCommand command, params object[] argv);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public object SetData(object value);

    }

    interface IGetData
    {
        /// <summary>
        /// 
        /// </summary>
        public DatabaseOperations GetSql { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DSIsStoredProcedureExists IfExists { get; set; }

        /// <summary>
        /// Sql query
        /// </summary>
        public string Insert { get; set; }

        /// <summary>
        /// Getting Data from sql
        /// </summary>
        /// <param name="argv">SqlDataReader and sometimes gets a unique value</param>
        /// <returns>Data from sql</returns>
        public object GetFromDataBase(params object[] argv);

        /// <summary>
        /// Inserting values ​​into SQL
        /// </summary>
        /// <param name="command">SQL connection</param>
        /// <param name="argv">values ​​we will enter</param>
        public object SetValues(SqlCommand command, params object[] argv);

        /// <summary>
        /// Contact functions that receive the data we required in SQL
        /// </summary>
        /// <param name="value">Identity value </param>
        /// <returns>Data we required</returns>
        public object GetData(object value);
    }

}
