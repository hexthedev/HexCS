using System;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// Contains instructions that can be used to process a string into
    /// relevent indices used for parsing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CharQueryProcessor<T>
    {
        /// <summary>
        /// Queries checked against each character in a string to determine indices that match a mapping
        /// </summary>
        public CharQueryMapping<T>[] AnalysisInstructions;

        /// <summary>
        /// Skip tests are performed before AnalysisInstructions to speed up analaysis. If they return true on
        /// a character, the index is skipped
        /// </summary>
        public DCharQuery[] SkipTests;

        /// <summary>
        /// Analyzes a string and returns an array of analysis units that hold info about imporant indices in the target string. 
        /// foreach character in a string the analysis first checks that no skip test is passed, 
        /// then uses AnalysisInstructions to test string index. When an AnalysisInstruction returns true, it is recorded
        /// along with the instruction id
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public CharQueryAnalysisUnit<T>[] Analyze(ref string target)
        {
            if (AnalysisInstructions == null || AnalysisInstructions.Length == 0) return new CharQueryAnalysisUnit<T>[0];

            List<CharQueryAnalysisUnit<T>> analysis = new List<CharQueryAnalysisUnit<T>>();

            if(SkipTests == null || SkipTests.Length == 0)
            {
                for (int i = 0; i < target.Length; i++)
                {
                    // should we log this index
                    foreach (CharQueryMapping<T> m in AnalysisInstructions)
                    {
                        if (m.Query(ref target, i))
                        {
                            analysis.Add(new CharQueryAnalysisUnit<T>() { Index = i, Id = m.Id });
                            break; ;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    bool skip = false;
                    // should we skip the character
                    foreach (DCharQuery q in SkipTests)
                    {
                        if (q(ref target, i))
                        {
                            skip = true;
                            break;
                        }
                    }
                    if (skip) continue;
                    
                    // should we log this index
                    foreach (CharQueryMapping<T> m in AnalysisInstructions)
                    {
                        if (m.Query(ref target, i))
                        {
                            analysis.Add(new CharQueryAnalysisUnit<T>() { Index = i, Id = m.Id });
                            break;
                        }
                    }
                }
            }

            return analysis.ToArray();
        }
    }
        
    /// <summary>
    /// A single unit of analysis
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct CharQueryAnalysisUnit<T>
    {
        /// <summary>
        /// The index where a CharQueryMapping was detected
        /// </summary>
        public int Index;

        /// <summary>
        /// The Id of the mapping that was detected
        /// </summary>
        public T Id;
    }

    /// <summary>
    /// A Char Query mapping is a mappying between a char query and an id
    /// which can be used to create a StringAnalysis
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CharQueryMapping<T>
    {
        /// <summary>
        /// The id associated with an index that passes the query
        /// </summary>
        public T Id;

        /// <summary>
        /// The query to use to test the id
        /// </summary>
        public DCharQuery Query;
    }
}
