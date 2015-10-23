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
        public DateTime Create { get; set; }
        public List<Message> MessageCollection { get; set; }
        public Guid Guid { get; set; }

        public Exception Ex { get; set; }

        public PhantomTorDriver Crawler { get; set; }
        public ScanObject ScanObject { get; set; }

        public NarrativeObject(PhantomTorDriver cralwer, ScanObject scanObject)
        {
            Create = DateTime.Now;
            MessageCollection = new List<Message>();
            Guid = Guid.NewGuid();
            Crawler = cralwer;
            ScanObject = scanObject;

            BeginNarrative();
        }

        private void BeginNarrative()
        {
            //First initial contact needs to be made
            try
            {
                Crawler.Driver.Navigate().GoToUrl(ScanObject.Url);
                NewMessage(String.Format("Contact made with : {0}", Crawler.Driver.Title), false);
                Console.WriteLine(CurrentMessage);
                NewMessage(String.Format("Updating Scan Object..."), false);
                ScanObject.Description = Crawler.Driver.Title;
                ScanObject.PageSource = Crawler.Driver.PageSource;
                ScanObject.screenshot = Crawler.Driver.GetScreenshot().AsByteArray;
                GenerateScreenShot(Crawler.Driver);
                Crawler.Driver.GetScreenshot().SaveAsFile(String.Format(@"C:\{0}.png", ScanObject.Name), System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine();
            }
            catch
            {

            }
        }

        private void GenerateScreenShot(OpenQA.Selenium.PhantomJS.PhantomJSDriver _driver)
        {
            try
            {

            }
            catch
            {

            }
        }


        private void NewMessage(string message, bool error)
        {
            CurrentMessage = message;
            Message _message = new Message(this.Id, false, message);
            _message.UploadNarrativeStep();
        }

    }
}
