using HexCS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace HexCS.Core
{
    /// <summary>
    /// <para> Because of the complexity that comes from parsing string paths, 
    /// I've decided to create a PathString class that contains many common 
    /// utilities for better handling paths in code. </para>
    /// 
    /// Paths are not thread safe. They all use the same StringBuilder to write strings
    /// </summary>
    public partial class PathString : IEquatable<PathString>, IEnumerable
    {
        private const string cIndexLessThanZeroErrorMessage = "Index must be > 0";
        private const string cIndexGreaterThanLengthErrorMessage = "Index greater than path length";
        private const string cInvalidArgErrorMessage = "Steps must be provided as a path string, so steps separated by / or \\";

        private string[] _path; // this is the cached path in steps

        #region Public Instance API
        /// <summary>
        /// This is character used to separate each element in path when writting out a path. 
        /// If the path is created using a string, this Separator is inferred when the PathString is
        /// created. If the path is created using a string[], then the Path.DirectorySeparatorChar is used. 
        /// This can be changed manually to force paths to use a specific separator. 
        /// </summary>
        public char Separator { get; set; } = cWindowsPathSeparator;

        /// <summary>
        /// Is the current path empty. Meaning there are no steps
        /// </summary>
        public bool IsEmpty => _path == null || _path.Length == 0;

        /// <summary>
        /// Returns the path length
        /// </summary>
        public int Length => _path == null ? 0 : _path.Length;

        /// <summary>
        /// Returns the extension of the path without the leading '.'. If no extension exists returns String.Empty.
        /// If there are multiple extensions (multiple '.' characters) the full extension will be taken. 
        /// example.test.file will return (test.file)
        /// </summary>
        public string Extension {
            get
            {
                if (IsEmpty) return string.Empty;

                string end = _path[_path.Length-1];
                int extensionIndex = end.IndexOf(".");
                if (extensionIndex == -1 || end.Length <= extensionIndex + 1) return string.Empty;
                return end.Substring(extensionIndex + 1);
            }
        }

        /// <summary>
        /// returns start step or null is path is empty
        /// </summary>
        public string Start => IsEmpty ? null : _path[0];

        /// <summary>
        /// returns last step or null if path is empty
        /// </summary>
        public string End => IsEmpty ? null : _path[Length-1];


        /// <summary>
        /// Get the path step at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Index out of range</exception>
        public string this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException(cIndexLessThanZeroErrorMessage);
                if (index >= _path.Length) throw new IndexOutOfRangeException(cIndexGreaterThanLengthErrorMessage);
                return _path[index];
            }
        }

        /// <summary>
        /// Create a PathString that automatically inferes the PathSeparator by testing against supported 
        /// path separators. Currently the supported path separators are / and \. If no path separator present assumes
        /// separator as Path.DirectorySeparatorChar
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">Path supplied has equal number of / and \\ so separator cannot be inferred</exception>
        public PathString(string path)
        {
            PathAnalysis an = Analyze(path);

            const string cArgErrorMessage = "The supplied string contains equal number / and \\. This is an invalid path and cannot be used in PathString because the path separator cannot be inferred";
            if (!an.Success)
                throw new ArgumentException(cArgErrorMessage);

            Separator = an.Separator;
            _path = an.Path;
        }

        /// <summary>
        /// Returns a PathString with extension added by appending .extesion to last string element
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public PathString AddExtension(string extension)
        {
            if (IsEmpty) throw new IndexOutOfRangeException("Cannot add extension to an empty path");
            PathString newPath = new PathString();
            newPath._path = new string[_path.Length];

            Array.Copy(_path, 0, newPath._path, 0, _path.Length - 1);
            newPath._path[_path.Length - 1] = $"{_path[_path.Length - 1]}.{extension}";

            return newPath;
        }

        /// <summary>
        /// Returns a PathString with extension added by replacing to .extesion on the last string in the path
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public PathString ReplaceExtension(string extension)
        {
            if (IsEmpty) throw new IndexOutOfRangeException("Cannot add extension to an empty path");
            PathString newPath = new PathString();
            newPath._path = new string[_path.Length];

            Array.Copy(_path, 0, newPath._path, 0, _path.Length - 1);

            string lastStep = _path[_path.Length - 1];
            string newLastStep = lastStep.Substring(0, lastStep.IndexOf('.'));
            newPath._path[_path.Length - 1] = $"{newLastStep}.{extension}";

            return newPath;
        }

        /// <summary>
        /// Add multiple steps to the end of path by inferrinf steps split on / or \. Child takes on separator of parent.
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        public PathString InsertAtEnd(string steps)
        {
            PathAnalysis pa = Analyze(steps);
            if (!pa.Success) throw new ArgumentException(cInvalidArgErrorMessage);
            return InsertAtEnd(pa.Path);
        }

        /// <summary>
        /// Add multiple steps to the end of path using another PathString. Child takes on separator of parent.
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        public PathString InsertAtEnd(PathString steps)
        {
            if (steps == null) return this;
            return InsertAtEnd(steps._path);
        }

        /// <summary>
        /// Add multiple steps to the end of path. Child takes on separator of parent.
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        public PathString InsertAtEnd(string[] steps)
        {
            if (steps == null || steps.Length == 0) return this;

            PathString newPath = CreateEmptyClone();

            int addLength = steps.Length;

            newPath._path = new string[_path.Length + addLength];
            Array.Copy(_path, 0, newPath._path, 0, _path.Length);
            Array.Copy(steps, 0, newPath._path, _path.Length, addLength);

            return newPath;
        }

        /// <summary>
        /// Add multiple steps to the end of path. Child takes on separator of parent.
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">steps string is not a valid path string using / or \ as separators</exception>
        public PathString InsertAtStart(string steps)
        {
            PathAnalysis pa = Analyze(steps);
            if (!pa.Success) throw new ArgumentException(cInvalidArgErrorMessage);
            return InsertAtStart(pa.Path);
        }

        /// <summary>
        /// Add multiple steps to the end of path. Child takes on separator of parent.
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        public PathString InsertAtStart(PathString steps)
        {
            if (steps == null) return this;
            return InsertAtStart(steps._path);
        }

        /// <summary>
        /// Add multiple steps to the end of path. Child takes on separator of parent.
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        public PathString InsertAtStart(string[] steps)
        {
            if (steps == null || steps.Length == 0) return this;

            PathString newPath = CreateEmptyClone();

            int addLength = steps.Length;

            newPath._path = new string[_path.Length + addLength];
            Array.Copy(steps, 0, newPath._path, 0, addLength);
            Array.Copy(_path, 0, newPath._path, addLength, _path.Length);

            return newPath;
        }

        /// <summary>
        /// Inserts the provided stesp at the given index. This means the step at index 
        /// will be replaced with the first step in steps, and steps will be inserted
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Path supplied has equal number of / and \\ so separator cannot be inferred</exception>
        public PathString InsertAt(string steps, int index)
        {
            PathAnalysis pa = Analyze(steps);
            if (!pa.Success) throw new ArgumentException(cInvalidArgErrorMessage);
            return InsertAt(pa.Path, index);
        }

        /// <summary>
        /// Inserts the provided stesp at the given index. This means the step at index 
        /// will be replaced with the first step in steps, and steps will be inserted
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        public PathString InsertAt(PathString steps, int index)
        {
            if (steps == null) return this;
            return InsertAt(steps._path, index);
        }

        /// <summary>
        /// Inserts the provided stesp at the given index. This means the step at index 
        /// will be replaced with the first step in steps, and steps will be inserted
        /// </summary>
        /// <param name="steps">steps to add. No validation, so null steps may cause errors</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Index out of range</exception>
        public PathString InsertAt(string[] steps, int index)
        {
            if (index < 0) throw new IndexOutOfRangeException(cIndexLessThanZeroErrorMessage);
            if (index > _path.Length) throw new IndexOutOfRangeException(cIndexGreaterThanLengthErrorMessage);

            if (index == 0) return InsertAtStart(steps);
            if (index == steps.Length) InsertAtEnd(steps);

            if (steps == null || steps.Length == 0) return this;

            PathString newPath = CreateEmptyClone();

            int addLength = steps.Length;
            newPath._path = new string[_path.Length + addLength];


            int preInsertLength = index;
            int insertLength = steps.Length;
            int postInsertLength = _path.Length - preInsertLength;

            int insertStartIndex = index;
            int postInsertIndex = insertStartIndex + steps.Length;

            Array.Copy(_path, 0, newPath._path, 0, preInsertLength);
            Array.Copy(steps, 0, newPath._path, insertStartIndex, insertLength);
            Array.Copy(_path, insertStartIndex, newPath._path, postInsertIndex, postInsertLength);

            return newPath;
        }

        /// <summary>
        /// Returns a new PathString with endstep removed.
        /// If current path is empty, returns null
        /// </summary>
        /// <returns>null if trying to remove from empty path</returns>
        public PathString RemoveAtEnd()
        {
            if (IsEmpty) return null;

            PathString newPath = CreateEmptyClone();

            newPath._path = new string[_path.Length - 1];
            Array.Copy(_path, 0, newPath._path, 0, _path.Length-1);

            return newPath;
        }

        /// <summary>
        /// Removes multiple steps from a path. If count is greater than steps return empty path
        /// </summary>
        /// <param name="count"></param>
        /// <returns>null if trying to remove from empty path</returns>
        public PathString RemoveAtEnd(int count)
        {
            int lengthTest = _path.Length - count;

            if (lengthTest < 0) return null;
            if (lengthTest == 0) return new PathString(string.Empty);

            PathString newPath = CreateEmptyClone();

            int newArraySize = _path.Length - count;
            newPath._path = new string[newArraySize];
            Array.Copy(_path, 0, newPath._path, 0, newArraySize);

            return newPath;
        }

        /// <summary>
        /// Returns a new PathString with start step removed.
        /// If current path is empty, returns null
        /// </summary>
        /// <returns>null if trying to remove from empty path</returns>
        public PathString RemoveAtStart()
        {
            if (IsEmpty) return null;

            PathString newPath = CreateEmptyClone();

            newPath._path = new string[_path.Length - 1];
            Array.Copy(_path, 1, newPath._path, 0, _path.Length - 1);

            return newPath;
        }

        /// <summary>
        /// Returns a new PathString with count start steps removed.
        /// If current path is empty, returns null
        /// </summary>
        /// <returns>null if trying to remove from empty path</returns>
        public PathString RemoveAtStart(int count)
        {
            if (IsEmpty) return null;
            if (count > _path.Length) return null;
            
            PathString newPath = CreateEmptyClone();
            if (count == _path.Length)
            {
                newPath._path = new string[0];
                return newPath;
            }

            int newLength = _path.Length - count;
            newPath._path = new string[newLength];
            Array.Copy(_path, count, newPath._path, 0, newLength);

            return newPath;
        }

        /// <summary>
        /// Returns a new Pathstring between the start index (inclusive) and end index (exclusive)
        /// </summary>
        /// <returns>null if trying to remove from empty path</returns>
        public PathString SubPath(int startIndex, int endIndex)
        {
            const string cIndexErrorMessage = "Endindex must be greater than start index";

            if (endIndex <= startIndex) throw new ArgumentException(cIndexErrorMessage);
            if (startIndex < 0) throw new IndexOutOfRangeException(cIndexLessThanZeroErrorMessage);
            if (endIndex > _path.Length) throw new IndexOutOfRangeException(cIndexGreaterThanLengthErrorMessage);
            if (IsEmpty) return null;

            PathString newPath = CreateEmptyClone();
            int newLength = endIndex - startIndex;
            newPath._path = new string[newLength];
            Array.Copy(_path, startIndex, newPath._path, 0, newLength);

            return newPath;
        }

        /// <summary>
        /// Return index of step in the path, or -1 if step does not exist
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int IndexOf(string step) => Array.IndexOf(_path, step);

        /// <summary>
        /// Returns true if the pathstring contains the step provided. 
        /// This can be used to check if a pathstring can be converted to be relative to a certain step
        /// in the path foo/bar/cer/grt/man, ContainsStep("bar") = true, ContainsStep("gost") = false
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public bool Contains(string step) => IndexOf(step) != -1;

        /// <summary>
        /// Returns a new string path relative to target. This is done
        /// by looking backwards from the end of the path and matching target.
        /// If target does not exist in path, then returns new path of same value. 
        /// 
        /// "foo/bar/foo/cha/mon".RelativeTo("foo") = "cha/mon"
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public PathString RelativeTo(string target)
        {
            int stepIndex = IndexOf(target);
            if (stepIndex == -1) return this;

            PathString newPath = CreateEmptyClone();

            int firstIncludedIndex = stepIndex + 1;
            if (firstIncludedIndex >= _path.Length) return new PathString();

            int newLength = _path.Length - firstIncludedIndex;
            newPath._path = new string[newLength];
            Array.Copy(_path, firstIncludedIndex, newPath._path, 0, newLength);

            return newPath;
        }


        /// <summary>
        /// If the path is a valid directory path and that directory does not yet exist, creates it
        /// </summary>
        /// <returns>false if path was not valid for directory creation</returns>
        public bool CreateIfNotExistsDirectory()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(ToString());

                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// If the path is a valid file path and that file does not yet exist, creates it
        /// </summary>
        /// <returns>false if path was not valid for file creation</returns>
        public bool CreateIfNotExistsFile()
        {
            try
            {
                FileInfo f =  new FileInfo(ToString());

                if (!f.Exists)
                {
                    using (f.Create()) { }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Try get path as a file info or return false
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool TryAsFileInfo(out FileInfo info)
        {
            try
            {
                info = new FileInfo(this);
            }
            catch (Exception)
            {
                info = default;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Try to get the path as a file stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool TryAsFileStream(FileMode mode, out FileStream stream)
        {
            try
            {
                stream = File.Open(ToString(), mode);
                return true;
            } 
            catch (Exception)
            {
                stream = default;
                return false;
            }
        }

        /// <summary>
        /// Try get path as a DirectoryInfo or return false
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool TryAsDirectoryInfo(out DirectoryInfo info)
        {
            try
            {
                info = new DirectoryInfo(this);
            }
            catch (Exception)
            {
                info = default;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the represented path is equal. Paths are equal even if they do not have 
        /// the same separator. It only depends on the value of steps. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PathString other)
        {
            return _path.EqualsElementWise(other._path);
        }

        /// <summary>
        /// Returns the path as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _path?.CombineString(Separator.ToString());
        }

        /// <summary>
        /// Returns the path as a string replacing the Separator with custom separator
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="withExtention"></param>
        /// <returns></returns>
        public string ToString(char separator, bool withExtention = true)
        {
            if(withExtention || Extension == string.Empty) return _path.CombineString(separator.ToString());

            string[] path = _path.ShallowCopy();

            string end = path[path.Length - 1];
            path[path.Length - 1] = end.Substring(0, end.IndexOf('.'));

            return path.CombineString(separator.ToString());
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator() => _path.GetEnumerator();
        #endregion

        private PathString() { }

        private PathString CreateEmptyClone()
        {
            PathString p = new PathString();
            p.Separator = Separator;
            return p;
        }

        private PathAnalysis Analyze(string path)
        {
            // test the most common char
            int unixTest = path.ToCharArray().QueryCount(c => c == cUnixPathSeparator);
            int winTest = path.ToCharArray().QueryCount(c => c == cWindowsPathSeparator);

            if (winTest > unixTest)
                return new PathAnalysis(true, cWindowsPathSeparator, path.Split(cWindowsPathSeparator));
            
            if (unixTest > winTest)
                return new PathAnalysis(true, cUnixPathSeparator, path.Split(cUnixPathSeparator));

            if (unixTest == 0 && winTest == 0)
                return new PathAnalysis(true, Separator, new string[] { path });

            return new PathAnalysis(); // fail
        }


        #region Interal Objects
        private struct PathAnalysis
        {
            public bool Success;
            public char Separator;
            public string[] Path;

            public PathAnalysis(bool success, char separator, string[] path)
            {
                Success = success;
                Separator = separator;
                Path = path;
            }
        }
        #endregion
    }
}
