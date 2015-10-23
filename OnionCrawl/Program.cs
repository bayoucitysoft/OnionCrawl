using OnionCrawl.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            //PhantomTorDriver crawler = new PhantomTorDriver();
            //ScanObject potential = new ScanObject().FindById(1);
            //NarrativeObject narrative = new NarrativeObject(crawler, potential);

            //Console.WriteLine(potential.Name);
            //crawler.Kill();


            NarrativeObject n = new NarrativeObject();
            string source = File.ReadAllText(@"Y:\scrape_test.txt");
            n.FindNewScanObjects(source);
            Console.ReadLine();

        }
    }
}
