using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Drs.Infrastructure.Extensions.Proc
{
    public static class ProcessExt
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        public static void ForceKillProcess(string sProcessName, int iTries = 4)
        {
            while (iTries >= 0)
            {
                try
                {
                    var processes = Process.GetProcessesByName(sProcessName);

                    if (processes.Length == 0)
                        break;

                    foreach (var process in processes)
                    {
                        process.Kill();
                    }

                    Thread.Sleep(500);
                }
                catch (Exception)
                {
                    Thread.Sleep(250);
                }
                iTries--;
            }
        }

        public static Process ForceStartProcess(string sPathFile, string sFileName, string sProcessName, bool bIsPos, int iTries = 4)
        {
            while (iTries >= 0)
            {
                try
                {
                    var processes = Process.GetProcessesByName(sProcessName);

                    if (processes.Length > 0)
                        return processes[0];


                    var process = new Process
                    {
                        StartInfo =
                        {
                            FileName = Path.Combine(sPathFile, sFileName),
                            WorkingDirectory = sPathFile,
                            UseShellExecute = false,
                            //WindowStyle = ProcessWindowStyle.Normal
                        }
                    };

                    if (bIsPos)
                    {
                        process.StartInfo.EnvironmentVariables["AlohaLeft"] = (SystemParameters.PrimaryScreenWidth * (0.15625)).ToString(CultureInfo.InvariantCulture);
                        process.StartInfo.EnvironmentVariables["AlohaXRes"] = (SystemParameters.PrimaryScreenWidth * (0.52083)).ToString(CultureInfo.InvariantCulture);
                        process.StartInfo.EnvironmentVariables["AlohaTop"] = (SystemParameters.PrimaryScreenHeight * (0.125)).ToString(CultureInfo.InvariantCulture);
                        process.StartInfo.EnvironmentVariables["AlohaYRes"] = (SystemParameters.PrimaryScreenHeight * (0.74074)).ToString(CultureInfo.InvariantCulture);
                    }
                    
                    process.Start();

                    //Process.Start(sExeFile);
                    Thread.Sleep(1000);

                    processes = Process.GetProcessesByName(sProcessName);

                    if (processes.Length > 0)
                        return process;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(250);
                }
                iTries--;
            }

            return null;
        }

        public static void ForceSetFocusApp(string sProcessName, int iTries = 3)
        {
            while (iTries > 0)
            {
                try
                {
                    var process = Process.GetProcessesByName(sProcessName).FirstOrDefault();

                    if (process == null)
                        return;

                    var hWnd = process.MainWindowHandle;
                    if (hWnd == IntPtr.Zero)
                        return;
                    SetForegroundWindow(hWnd);
                    ShowWindow(hWnd, 5);
                    break;
                }
                catch (Exception)
                {
                    iTries--;
                }
            }
        }

        public static void ForceSetFocusThisApp()
        {
            var currentProcess = Process.GetCurrentProcess();
            var hWnd = currentProcess.MainWindowHandle;
            if (hWnd == IntPtr.Zero) return;
            SetForegroundWindow(hWnd);
            ShowWindow(hWnd, 5);
        }

        public static bool ProcessIsRunning(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            return process != null;
        }
    }
}
