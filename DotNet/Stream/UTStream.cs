using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// Utilties and extension methods for streams
    /// </summary>
    public static class UTStream
    {
        /// <summary>
        /// Reads all bytes from a stream. This is used in the case when a you don't know how large the stream is. 
        /// It reads the stream in chucks that are supplied to a List that is then converted into an array. This is
        /// not efficient and creates garbage. Can be improved later. 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            List<byte> bytes = new List<byte>();

            byte[] buffer = new byte[1024]; // buffer window
            int length = stream.Read(buffer, 0, 1024);

            while (length > 0)
            {
                for(int i = 0; i<length; i++)
                {
                    bytes.Add(buffer[i]);
                }

                length = stream.Read(buffer, 0, 1024);
            }

            return bytes.ToArray();
        }
    }
}
