using System;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// Extract info from a byte[] by holding an index and automatically
    /// stepping through it every time you extract something
    /// </summary>
    public class ByteArrayExtractor : ADisposableManager
    {
        private byte[] _extractionArray;
        private int _index = 0;

        /// <inheritdoc/>
        protected override string AccessAfterDisposalMessage => "Trying to access ByteArrayExtractor that has been disposed";

        /// <inheritdoc/>
        protected override void SetDisposablesToNull()
        {
            _extractionArray = null;
        }

        #region API
        /// <summary>
        /// Bytes remaining in the array that can be extracted
        /// </summary>
        public int RemainingBytes => _extractionArray.Length - _index;

        /// <summary>
        /// Create a byte array extractor using the provided byte array
        /// </summary>
        /// <param name="extractionArray"></param>
        public ByteArrayExtractor(byte[] extractionArray)
        {
            _extractionArray = extractionArray;
        }

        /// <summary>
        /// Extract uint from byte array
        /// </summary>
        /// <returns>extracted uint</returns>
        public uint ExtractUInt()
        {
            uint value = BitConverter.ToUInt32(_extractionArray, _index);
            _index += sizeof(uint);
            return value;
        }

        /// <summary>
        /// Extract int from byte array
        /// </summary>
        /// <returns>extracted int</returns>
        public int ExtractInt()
        {
            int value = BitConverter.ToInt32(_extractionArray, _index);
            _index += sizeof(int);
            return value;
        }

        /// <summary>
        /// Extract bool from byte array
        /// </summary>
        /// <returns>extracted bool</returns>
        public bool ExtractBool()
        {
            bool value = BitConverter.ToBoolean(_extractionArray, _index);
            _index += sizeof(bool);
            return value;
        }

        /// <summary>
        /// Extract char from byte array
        /// </summary>
        /// <returns>extracted char</returns>
        public char ExtractChar()
        {
            char value = BitConverter.ToChar(_extractionArray, _index);
            _index += sizeof(char);
            return value;
        }

        /// <summary>
        /// Extract float from byte array
        /// </summary>
        /// <returns>extracted float</returns>
        public float ExtractFloat()
        {
            float value = BitConverter.ToSingle(_extractionArray, _index);
            _index += sizeof(float);
            return value;
        }

        /// <summary>
        /// Extract double from byte array
        /// </summary>
        /// <returns>extracted double</returns>
        public double ExtractDouble()
        {
            double value = BitConverter.ToDouble(_extractionArray, _index);
            _index += sizeof(double);
            return value;
        }

        /// <summary>
        /// Extract a string from a byte array with a particular encoding
        /// </summary>
        /// <param name="length">The number of characters (not bytes) in the string</param>
        /// <param name="encoding">The encoding which the string is encoded in</param>
        /// <returns></returns>
        public string ExtractString(int length, Encoding encoding)
        {
            int bytesPerChar = encoding.GetByteCount("a");
            int byteLength = length * bytesPerChar;

            string value = encoding.GetString(UTArray.SubArray(_extractionArray, _index, _index + byteLength));
            _index += byteLength;
            return value;
        }

        /// <summary>
        /// Extract remaining bytes in byte[] as new byte[]
        /// </summary>
        /// <returns>extracted byte[] containing remaining bytes</returns>
        public byte[] ExtractRemaining()
        {
            return _extractionArray.SubArray(_index, _extractionArray.Length);
        }
        #endregion
    }
}
