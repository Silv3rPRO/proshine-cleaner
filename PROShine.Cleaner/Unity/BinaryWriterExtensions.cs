using System;
using System.IO;
using System.Text;

namespace PROShine.Cleaner.Unity
{
    internal static class BinaryWriterExtensions
    {
        internal static void WriteInt32BE(this BinaryWriter writer, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            writer.Write(bytes);
        }

        internal static void WriteUInt32BE(this BinaryWriter writer, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            writer.Write(bytes);
        }

        internal static void WriteNullTerminatedUtf8String(this BinaryWriter writer, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes);
            writer.Write((byte)0);
        }

        internal static void Align4(this BinaryWriter writer)
        {
            while (writer.BaseStream.Position % 4 != 0)
            {
                writer.Write((byte)0);
            }
        }
    }
}
