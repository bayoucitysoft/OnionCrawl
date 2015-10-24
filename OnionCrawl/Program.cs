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
                Task.Delay(600);
                Task<bool> run2 = StartNewAsyncScan("9151");
                Task.Delay(600);
                Task<bool> run3 = StartNewAsyncScan("9152");
                Task.Delay(600);
                Task<bool> run4 = StartNewAsyncScan("9153");
                Task.Delay(600);
                if (run1.Result == false | run2.Result == false | run3.Result == false | run4.Result == false)
                {
                    Task.Delay(10000);
                }
                Main(new string[] { });
            }
            catch
            {
                Process restart = new Process();
                
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
