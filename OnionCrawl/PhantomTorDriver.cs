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
        //string torPath = @"C:\Users\dtsadmin\Desktop\Tor Browser\Browser\firefox.exe";
        PhantomJSDriverService Service { get; set; }
        public PhantomJSDriver Driver { get; set; }
        public bool TorRunning { get { return CheckForTorProcess(); } }
        public Exception Ex { get; set; }

        public PhantomTorDriver(string port)
        {
            try
            {
                StartTorProcess();
                PhantomJSOptions options = new PhantomJSOptions();
                Proxy proxy = new Proxy();
                proxy.SocksProxy = string.Format("127.0.0.1:" + port);
                Service = PhantomJSDriverService.CreateDefaultService();
                Service.ProxyType = "socks5";
                Service.Proxy = proxy.SocksProxy;
                Service.HideCommandPromptWindow = true;
                Driver = new PhantomJSDriver(Service);

                while (!TorRunning)
                {
                    Task.Delay(2000);
                }
            }
            catch
            {

            }
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

        internal void Kill()
        {
            this.Driver.Close();
        }
    }
}
