using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl
{
    public class PhantomTorDriver
    {
        public Process Tor { get; set; }
        string torPath = @"C:\Users\Pax Prose\Desktop\Tor Browser\Browser\firefox.exe";
        PhantomJSDriverService Service { get; set; }
        public PhantomJSDriver Driver { get; set; }
        public bool TorRunning { get { return CheckForTorProcess(); } }
        public Exception Ex { get; set; }

        public PhantomTorDriver()
        {
            StartTorProcess();
            PhantomJSOptions options = new PhantomJSOptions();
            Proxy proxy = new Proxy();
            proxy.SocksProxy = string.Format("127.0.0.1:9150");
            Service = PhantomJSDriverService.CreateDefaultService();
            Service.ProxyType = "socks5";
            Service.Proxy = proxy.SocksProxy;
            Driver = new PhantomJSDriver(Service);

            while (!TorRunning)
            {
                Task.Delay(2000);
            }

            Driver.Navigate().GoToUrl(@"https://check.torproject.org");
            File.WriteAllLines(@"C:\test_dump.txt", new string[] { Driver.PageSource });
            Driver.GetScreenshot().SaveAsFile(@"C:\check.png", ImageFormat.Png);

        }

        private async void StartTorProcess()
        {
            //attempt to try tor .exe process
            if (!Process.GetProcesses().Any(x => x.ProcessName == "tor"))
            {
                Tor = new Process();
                Tor.StartInfo.FileName = torPath;
                Tor.StartInfo.Arguments = "-n";
                Tor.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                await Task.Run((Action)(() =>
                {
                    Tor.Start();
                }));
            }
        }

        private bool CheckForTorProcess()
        {
            return Process.GetProcesses().Any(x => x.ProcessName == "tor");
        }
    }
}
