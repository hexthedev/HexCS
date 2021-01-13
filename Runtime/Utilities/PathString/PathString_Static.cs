namespace HexCS.Core
{
    public partial class PathString
    {
        /// <summary>
        /// The forward slash character used as a path separator. Used on unix-like platforms
        /// </summary>
        public const char cUnixPathSeparator = '/';

        /// <summary>
        /// The backslash operator used as a path separator. Used on windows platforms
        /// </summary>
        public const char cWindowsPathSeparator = '\\';

        #region Public Static API
        /// <summary>
        /// PathStrings can be implicitly used as strings. This is a shorthand for calling
        /// the ToString() function
        /// </summary>
        /// <param name="pathString"></param>
        public static implicit operator string(PathString pathString) => pathString.ToString();

        /// <summary>
        /// Automatically converts a string to a PathString by inferring the Separator. 
        /// See PathString(string) for more info
        /// </summary>
        /// <param name="pathString"></param>
        public static implicit operator PathString(string pathString) =>  new PathString(pathString);

        public static PathString operator + (PathString p1, PathString p2) => p1.InsertAtEnd(p2);
        #endregion 

    }
}
