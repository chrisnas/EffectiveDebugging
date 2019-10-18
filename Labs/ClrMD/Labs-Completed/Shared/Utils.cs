using System.IO;
using Microsoft.Diagnostics.Runtime;

namespace Shared
{
    public static class Utils
    {
        public static DataTarget GetTarget(string dumpFilename)
        {
            var sympath = SetupSymbols();

            var target = DataTarget.LoadCrashDump(dumpFilename);
            target.SymbolLocator.SymbolPath = sympath;

            return target;
        }

        private static string SetupSymbols()
        {
            // symbols/images will be cached in c:\symbols
            var localCacheFolder = @"c:\symbols";
            if (!Directory.Exists(localCacheFolder))
            {
                Directory.CreateDirectory(localCacheFolder);
            }

            return $"SRV*{localCacheFolder}*http://msdl.microsoft.com/download/symbols";
        }

        public static object GetFieldValue(ClrHeap heap, ulong address, string fieldName)
        {
            var type = heap.GetObjectType(address);
            ClrInstanceField field = type.GetFieldByName(fieldName);
            return field?.GetValue(address);
        }

        public static string BuildTimerCallbackMethodName(ClrRuntime clr, ClrHeap heap, ulong timerCallbackRef)
        {
            var methodPtr = GetFieldValue(heap, timerCallbackRef, "_methodPtr");
            if (methodPtr != null)
            {
                ClrMethod method = clr.GetMethodByAddress((ulong)(long)methodPtr);
                if (method != null)
                {
                    // look for "this" to figure out the real callback implementor type
                    string thisTypeName = "?";
                    var thisPtr = GetFieldValue(heap, timerCallbackRef, "_target");
                    if ((thisPtr != null) && ((ulong)thisPtr) != 0)
                    {
                        ulong thisRef = (ulong)thisPtr;
                        var thisType = heap.GetObjectType(thisRef);
                        if (thisType != null)
                        {
                            thisTypeName = thisType.Name;
                        }
                    }
                    else
                    {
                        thisTypeName = (method.Type != null) ? method.Type.Name : "?";
                    }
                    return $"{thisTypeName}.{method.Name}";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static ClrModule GetMscorlib(ClrRuntime clr)
        {
            foreach (ClrModule module in clr.Modules)
                if (module.AssemblyName.Contains("mscorlib.dll"))
                    return module;

            // Uh oh, this shouldn't have happened.  Let's look more carefully (slowly).
            foreach (ClrModule module in clr.Modules)
                if (module.AssemblyName.ToLower().Contains("mscorlib"))
                    return module;

            // Ok...not sure why we couldn't find it.
            return null;
        }

    }
}
