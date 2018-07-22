using System.IO;

namespace PROShine.Cleaner.Unity.Header
{
    public class BundleHeader
    {
        public int MetadataSize { get; set; }
        public int FileSize { get; set; }
        public int Generation { get; set; }
        public uint DataOffset { get; set; }
        public bool IsBigEndian { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            MetadataSize = reader.ReadInt32BE();
            FileSize = reader.ReadInt32BE();
            Generation = reader.ReadInt32BE();
            DataOffset = reader.ReadUInt32BE();
            IsBigEndian = reader.ReadBoolean();
            reader.Align4();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteInt32BE(MetadataSize);
            writer.WriteInt32BE(FileSize);
            writer.WriteInt32BE(Generation);
            writer.WriteUInt32BE(DataOffset);
            writer.Write(IsBigEndian);
            writer.Align4();
        }
    }
}
