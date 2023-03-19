using NewsWebProject.Dal;
using NewsWebProject.Model.ViewModel;
using System.Data;
using System.Data.SqlClient;
using Utilities.Logger;

namespace NewsWebProject.Data.Sql.NewsItems
{
    public class DSGetEachCategoryLastItems : BaseDataSql, IGetData
    {

        public DSGetEachCategoryLastItems(Logger Logger) : base(Logger)
        {
            GetSql = new DatabaseOperations();

            IfExists = new DSIsStoredProcedureExists(Logger);
        }

        public DatabaseOperations GetSql { get; set; }
        public DSIsStoredProcedureExists IfExists { get; set; }


        public string Insert { get; set; } = "GetLastItemEachWeb";
        public object GetFromDataBase(params object[] argv)
        {
            List<VMLastNew> LastNewList = new List<VMLastNew>();

            if (argv.Length > 0)
            {
                try
                {
                    SqlDataReader reader = argv.OfType<SqlDataReader>().FirstOrDefault();

                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            LastNewList.Add(new VMLastNew
                            {
                                NewsItemDate = (DateTime)reader["NewsItemDate"],
                                RSSWebID = int.Parse(reader["RSSWeb_RSSWebID"].ToString()),
                            });
                        }

                        // Logger.LogEvent("End AddBusinessCompanyInformation function and taking the data of business company representative");

                        return LastNewList;
                    }
                }
                catch (SqlException Ex)
                {
                    // Logger.LogException(Ex.Message, Ex);

                    throw;
                }
                catch (Exception Ex)
                {
                    // Logger.LogException(Ex.Message, Ex);

                    throw;
                }

            }


            return null;

        }


        public object SetValues(SqlCommand command, params object[] argv)
        {

            if (argv.Length > 0)
            {
                try
                {
                    string select = argv.OfType<string>().FirstOrDefault();

                    if (select != null)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@WebName", select);

                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception Ex) 
                {
                    throw;
                }

            }

            return null;
        }


        public object GetData(object value)
        {
            // Logger.LogEvent("\n\nEnter into GetBusinessCompanyUserRow function");

            List<VMLastNew> LastNewList = null;

            bool flag = false;

            try
            {
                object objectFlag= IfExists.SetData(Insert);

                if (objectFlag != null && objectFlag is bool)
                {
                    flag = (bool)objectFlag;

                    if(flag)
                    {
                        object listObjectNews = GetSql.GetData(Insert, SetValues, GetFromDataBase, value);

                        if (listObjectNews is List<VMLastNew>)
                        {
                            LastNewList = (List<VMLastNew>)listObjectNews;
                        }
                    }

                }


            }
            catch (Exception Ex)
            {
                //Logger.LogException(Ex.Message, Ex);

                throw;
            }

            // Logger.LogEvent("End GetBusinessCompanyUserRow function");

            return LastNewList;

        }
    }
}
