using NewsWebProject.Dal;
using NewsWebProject.Model.Tables;
using NewsWebProject.Model.ViewModel;
using System.Data.SqlClient;
using Utilities.Logger;

namespace NewsWebProject.Data.Sql.Users
{
    public class DSGetUserCategories : BaseDataSql, IGetData
    {
        public DSGetUserCategories(Logger Logger) : base(Logger)
        {
            GetSql = new DatabaseOperations();

            IfExists = new DSIsStoredProcedureExists(Logger);
        }

        public DatabaseOperations GetSql { get; set; }
        public DSIsStoredProcedureExists IfExists { get; set; }
        public string Insert { get; set; } = "GetCategoriesAndUserCategories";

        public object GetFromDataBase(params object[] argv)
        {
            VMUserCategory UserAndCategories = null;

            List<TBUsersCategories> userCategories = new List<TBUsersCategories>();

            List<TBCategories> categories = new List<TBCategories>();

            bool Flag = true;

            if (argv.Length > 0)
            {
                while (Flag)
                {

                    try
                    {
                        SqlDataReader reader = argv.OfType<SqlDataReader>().FirstOrDefault();

                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string Answer = reader["Answer"].ToString();

                                if (Answer == null)
                                {
                                    //Logger.LogError("End AddTwitterUserRow function,The Answer of type of tweets is null");

                                    return null;
                                }


                                switch (Answer)
                                {
                                    case "userCategories":

                                        userCategories.Add(new TBUsersCategories
                                        {
                                            Category = new TBCategories
                                            {
                                                CategoryID = int.Parse(reader["Category_CategoryID"].ToString()),
                                                CategoryName = reader["CategoryName"].ToString(),
                                            },
                                            User = new TBUsers
                                            {
                                                UserGmail = reader["UserGmail"].ToString(),
                                                UserID = int.Parse(reader["User_UserID"].ToString()),
                                            },
                                            UserCategoryID = int.Parse(reader["UserCategoryID"].ToString()),

                                        }) ;

                                        break;

                                    case "categories":

                                        categories.Add(new TBCategories
                                        {
                                            CategoryID = int.Parse(reader["CategoryID"].ToString()),
                                            CategoryName = reader["CategoryName"].ToString(),
                                        });

                                        break;
                                }

                                // Logger.LogEvent("End AddBusinessCompanyInformation function and taking the data of business company representative");

                            }
                        }

                        if (!reader.NextResult())
                        {
                            Flag = false;

                            UserAndCategories = new VMUserCategory
                            {
                                Categories = categories,
                                UsersCategories = userCategories,
                            };

                            //Logger.LogEvent("End AddTwitterUserRow function successfully");
                            return UserAndCategories;
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

                        command.Parameters.AddWithValue("@UserEmail", select);

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

            List<VMUserCategory> userCategories = null;

            bool flag = false;

            try
            {
                object objectFlag = IfExists.SetData(Insert);

                if (objectFlag != null && objectFlag is bool)
                {
                    flag = (bool)objectFlag;

                    if (flag)
                    {
                        object listObjectCategories = GetSql.GetData(Insert, SetValues, GetFromDataBase, value);

                        if (listObjectCategories is List<VMUserCategory>)
                        {
                            userCategories = (List<VMUserCategory>)listObjectCategories;
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

            return userCategories;

        }

    }
}
