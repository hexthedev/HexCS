using System;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// CharQueries query a particular index of a string for certain properties. 
    /// These are used in Parsing to determine key indices in the parse string
    /// </summary>
    /// <param name="target"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public delegate bool DCharQuery(ref string target, int index);

    /// <summary>
    /// enumerates common char queries
    /// </summary>
    public enum ECharQuery
    {
        /// <summary>
        /// Is the index an instance of a newline
        /// i.e \r\n Windows, \n Linux. This function also checks backwards
        /// so that \n is not taken when \r\n was already taken.
        /// </summary>
        NewLine = 0,

        /// <summary>
        /// Is the index an instance of a Windows newline i.e \r\n
        /// </summary>
        NewLine_Windows = 1,

        /// <summary>
        /// Is the index an instance of a Linux newline i.e \r\n
        /// </summary>
        NewLine_Linux = 2,

        /// <summary>
        /// Was this character proceeded by \n
        /// </summary>
        StartOfLine = 3
    }

    /// <summary>
    /// Contains helper methods for common CharQueries
    /// </summary>
    public static class UTCharQuery
    {
        /// <summary>
        /// New line character for linux systems
        /// </summary>
        public const char cLinuxNewline = '\n';

        /// <summary>
        /// New line character for windows systems
        /// </summary>
        public static readonly char[] cWindowsNewLine = new char[] { '\r', '\n' };

        /// <summary>
        /// Returns a delegate that performs a query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DCharQuery GetQuery(ECharQuery query)
        {
            switch (query)
            {
                case ECharQuery.NewLine: return NewLine;
                case ECharQuery.NewLine_Windows: return NewLine_Windows;
                case ECharQuery.NewLine_Linux: return NewLine_Linux;
                case ECharQuery.StartOfLine: return StartOfLine;
            }
            return null;
        }

        /// <summary>
        /// Constructs a query that tests if thte indexed string character is an
        /// occurence of a particular character
        /// </summary>
        /// <param name="character"></param>
        public static DCharQuery GetOccurenceOfQuery(char character)
        {
            return (ref string s, int index) =>
            {
                return IsValidIndex(ref s, index) && s[index] == character;
            };
        }

        /// <summary>
        /// Constructs a query that tests if thte indexed string character is an
        /// occurence of a block of characters. This will not count larger blocks.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="blockSize"></param>
        public static DCharQuery GetOccurenceBlockQuery(char character, int blockSize)
        {
            return (ref string s, int index) =>
            {
                if (!IsValidIndex(ref s, index) || blockSize <= 0) return false;
                if (s[index] != character) return false;
                if (index - 1 >= 0 && s[index - 1] == character) return false; // is this the second characte rof a block

                // scan block
                for (int i = index+1; i < index+blockSize; i++)
                {
                    if (i < s.Length && s[i] != character) return false;
                }

                if (index + blockSize < s.Length && s[index + blockSize] == character) return false; // is it a bigger block

                return true;
            };
        }

        private static bool NewLine(ref string target, int index)
        {
            // is valid index
            if (!IsValidIndex(ref target, index)) return false;

            // is it a windows new line?
            if (index < target.Length - 1 && target[index] == cWindowsNewLine[0] && target[index + 1] == cWindowsNewLine[1]) return true; 

            // Is it a linux newline?
            if (target[index] == cLinuxNewline)
            {
                if (index != 0 && target[index - 1] != cWindowsNewLine[0]) return true;
            }

            return false;
        }

        private static bool NewLine_Windows(ref string target, int index)
        {
            // is valid index
            if (!IsValidIndex(ref target, index) || index + 1 >= target.Length) return false;
            // is it a windows new line?
            if (target[index] == cWindowsNewLine[0] && target[index + 1] == cWindowsNewLine[1]) return true;
            return false;
        }

        private static bool NewLine_Linux(ref string target, int index)
        {
            // is valid index
            if (!IsValidIndex(ref target, index) || index - 1 < 0) return false;

            // Is it a linux newline?
            if (target[index] == cLinuxNewline) return true;

            return false;
        }

        private static bool StartOfLine(ref string target, int index)
        {
            // is valid string
            if (!IsValidIndex(ref target, index) || target[index] == cWindowsNewLine[0] || target[index] == cLinuxNewline) return false;

            // is there a \n infront 
            index--;
            if (index >= 0)
            {
                if (target[index] == cLinuxNewline) return true;
            }

            return false;
        }

        private static bool IsValidIndex(ref string target, int index) => index >= 0 && index < target.Length;
    }
}
