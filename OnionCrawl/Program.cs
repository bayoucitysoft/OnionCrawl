using OnionCrawl.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            PhantomTorDriver crawler = new PhantomTorDriver();
            ScanObject potential = new ScanObject().FindById(1);
            NarrativeObject narrative = new NarrativeObject(crawler, potential);

            Console.WriteLine(potential.Name);
            Console.ReadLine();

        }
    }
}
