using System;
using System.Text;
using System.IO;
using System.Net;

namespace NetIO
{
    public class NetReader
    {
        private Stream s;

        /* Function:        NetReader (constructor)
         * Parameters:      Stream stream, the stream to read from
         * Description:     Initialises this instance of the class.
         */
        public NetReader(Stream stream)
        {
            s = stream;
            if (!s.CanRead)
                throw new ArgumentException("Stream passed to NetReader cannot be read from");

        }

        /* Function:        readByte
         * Parameters:      none
         * Description:     Reads a single byte from the stream. If attempting to read past the end of the stream, an IOException will be thrown.
         */
        public byte readByte()
        {
            int b;
            b = s.ReadByte();
            if (b == -1)
                throw new IOException("NetReader attempted to read past the end of the stream");
            return (byte)b;
        }

        /* Function:        readByteArray
         * Parameters:      byte[] b, the array to read the bytes into
         *                  int offset, the offset of the array to read into
         *                  int length, the number of bytes to read into the array
         * Description:     Reads length bytes from the stream into the array starting at offset. If attempting to read past the end of the stream, an IOException will be thrown.
         */
        public void readByteArray(ref byte[] b, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                b[i] = readByte();
            }
        }

        /* Function:        readInt32
         * Parameters:      none
         * Description:     Reads a 32-bit integer from the stream in Network Byte Order. If attempting to read past the end of the stream, an IOException will be thrown.
         */
        public int readInt32()
        {
            byte[] bytes = new byte[4];
            int retval;

            readByteArray(ref bytes, 0, 4);

            retval = BitConverter.ToInt32(bytes, 0);
            retval = IPAddress.NetworkToHostOrder(retval);

            return retval;
        }

        /* Function:        readString
         * Parameters:      none
         * Description:     Reads a UTF-8 encoded string from the stream in the format: (int32 length)(char[] UTF8chars)
         */
        public string readString()
        {
            StringBuilder strBuilder = new StringBuilder();
            int length;
            length = readInt32();
            byte[] input = new byte[length];
            readByteArray(ref input, 0, length);
            char[] chars = Encoding.UTF8.GetChars(input);
            strBuilder.Append(chars);
            return strBuilder.ToString();
        }
    }
}
