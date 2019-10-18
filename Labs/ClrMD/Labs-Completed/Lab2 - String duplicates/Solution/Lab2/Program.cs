using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Diagnostics.Runtime;
using Shared;

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
            using (var target = Utils.GetTarget(dumpFilename))
            {
                ClrRuntime clr = target.ClrVersions[0].CreateRuntime();

                var strings = ComputeDuplicatedStrings(clr.Heap);
                int totalSize = 0;

                // sort by size taken by the instances of string (UTF16 char takes 2 bytes)
                foreach (var element in strings.OrderBy(s => 2 * s.Value * s.Key.Length))
                {
                    Console.WriteLine(string.Format(
                        "{0,8} {1,12} {2}",
                        element.Value.ToString(),
                        (2 * element.Value * element.Key.Length).ToString(),
                        element.Key.Replace("\n", "## ").Replace("\r", " ##")
                    ));

                    totalSize += 2 * element.Value * element.Key.Length;
                }

                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine($"         {(totalSize / (1024 * 1024)).ToString(),12} MB");
            }
        }

        public static Dictionary<string, int> ComputeDuplicatedStrings(ClrHeap heap)
        {
            var strings = new Dictionary<string, int>(1024 * 1024);

            // never forget to check if it is possible to walk the heap
            if (!heap.CanWalkHeap)
                return null;

            foreach (var address in heap.EnumerateObjectAddresses())
            {
                try
                {
                    var objType = heap.GetObjectType(address);
                    if (objType == null)
                        continue;

                    if (objType.Name != "System.String")
                        continue;

                    var obj = objType.GetValue(address);
                    var s = obj as string;
                    if (!strings.ContainsKey(s))
                    {
                        strings[s] = 0;
                    }

                    strings[s] = strings[s] + 1;
                }
                catch (Exception x)
                {
                    Console.WriteLine(x);
                    // some InvalidOperationException seems to occur  :^(
                }
            }

            return strings;
        }
    }
}
