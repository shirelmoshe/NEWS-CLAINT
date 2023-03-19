using HtmlAgilityPack;
using NewsWebProject.Data.Sql.NewsItems;
using NewsWebProject.Model.Tables;
using NewsWebProject.Model.ViewModel;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using Utilities.Logger;

namespace NewsWebProject.Entites.WebsData.ModelProviders
{
    public class MPYnet: BaseWebsData, IProvideData
    {
        public DataTable DataTable { get; set; }
        public Task QueueTask { get; set; }
        public bool StopLoop { get; set; } = false;
        public List<TBRSSWebs> WebsData { get; set; }
        public DateTime StartRead { get; set; }
        public DSGetEachCategoryLastItems GetEachCategoryLastItems { get; set; }
        public List<VMLastNew> LastNewDate { get; set; }
        public DSInsertNewItems InsertItems { get; set; }
        public MPYnet(Logger logger, List<TBRSSWebs> ynetData) : base(logger)
        {
            WebsData = ynetData;

            GetEachCategoryLastItems = new DSGetEachCategoryLastItems(null);

            InsertItems = new DSInsertNewItems(null);

            Init();
        }
        public void Init()
        {
            QueueTask = Task.Run(() =>
            {
                DataTable = this.CreateDataTableTable();

                while (!StopLoop)
                {
                    if (WebsData!= null && WebsData.Count > 0)
                    {
                        this.GettingEachCategoryLastNews(WebsData.First().RSSWebName);

                        this.GettingEachCategoryNews(WebsData);

                        lock (obj)
                        {
                            this.InsertNewItems(DataTable);
                        }
                    }

                    //  DataTable.Clear();
                    DataTable.Rows.Count.ToString();
                    Thread.Sleep(1000 * 60 * 3);
                    int x = 1;
                }
            });
        }
        public void GettingEachCategoryLastNews(string WebName)
        {
            object ObjectList = GetEachCategoryLastItems.GetData(WebName);

            if (ObjectList != null && ObjectList is List<VMLastNew>)
            {
                LastNewDate = (List<VMLastNew>)ObjectList;
            }
        }
        public void GettingEachCategoryNews(List<TBRSSWebs> WebsData)
        {

            foreach (TBRSSWebs WebData in WebsData)
            {
                VMLastNew lastNew = null;

                IEnumerable<XElement>? items;

                XDocument doc = XDocument.Load(WebData.RSSWebUrl);

                XNamespace media = "http://search.yahoo.com/mrss/";

                int count = 0;

                if (LastNewDate != null)
                {
                    lastNew = LastNewDate.FirstOrDefault(l => l.RSSWebID == WebData.RSSWebID);
                }

                if (lastNew != null)
                {
                    items = doc.Descendants("item").Take(10).Where(i => ((DateTime)i.Element("pubDate")) > lastNew.NewsItemDate);
                }
                else
                {
                    items = doc.Descendants("item").Take(10);
                }


                foreach (var item in items)
                {
                    string src;
                    string description;

                    this.SubstringImageAndDescription(item.Element("description").Value, "</div>", null,"src", out src, out description);

                    DataTable.Rows.Add(item.Element("title").Value, src, description, item.Element("link").Value, 0, WebData.RSSWebID, true, (DateTime)item.Element("pubDate"));

                    count++;

                    //לנסות לעשות אינדאקאר
                    //להוסיף תאריך
                }

            }

        }
        public void InsertNewItems(DataTable dataTable)
        {
            if (dataTable != null)
            {
                InsertItems.SetData(dataTable);

                DataTable.Clear();
            }
        }

    }
}
