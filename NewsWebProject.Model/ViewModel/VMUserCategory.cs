using NewsWebProject.Model.Tables;

namespace NewsWebProject.Model.ViewModel
{
    public class VMUserCategory
    {
        public VMUserCategory()
        {
            Categories = new List<TBCategories>();
            UsersCategories = new List<TBUsersCategories>();
        }

        public List<TBCategories> Categories { get; set; }
        public List< TBUsersCategories> UsersCategories { get; set; }
    }
}
