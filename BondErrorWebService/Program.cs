using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace BondErrorWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = WebApp.Start<Startup>("http://localhost:9990");
            Console.WriteLine("Type 'quit' and then Enter to stop");
            var entered = Console.ReadLine();
            while (true)
            {
                if (string.Equals(entered, "quit", StringComparison.InvariantCultureIgnoreCase))
                {
                    Publisher.Stop();
                    app.Dispose();
                    return;
                }
                entered = Console.ReadLine();
            }
        }
    }
}
