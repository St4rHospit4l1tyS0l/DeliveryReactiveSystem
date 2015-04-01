using System;
using System.IO;
using System.Reflection;

namespace Drs.Infrastructure.Extensions.Io
{
    public static class UriExtension
    {
        public static string AbsolutePathRelativeToEntryPointLocation(this string relativePath)
        {
            var entryPoint = Assembly.GetEntryAssembly();
            var basePath = Path.GetDirectoryName(entryPoint.Location);
            if (basePath == null) return relativePath;
            var combinedPath = Path.Combine(basePath, relativePath);
            var canonicalPath = Path.GetFullPath(combinedPath);

            return canonicalPath;
        }
    }
}
