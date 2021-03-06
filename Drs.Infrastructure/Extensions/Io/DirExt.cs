﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.VisualBasic.FileIO;

namespace Drs.Infrastructure.Extensions.Io
{
    public static class DirExt
    {
        public static void ForceDeleteFolder(string sPath, int iTries = 3)
        {
            var isDone = false;
            while (iTries >= 0 && isDone == false)
            {
                try
                {
                    Directory.Delete(sPath, true);
                    isDone = true;
                }
                catch (Exception)
                {
                    isDone = false;
                    Thread.Sleep(250);
                }
                iTries--;
            }
        }

        public static void ForceCopyFolder(string sPathSource, string sPathDest, int iTries = 3)
        {
            var isDone = false;
            while (iTries >= 0 && isDone == false)
            {
                try
                {
                    FileSystem.CopyDirectory(sPathSource, sPathDest, true);
                    isDone = true;
                }
                catch (Exception)
                {
                    isDone = false;
                    Thread.Sleep(250);
                }
                iTries--;
            }
        }

        public static void ForceDeleteFile(string fileName, int iTries = 3)
        {
            var isDone = false;
            while (iTries >= 0 && isDone == false)
            {
                try
                {
                    File.Delete(fileName);
                    isDone = true;
                }
                catch (Exception)
                {
                    isDone = false;
                    Thread.Sleep(250);
                }
                iTries--;
            }
        }

        public static void CreateDirectoryIfNotExist(this string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }

        public static string GetCurrentDirectory()
        {
            var codeBase = Assembly.GetCallingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static void ForceToCreateFile(string fileCode)
        {
            File.Create(fileCode);
        }
    }
}
