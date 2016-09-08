using System;
using System.Linq;

namespace DnaAnaliser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine("Use it:\nDnaAnaliser.exe [-F directory with 23AndMe files] | [23AndMe file]\n");
            }
            else
            {
                if (args[0] == "-F")
                {
                    try
                    {
                        var coll = new GenomAnaliseCollection(args[1]);
                        foreach (var Analiser in coll.OrderBy(i => i.Count))
                        {
                            Console.WriteLine($"{Analiser.Name} with {Analiser.Count} Genes");
                            Console.WriteLine(Analiser.DistributionToString(2));
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Can't open files");
                    }
                }
                else
                {
                    try
                    {
                        var Anal = new GenomAnalise(args[0]);
                        Console.WriteLine(Anal.DistributionToString());
                        foreach (var grup in Anal.SplitToChromosones())
                        {
                            Console.WriteLine($"{grup.Key}:");

                            Console.WriteLine($"{grup.Value.DistributionToString(2, false)}");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Can't open a file");
                    }
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}