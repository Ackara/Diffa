using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Acklann.Diffa.Reporters
{
    /// <summary>
    /// Represents Visual Studio's file (diff) comparison tool.
    /// </summary>
    /// <seealso cref="Acklann.Diffa.Reporters.ReporterBase" />
    [Rank(Rating.FREE - 1)]
    public class VisualStudioReporter : ReporterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStudioReporter"/> class.
        /// </summary>
        public VisualStudioReporter() : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStudioReporter"/> class.
        /// </summary>
        /// <param name="shouldPause">if set to <c>true</c> [should pause].</param>
        public VisualStudioReporter(bool shouldPause) : base(GetExecutablePath(), "/diff \"{0}\" \"{1}\"", false)
        {
        }

        private static string _executablePath;

        #region code from ApprovalTests

        /// This following code was taken from
        /// ref: https://github.com/approvals/ApprovalTests.Net/blob/master/ApprovalTests/Reporters/VisualStudioReporter.cs

        internal static string GetExecutablePath()
        {
            if (string.IsNullOrEmpty(_executablePath))
            {
                Process process;
                try
                {
                    var processAndParent = ParentProcessUtils.CurrentProcessWithAncestors();

                    process = processAndParent.FirstOrDefault(x => x.MainModule.FileName.EndsWith("devenv.exe"));
                }
                catch (Exception)
                {
                    // Any exception means we are not working in this environment.

                    return _executablePath = null;
                }

                if (process != null)
                {
                    var processModule = process.MainModule;
                    var version = processModule.FileVersionInfo.FileMajorPart;

                    if (11 <= version)
                    {
                        return _executablePath = processModule.FileName;
                    }
                }

                return _executablePath = null;
            }
            else return _executablePath;
        }

        private static class ParentProcessUtils
        {
            public static Process GetParentProcess(Process currentProcess)

            {
                return ParentProcess(currentProcess);
            }

            public static IEnumerable<Process> CurrentProcessWithAncestors()

            {
                return GetSelfAndAncestors(Process.GetCurrentProcess());
            }

            private static IEnumerable<Process> GetSelfAndAncestors(Process self)

            {
                var processIds = new HashSet<int>();

                var process = self;

                while (process != null)

                {
                    yield return process;

                    if (processIds.Contains(process.Id))

                    {
                        // loop detected (parent id have been re-allocated to a child process!)

                        yield break;
                    }

                    processIds.Add(process.Id);

                    process = ParentProcess(process);
                }
            }

            private static Process ParentProcess(Process process)

            {
                var parentPid = 0;

                var processPid = process.Id;

                const uint TH32_CS_SNAPPROCESS = 2;

                // Take snapshot of processes

                var hSnapshot = CreateToolhelp32Snapshot(TH32_CS_SNAPPROCESS, 0);

                if (hSnapshot == IntPtr.Zero)

                {
                    return null;
                }

                var procInfo = new PROCESSENTRY32 { dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32)) };

                // Read first

                if (Process32First(hSnapshot, ref procInfo) == false)

                {
                    return null;
                }

                // Loop through the snapshot

                do

                {
                    // If it's me, then ask for my parent.

                    if (processPid == procInfo.th32ProcessID)

                    {
                        parentPid = (int)procInfo.th32ParentProcessID;
                    }
                } while (parentPid == 0 && Process32Next(hSnapshot, ref procInfo)); // Read next

                if (parentPid <= 0)

                {
                    return null;
                }

                try

                {
                    return Process.GetProcessById(parentPid);
                }
                catch (ArgumentException)

                {
                    //Process with an Id of X is not running

                    return null;
                }
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessId);

            [DllImport("kernel32.dll")]
            private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

            [DllImport("kernel32.dll")]
            private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

            [StructLayout(LayoutKind.Sequential)]
            private struct PROCESSENTRY32

            {
                public uint dwSize;

                public uint cntUsage;

                public uint th32ProcessID;

                public IntPtr th32DefaultHeapID;

                public uint th32ModuleID;

                public uint cntThreads;

                public uint th32ParentProcessID;

                public int pcPriClassBase;

                public uint dwFlags;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szExeFile;
            };
        }

        #endregion code from ApprovalTests
    }
}