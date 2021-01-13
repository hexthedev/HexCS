using HexCS.Core;
using System.Collections.Generic;
using System.IO;

namespace HexCS.Core
{
    /// <summary>
    /// Helpers and extentions for PathStrings
    /// </summary>
    public static class UTPathString
    {
        /// <summary>
        /// <para>Searches the children files of a path string and returns an array pathstrings pointing
        /// to all files with an extention in the extenions array. If recurisive = true, then the search will
        /// continue to children folders (and so on). If the original path is not a directory, an empty
        /// array will be returned</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        /// <param name="extentions"></param>
        /// <returns></returns>
        public static PathString[] ChildrenWithExtention(this PathString path, bool recursive, params string[] extentions)
        {
            if (!path.TryAsDirectoryInfo(out DirectoryInfo info)) return new PathString[] { };

            List<PathString> pathStrings = new List<PathString>();

            foreach(FileInfo f in info.GetFiles())
            {
                PathString filePath = new PathString(f.FullName);

                // if no extentions provided, then no tests necessary. Take all files
                if (extentions == null || extentions.Length == 0) pathStrings.Add(filePath);

                // check for valid extention
                if (filePath.Extension != null && extentions.QueryContains(s => s == filePath.Extension)) pathStrings.Add(filePath);
            }

            if (recursive)
            {
                DirectoryInfo[] dirs = info.GetDirectories();

                if(dirs.Length > 0)
                {
                    Queue<DirectoryInfo> directories = new Queue<DirectoryInfo>(dirs);

                    while(directories.Count > 0)
                    {
                        DirectoryInfo process = directories.Dequeue();
                        pathStrings.AddRange(new PathString(process.FullName).ChildrenWithExtention(true, extentions));
                    }
                }
            }

            return pathStrings.ToArray();
        }

        /// <summary>
        /// Calls CreateIfNotExistsFile on an array of PathStrings
        /// </summary>
        /// <param name="paths"></param>
        public static void CreateIfNotExistFile(params PathString[] paths)
        {
            foreach(PathString path in paths)
            {
                path.CreateIfNotExistsFile();
            }
        }

        /// <summary>
        /// Calls CreateIfNotExistsDirectory on an array of PathStrings
        /// </summary>
        /// <param name="paths"></param>
        public static void CreateIfNotExistDirectory(params PathString[] paths)
        {
            foreach (PathString path in paths)
            {
                path.CreateIfNotExistsDirectory();
            }
        }
    }
}
