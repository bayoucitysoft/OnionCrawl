using OnionCrawl.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnionCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Task<bool> run1 = StartNewAsyncScan("9150");
                while (!run1.Result)
                    Task.Delay(1000);
                Main(new string[] { });
            }
            catch
            {
                GC.WaitForPendingFinalizers();
                Main(new string[] { });
            }
        }

        private async static Task<bool> StartNewAsyncScan(string port)
        {
            await Task.Run((Action)(() =>
                {
                    PhantomTorDriver crawler = new PhantomTorDriver(port);
                    ScanObject potential = new ScanObject().GetNext();
                    NarrativeObject narrative = new NarrativeObject(crawler, potential);
                    crawler.Driver.Quit();
                    narrative.FindPotentialScans();
                }));
            return true;
        }
    }
}
