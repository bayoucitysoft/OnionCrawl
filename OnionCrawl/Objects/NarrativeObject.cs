using HtmlAgilityPack;
using OnionCrawl.Utility;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl.Objects
{
    public class NarrativeObject
    {
        public long Id { get; set; }
        public string CurrentMessage { get; set; }
        public DateTime Created { get; set; }
        public List<Message> MessageCollection { get; set; }
        public Guid Guid { get; set; }

        public Exception Ex { get; set; }

        public PhantomTorDriver Crawler { get; set; }
        public ScanObject ScanObject { get; set; }

        public NarrativeObject(PhantomTorDriver cralwer, ScanObject scanObject)
        {
            Created = DateTime.Now;
            MessageCollection = new List<Message>();
            Guid = Guid.NewGuid();
            Crawler = cralwer;
            ScanObject = scanObject;

            BeginNarrative();
        }

        public NarrativeObject()
        {
            // TODO: Complete member initialization
        }

        public void BeginNarrative()
        {
            try
            {
                this.Insert();
                this.RetreiveId();
                Crawler.Driver.Navigate().GoToUrl(ScanObject.Url);
                NewMessage(String.Format("Contact made with : {0}", ScanObject.Name), false);
                ScanObject.Description = Crawler.Driver.Title;
                ScanObject.PageSource = Crawler.Driver.PageSource;
                ScanObject.PageStatus = "OK";
                ScanObject.screenshot = Crawler.Driver.GetScreenshot().AsByteArray;
                GenerateScreenShot(ScanObject.Guid, Crawler.Driver);
                ScanObject.Update();
                NewMessage(String.Format("ScanObject {0} Updated", ScanObject.Name), false);
            }
            catch (Exception ex)
            {
                ScanObject.Description = ex.Message;
                NewMessage(String.Format("Error: {0}", ex.Message), true);
                ScanObject.PageStatus = "ERROR";
                ScanObject.Update();
                
            }
        }

        public void FindPotentialScans()
        {
            if (this.ScanObject.PageStatus != "ERROR")
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(this.ScanObject.PageSource);
                var _test = doc.DocumentNode.SelectNodes("//a");
                if(_test != null)
                {
                    var links = doc.DocumentNode.SelectNodes("//a").Select(x => x.OuterHtml).ToList();
                    for (int i = 0; i < links.Count; i++)
                    {
                        string[] parts = links[i].Split(new string[] { "href=" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in parts)
                        {
                            if (s.Contains(@"http://") && s.Contains(".onion"))
                            {
                                string url = string.Empty;
                                string name = string.Empty;

                                string[] initial = s.Split(new string[] { ">" }, StringSplitOptions.RemoveEmptyEntries);
                                for (int j = 0; j < initial[0].Count(); j++)
                                {
                                    if (initial[0][j] != '"')
                                        url += initial[0][j];
                                }
                                url = RemoveKnownHindrences(url);
                                string[] nameSplit = initial[1].Split(new string[] { "<" }, StringSplitOptions.RemoveEmptyEntries);
                                name = nameSplit[0];
                                Console.WriteLine(String.Format("Name: {0}, Url: {1}", name, url));
                                ScanObject o = new ScanObject(name, url, ScanObject.Id, ScanObject.CrawlDepth + 1 );
                                o.Insert();
                                NewMessage(String.Format("ScanObject: {0} inserted at {1}", o.Name, DateTime.Now.ToShortTimeString()), false);
                            }
                        }
                    }
                }
            }

        }

        private string RemoveKnownHindrences(string url)
        {
            url.Replace("rel=nofollow", ""); url.Replace("style=text-decoration: none;", "");
            url.Replace("rel=no-follow", ""); url.Replace("class=random choice", "");
            url.Replace("rel=home", ""); url.Replace("class=login-required", "");
            url.Replace("target=_BLANK", "");
            url.Replace("target=_blank", "");
            url.Replace(" target=_blank", "");
            url.Replace("class=choice", "");
            url.Replace("class=", "");
            return url;
        }

        private void RetreiveId()
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "GetNarrativeObjectId";
            db.Parameters.Add(@"@guid", Guid);
            db.Parameters.Add(@"scan_object_id", ScanObject.Id);
            db.ExecuteProcedure();
            if (db.HasData)
                Id = (long)db.Response.Rows[0][0];
        }

        public void Insert()
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "InsertNarrativeObject";
            db.Parameters.Add(@"@guid", Guid);
            db.Parameters.Add(@"scan_object_id", ScanObject.Id);
            db.Parameters.Add(@"created", Created);
            db.ExecuteProcedure();
        }

        private void GenerateScreenShot(Guid guid, PhantomJSDriver driver)
        {
            try
            {
                if (driver.PageSource != "<html><head></head><body></body></html>"
                    && !driver.PageSource.Contains("Congratulations. This browser is configured to use Tor.")
                    && !driver.PageSource.Contains("reddit404a.png")
                    && !driver.PageSource.Contains("403 Forbidden")
                    && !driver.PageSource.Contains("404 Not Found"))
                {
                    //temporary until I can figure how the f* to get the bytearray to write to a stream
                    driver.GetScreenshot().SaveAsFile(String.Format(@"C:\torimages\x{0}.tiff", guid.ToString()), System.Drawing.Imaging.ImageFormat.Tiff);
                    NewMessage(String.Format("Image saved of {0}", ScanObject.Name), false);
                }
                else
                {
                    NewMessage(String.Format("{0} Contains invalid html. No Screenshot avialable.", this.ScanObject.Name), true);
                }
            }
            catch
            {
                ScanObject.Description = "Error while creating screenshot - Aborting...";
                ScanObject.PageStatus = "ERROR";
                ScanObject.Update();
            }
        }


        public void NewMessage(string message, bool error)
        {
            CurrentMessage = message;
            Message _message = new Message(this.Id, false, message);
            _message.UploadNarrativeMessage();
            Console.WriteLine(message);
        }

    }
}
