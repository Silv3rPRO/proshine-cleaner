using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PROShine.Cleaner.Unity
{
    internal static class BinaryReaderExtensions
    {
        internal static int ReadInt32BE(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        internal static uint ReadUInt32BE(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        internal static string ReadNullTerminatedUtf8String(this BinaryReader reader)
        {
            IList<byte> bytes = new List<byte>();
            byte currentByte;
            while ((currentByte = reader.ReadByte()) != 0)
            {
                bytes.Add(currentByte);
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        internal static void Align4(this BinaryReader reader)
        {
            while (reader.BaseStream.Position % 4 != 0)
            {
                reader.BaseStream.Position++;
            }
        }
    }
}
