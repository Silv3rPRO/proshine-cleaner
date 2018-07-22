using System.IO;

namespace PROShine.Cleaner.Unity.Metadata
{
    public class FileReference
    {
        public string AssetPath { get; set; }
        public byte[] Hash { get; set; }
        public int Type { get; set; }
        public string FilePath { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            AssetPath = reader.ReadNullTerminatedUtf8String();
            Hash = reader.ReadBytes(16);
            Type = reader.ReadInt32();
            FilePath = reader.ReadNullTerminatedUtf8String();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteNullTerminatedUtf8String(AssetPath);
            writer.Write(Hash);
            writer.Write(Type);
            writer.WriteNullTerminatedUtf8String(FilePath);
        }
    }
}
