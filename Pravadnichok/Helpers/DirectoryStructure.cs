using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pravadnichok.Model;

namespace Pravadnichok.FileExplorer
{
    public static class DirectoryStructure
    {
        public static List<DirectoryItemModel> GetLogicalDrives()
        {
            return Directory.GetLogicalDrives().Select(drive => new DirectoryItemModel { FullPath = drive, Type = DirectoryItemType.Drive }).ToList();
        }

        public static List<DirectoryItemModel> GetDirectoryContents(string fullPath)
        {
            var items = new List<DirectoryItemModel>();

            // Get directories from the folder
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    items.AddRange(dirs.Select(dir => new DirectoryItemModel { FullPath = dir, Type = DirectoryItemType.Folder }));
            }
            catch { }


            // Get files from the folder
            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                    items.AddRange(fs.Select(file => new DirectoryItemModel { FullPath = Path.Combine(fullPath, file), Type = DirectoryItemType.File }));
            }
            catch { }

            return items;
        }

        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return path;

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }
    }
}
