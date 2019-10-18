using System;
using System.IO;
using Microsoft.Diagnostics.Runtime;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            // args[0] is supposed to be the dump filename

            if (args.Length != 1)
            {
                Console.WriteLine("A dump filename must be provided.");
                return;
            }

            var dumpFilename = args[0];
            if (!File.Exists(dumpFilename))
            {
                Console.WriteLine($"{dumpFilename} does not exist.");
                return;
            }

            ProcessDumpFile(args[0]);
        }

        private static void ProcessDumpFile(string dumpFilename)
        {
            // TODO
        }
    }
}
