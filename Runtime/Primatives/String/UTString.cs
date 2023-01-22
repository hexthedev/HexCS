using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HexCS.Core
{
    /// <summary>
    /// Class that contains string manipulation utilities
    /// </summary>
    public static class UTString
    {
        /// <summary>
        /// Enforce that a string has only the first letter captialized
        /// </summary>
        /// <param name="target">target string</param>
        /// <returns>string with only first char capital</returns>
        public static string EnforceFistCharCaptialOnly(this string target)
        {
            return $"{char.ToUpper(target[0])}{target.Substring(1).ToLower()}";
        }

        /// <summary>
        /// Enforce that the first character in the string is captital. Other characters are ignored
        /// </summary>
        /// <param name="target">target string</param>
        /// <returns></returns>
        public static string EnforceFistCharCaptial(this string target)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(char.ToUpper(target[0]));
            sb.Append(target.Substring(1, target.Length - 1));

            return sb.ToString();
        }

        /// <summary>
        /// Enforce that a string has only the first letter lowercase
        /// </summary>
        /// <param name="target">target string</param>
        /// <returns></returns>
        public static string EnforceFistCharLowerCaseOnly(this string target)
        {
            return $"{char.ToLower(target[0])}{target.Substring(1).ToUpper()}";
        }

        /// <summary>
        /// Enforce that the first character in the string is lowercase. Other characters are ignored
        /// </summary>
        /// <param name="target">target string</param>
        /// <returns></returns>
        public static string EnforceFirstCharLowerCase(this string target)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(char.ToLower(target[0]));
            sb.Append(target.Substring(1, target.Length - 1));

            return sb.ToString();
        }

        /// <summary>
        /// Takes an array of words and makes them snake case
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string EnforceSnakeCase(params string[] words)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                sb.Append(words[i].ToLower());

                if (i != words.Length - 1)
                {
                    sb.Append("_");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Takes an array of words and makes them camel case
        /// </summary>
        /// <param name="isFirstCapital"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string EnforceCamelCase(bool isFirstCapital, params string[] words)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0 && !isFirstCapital)
                {
                    sb.Append(words[i].ToLower());
                }
                else
                {
                    sb.Append(words[i].EnforceFistCharCaptialOnly());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string that repears another string a numer of times
        /// </summary>
        /// <param name="character"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string RepeatedCharacter(string character, int times)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < times; i++)
            {
                sb.Append(character);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Taks an array of strings and combines them into a single string by placing a
        /// combiner string between each element. 
        /// 
        /// Example: {"a", "b" "c"}.CombineString(".") = "a.b.c"
        /// </summary>
        /// <param name="split"></param>
        /// <param name="combiner"></param>
        /// <returns></returns>
        public static string CombineString(this string[] split, string combiner)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < split.Length; i++)
            {
                sb.Append(split[i]);
                if (i < split.Length - 1) sb.Append(combiner);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Splits a string based on a separator, then reconstructs the string up to the prune level.
        /// If prune level lessthaneq 0, then no prune is performed and the original string is returned
        /// If the prune level is greaterthaneq the number of  element sin the split string, returns the original string
        /// Example: "a.b.c.d".SeparatorLevelPrune(".", 2) is "a.b"
        /// </summary>
        /// <param name="original">The original string</param>
        /// <param name="separator">the string to split the original with</param>
        /// <param name="pruneLevel">the prune level used in string reconstruction</param>
        /// <returns></returns>
        public static string SeparatorLevelPrune(this string original, char separator, int pruneLevel)
        {
            if (pruneLevel <= 0) return original;

            string[] split = original.Split(separator);
            if (split.Length <= pruneLevel) return original;

            string[] levels = split.SubArray(0, pruneLevel);
            return levels.CombineString($"{separator}");
        }

        /// <summary>
        /// <para>Returns the string that exists between start and end. It does not include the start and end strings </para>
        /// </summary>
        /// <param name="original"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string SubStringBetween(this string original, string start, string end)
        {
            int startIndex = original.IndexOf(start);
            if (startIndex == -1) return string.Empty;

            int endIndex = original.IndexOf(end);
            if (endIndex <= startIndex) return string.Empty;

            return original.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        /// <summary>
        /// Uses a Regex.Split() to get the sprint as an array of lines
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string[] GetLines(this string original) => Regex.Split(original, "\r\n|\r|\n");

        public static bool SubStringMatch(this string target, int index, string check)
        {
            for (int i = 0; i < check.Length; i++)
            {
                int targetIndex = i + index;
                
                if (targetIndex >= target.Length)
                    return false;

                if (target[targetIndex] != check[i])
                    return false;
            }

            return true;
        }
        
        // Scans take a string reference and index reference, and move the index forward in the string
        // until some condition is met
        #region Scans
        /// <summary>
        /// Move the index until the first character index that passes all of the char queries
        /// provided in the query array. If query array is null or empty, sets index to last index in string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="queries"></param>
        public static void ScanUntilAll(ref string s, ref int index, params DCharQuery[] queries)
        {
            if (queries == null || queries.Length == 0) index = s.Length;

            for (; index < s.Length; index++)
            {
                bool isComplete = true;

                foreach (DCharQuery q in queries)
                {
                    if (!q(ref s, index))
                    {
                        isComplete = false;
                        break;
                    }
                }

                if (isComplete) break;
            }
        }

        /// <summary>
        /// Move the index until the first character index that passes one of the char queries
        /// provided in the query array. If query array is null or empty, sets index to last index in string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="queries"></param>
        public static void ScanUntilOne(ref string s, ref int index, params DCharQuery[] queries)
        {
            if (queries == null || queries.Length == 0) index = s.Length;

            for (; index < s.Length; index++)
            {
                bool isComplete = false;

                foreach(DCharQuery q in queries)
                {
                    if (q(ref s, index))
                    {
                        isComplete = true;
                        break;
                    }
                }

                if (isComplete) break;
            }
        }

        /// <summary>
        /// Move the index until the first character index that passes the char query
        /// provided in the query array. If query array is null or empty, sets index to last index in string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="query"></param>
        public static void ScanUntil(ref string s, ref int index, DCharQuery query)
        {
            if (query == null) index = s.Length;

            for (; index < s.Length; index++)
            {
                if (query(ref s, index))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Moves the index to the index of the next line start. This is the character following /n. It is possible
        /// that the index returned will be outside of the string index bounds. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        public static void ScanToNewLineStart(ref string s, ref int index)
        {
            ScanUntil(ref s, ref index, UTCharQuery.GetOccurenceOfQuery(UTCharQuery.cLinuxNewline));
            index++;
        }
        #endregion
        
        // Character loops perform a function on each character of a string until some condition is met (i.e. Line end)
        #region Character Loops
        /// <summary>
        /// Performs an aciton on every character of a string automatically stopping at the end of the string. 
        /// If the action returns true at any point, the loop will stop.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex"></param>
        /// <param name="characterAction"></param>
        public static void LoopCharacterUntilStringEnd(ref string s, int startIndex, Func<char, bool> characterAction)
        {
            int maxLookForward = s.Length - startIndex;

            for (; maxLookForward > 0; maxLookForward--)
            {
                if (characterAction(s[startIndex])) return;
                startIndex++;
            }
        }

        /// <summary>
        /// Performs an action on every character of a string automatically stopping at the end of the next line
        /// If the action returns true at any point, the loop will stop. This loops includes the /r/n or /n characters
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex"></param>
        /// <param name="characterAction"></param>
        public static void LoopCharacterUntilLine(ref string s, int startIndex, Func<char, bool> characterAction)
        {
            int endLine = startIndex;
            ScanToNewLineStart(ref s, ref endLine);

            for (; startIndex != endLine; startIndex++)
            {
                if (characterAction(s[startIndex])) return;
            }
        }
        #endregion
    }
}
