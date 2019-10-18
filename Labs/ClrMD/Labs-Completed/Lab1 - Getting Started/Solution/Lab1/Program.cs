using System;
using System.IO;
using Microsoft.Diagnostics.Runtime;
using Shared;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // args[0] is supposed to be the dump filename or a process id

            if (args.Length != 1)
            {
                Console.WriteLine("A dump filename or a process id must be provided.");
                return;
            }

            var dumpFilename = args[0];
            if (int.TryParse(args[0], out int pid))
            {
                AttachToLiveProcess(pid);
                return;
            }

            if (!File.Exists(dumpFilename))
            {
                Console.WriteLine($"{dumpFilename} does not exist.");
                return;
            }

            ProcessDumpFile(args[0]);
        }

        private static void AttachToLiveProcess(int pid)
        {
            using (var target = DataTarget.AttachToProcess(pid, 2000))
            {
                AnalyzeApplication(target);
            }
        }

        private static void ProcessDumpFile(string dumpFilename)
        {
            using (var target = Utils.GetTarget(dumpFilename))
            {
                AnalyzeApplication(target);
            }
        }

        private static void AnalyzeApplication(DataTarget target)
        {
            try
            {
                ClrRuntime clr = null;

                // get the bitness of the application in two different ways
                Console.WriteLine(target.Architecture.ToString());
                Console.WriteLine((target.PointerSize == 4) ? "32 bit" : "64 bit");

                // At this point, it is expected that the current tool and the dump are sharing the same bitness
                // or an exception will be thrown

                // if you were able to get the mscordacwks.dll of the machine on which the dump
                // was taken, pass it to the CreateRuntime(string) overwrite
                clr = target.ClrVersions[0].CreateRuntime();

                // get the version of the CLR in two different ways
                Console.WriteLine(target.ClrVersions[0].Version.ToString());
                Console.WriteLine(clr.ClrInfo.Version.ToString());

                ListLoadedAssemblies(clr);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }

        private static void ListLoadedAssemblies(ClrRuntime clr)
        {
            // it is possible to get the list of assembly files by using the DataTarget itself
            foreach (var module in clr.DataTarget.EnumerateModules())
            {
                // skip native dlls
                if (!module.IsManaged)
                    continue;

                Console.WriteLine(module.FileName);
            }

            // ... or use the ClrRuntime instance
            // (remember that an assembly can contain several modules in .NET)
            foreach (var module in clr.Modules)
            {
                // note that the filenames are not the "really" loaded ones!
                // --> based on the references stored in the imports of the application
                Console.WriteLine(module.AssemblyName);
                // use module.FileName if you expect multi-modules assemblies

                // don't know why there is no .NET assembly details such as version  :^(
            }
        }
    }
}
