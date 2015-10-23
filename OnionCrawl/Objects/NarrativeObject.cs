using OnionCrawl.Utility;
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

        private void BeginNarrative()
        {
            try
            {
                this.Insert();
                this.RetreiveId();
                Crawler.Driver.Navigate().GoToUrl(ScanObject.Url);
                NewMessage(String.Format("Contact made with : {0}", Crawler.Driver.Title), false);
                NewMessage(String.Format("Updating Scan Object..."), false);
                ScanObject.Description = Crawler.Driver.Title;
                ScanObject.PageSource = Crawler.Driver.PageSource;
                ScanObject.screenshot = Crawler.Driver.GetScreenshot().AsByteArray;
                GenerateScreenShot(Crawler.Driver);
                ScanObject.PageStatus = "OK";
                ScanObject.Update();
                NewMessage(String.Format("ScanObject Updated"), false);
                FindNewScanObjects(ScanObject.PageSource);
               
            }
            catch
            {

            }
        }

        public void FindNewScanObjects(string source)
        {
            LinkFinder finder = new LinkFinder();
            finder.FindPotentialScans(source);
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

        private void GenerateScreenShot(OpenQA.Selenium.PhantomJS.PhantomJSDriver _driver)
        {
            try
            {
                //temporary until I can figure how the f* to get the bytearray to write to a stream
                _driver.GetScreenshot().SaveAsFile(String.Format(@"Y:\x{0}.png", _driver.Title), System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {

            }
        }


        private void NewMessage(string message, bool error)
        {
            CurrentMessage = message;
            Message _message = new Message(this.Id, false, message);
            _message.UploadNarrativeMessage();
            Console.WriteLine(message);
        }

    }
}
