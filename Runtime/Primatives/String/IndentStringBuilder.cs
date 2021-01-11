using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// String Builder that allows user to specify an indent to apply as tabs in front of a string
    /// </summary>
    public class IndentStringBuilder
    {
        private StringBuilder _stringBuilder = new StringBuilder();

        private int _initialIndent;

        private string _indentCharacter;

        #region API
        /// <summary>
        /// The current indent in the string builder
        /// </summary>
        public int CurrentIndent { get; private set; }

        /// <summary>
        /// Construct with inital indent and indent character
        /// </summary>
        /// <param name="initialIndent"></param>
        /// <param name="indentChar"></param>
        public IndentStringBuilder(int initialIndent, string indentChar = "\t")
        {
            _initialIndent = initialIndent;
            CurrentIndent = initialIndent;
            _indentCharacter = indentChar;
        }

        /// <summary>
        /// Add a line to string buildren automatically indented
        /// </summary>
        /// <param name="toAppend"></param>
        /// <param name="indentChange"></param>
        public void AppendLine(string toAppend, int indentChange = 0)
        {
            _stringBuilder.AppendLine($"{UTString.RepeatedCharacter(_indentCharacter, CurrentIndent)}{toAppend}");
            CurrentIndent += indentChange;
        }

        /// <summary>
        /// Add a line to string buildren automatically indented
        /// </summary>
        /// <param name="toAppend"></param>
        /// <param name="indentChange"></param>
        public void Append(string toAppend, int indentChange = 0)
        {
            _stringBuilder.Append($"{UTString.RepeatedCharacter(_indentCharacter, CurrentIndent)}{toAppend}");
            CurrentIndent += indentChange;
        }

        /// <summary>
        /// Add a string to string buildren without adding tabs
        /// </summary>
        /// <param name="toAppend"></param>
        /// <param name="indentChange"></param>
        public void AppendRaw(string toAppend, int indentChange = 0)
        {
            _stringBuilder.Append(toAppend);
            CurrentIndent += indentChange;
        }

        /// <summary>
        /// Add the change value to the current indent
        /// </summary>
        /// <param name="change"></param>
        public void ChangeIndent(int change)
        {
            CurrentIndent += change;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        /// <summary>
        /// Clears string cache and resets the current indent
        /// </summary>
        public void Clear()
        {
            _stringBuilder.Clear();
            CurrentIndent = _initialIndent;
        }
        #endregion
    }
}
