using NewsWebProject.Data.Sql.RSSWebs;
using NewsWebProject.Data.Sql.Users;
using NUnit.Framework;

namespace NewsWebProject.Data.Sql.Test
{
    [TestFixture]
    public class GetCategories:BaseTestDataSql
    {
        [Test]
        [SetUp]
        public void Init()
        {
            UserCategories = new DSGetUserCategories(null);
        }

        private DSGetUserCategories UserCategories;

        [Test]
        [Category("GetSqlDataRSSWebs")]
        public void GetWebsList()
        {
            string Insert = "GetCategoriesAndUserCategories";
            string value = "aaa@gmail.com";

            Assert.That(UserCategories.GetSql.GetData(Insert, UserCategories.SetValues, UserCategories.GetFromDataBase, value), !Is.Null);
        }
    }
}
