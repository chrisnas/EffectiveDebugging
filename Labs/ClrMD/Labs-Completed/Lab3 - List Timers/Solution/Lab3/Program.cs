using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Diagnostics.Runtime;
using Shared;

namespace Lab3
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

                Dictionary<string, TimerStat> stats = new Dictionary<string, TimerStat>(64);
                int totalCount = 0;
                foreach (var timer in EnumerateTimers(clr).OrderBy(t => t.Period))
                {
                    totalCount++;

                    string line = string.Intern(GetTimerString(timer));
                    string key = string.Intern(string.Format(
                        "@{0,8} ms every {1,8} ms | {2} ({3}) -> {4}",
                        timer.DueTime.ToString(),
                        (timer.Period == 4294967295) ? "  ------" : timer.Period.ToString(),
                        timer.StateAddress.ToString("X16"),
                        timer.StateTypeName,
                        timer.MethodName
                    ));

                    TimerStat stat;
                    if (!stats.ContainsKey(key))
                    {
                        stat = new TimerStat()
                        {
                            Count = 0,
                            Line = line,
                            Period = timer.Period
                        };
                        stats[key] = stat;
                    }
                    else
                    {
                        stat = stats[key];
                    }
                    stat.Count = stat.Count + 1;

                    Console.WriteLine(line);
                }

                // create a summary
                Console.WriteLine("\r\n " + totalCount.ToString() + " timers\r\n-----------------------------------------------");
                foreach (var stat in stats.OrderBy(kvp => kvp.Value.Count))
                {
                    Console.WriteLine(string.Format(
                        "{0,4} | {1}",
                        stat.Value.Count.ToString(),
                        stat.Value.Line
                    ));
                }

            }
        }

        private static IEnumerable<TimerInfo> EnumerateTimers(ClrRuntime clr)
        {
            var timerQueueType = Utils.GetMscorlib(clr).GetTypeByName("System.Threading.TimerQueue");
            if (timerQueueType == null)
                yield break;

            ClrStaticField instanceField = timerQueueType.GetStaticFieldByName("s_queue");
            if (instanceField == null)
                yield break;

            var heap = clr.Heap;
            foreach (ClrAppDomain domain in clr.AppDomains)
            {
                ulong? timerQueue = (ulong?)instanceField.GetValue(domain);
                if (!timerQueue.HasValue || timerQueue.Value == 0)
                    continue;

                ClrType t = heap.GetObjectType(timerQueue.Value);
                if (t == null)
                    continue;

                // m_timers is the start of the list of TimerQueueTimer
                var currentPointer = Utils.GetFieldValue(heap, timerQueue.Value, "m_timers");

                while ((currentPointer != null) && (((ulong)currentPointer) != 0))
                {
                    // currentPointer points to a TimerQueueTimer instance
                    ulong currentTimerQueueTimerRef = (ulong)currentPointer;

                    TimerInfo ti = new TimerInfo()
                    {
                        TimerQueueTimerAddress = currentTimerQueueTimerRef
                    };

                    var val = Utils.GetFieldValue(heap, currentTimerQueueTimerRef, "m_dueTime");
                    ti.DueTime = (uint)val;
                    val = Utils.GetFieldValue(heap, currentTimerQueueTimerRef, "m_period");
                    ti.Period = (uint)val;
                    val = Utils.GetFieldValue(heap, currentTimerQueueTimerRef, "m_canceled");
                    ti.Cancelled = (bool)val;
                    val = Utils.GetFieldValue(heap, currentTimerQueueTimerRef, "m_state");
                    ti.StateTypeName = "";
                    if (val == null)
                    {
                        ti.StateAddress = 0;
                    }
                    else
                    {
                        ti.StateAddress = (ulong)val;
                        var stateType = heap.GetObjectType(ti.StateAddress);
                        if (stateType != null)
                        {
                            ti.StateTypeName = stateType.Name;
                        }
                    }

                    // decypher the callback details
                    val = Utils.GetFieldValue(heap, currentTimerQueueTimerRef, "m_timerCallback");
                    if (val != null)
                    {
                        ulong elementAddress = (ulong)val;
                        if (elementAddress == 0)
                            continue;

                        var elementType = heap.GetObjectType(elementAddress);
                        if (elementType != null)
                        {
                            if (elementType.Name == "System.Threading.TimerCallback")
                            {
                                ti.MethodName = Utils.BuildTimerCallbackMethodName(clr, heap, elementAddress);
                            }
                            else
                            {
                                ti.MethodName = "<" + elementType.Name + ">";
                            }
                        }
                        else
                        {
                            ti.MethodName = "{no callback type?}";
                        }
                    }
                    else
                    {
                        ti.MethodName = "???";
                    }

                    yield return ti;

                    currentPointer = Utils.GetFieldValue(heap, currentTimerQueueTimerRef, "m_next");
                }
            }
        }

        private static string GetTimerString(TimerInfo timer)
        {
            return string.Format(
                "0x{0} @{1,8} ms every {2,8} ms |  {3} ({4}) -> {5}",
                timer.TimerQueueTimerAddress.ToString("X16"),
                timer.DueTime.ToString(),
                (timer.Period == 4294967295) ? "  ------" : timer.Period.ToString(),
                timer.StateAddress.ToString("X16"),
                timer.StateTypeName,
                timer.MethodName
            );
        }

    }

    class TimerStat
    {
        public uint Period { get; set; }
        public String Line { get; set; }
        public int Count { get; set; }
    }

    class TimerInfo
    {
        public ulong TimerQueueTimerAddress { get; set; }
        public uint DueTime { get; set; }
        public uint Period { get; set; }
        public bool Cancelled { get; set; }
        public ulong StateAddress { get; set; }
        public string StateTypeName { get; set; }
        public ulong ThisAddress { get; set; }
        public string MethodName { get; set; }
    }
}
