using System.IO;

namespace PROShine.Cleaner.Unity.Metadata
{
    public class ObjectInfo
    {
        public long PathId { get; set; }

        public int DataOffset { get; set; }
        public int DataSize { get; set; }

        public int TypeIndex { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            reader.Align4();
            PathId = reader.ReadInt64();

            DataOffset = reader.ReadInt32();
            DataSize = reader.ReadInt32();

            TypeIndex = reader.ReadInt32();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Align4();
            writer.Write(PathId);

            writer.Write(DataOffset);
            writer.Write(DataSize);

            writer.Write(TypeIndex);
        }
    }
}
