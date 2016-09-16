using System;
using System.IO;
using System.Linq;

namespace Seek.Checkout.WebApi.Tests.Utils
{
    public static class TestFilesHelper
    {
        public static string ReadFile(string fileName)
        {
            return File.ReadAllText(GetPath(fileName));
        }

        private static string GetPath(string file)
        {
            var startupPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + Path.DirectorySeparatorChar;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var projectPath = string.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 3));
            return Path.Combine(projectPath, "TestFiles", file);
        }
    }
}
