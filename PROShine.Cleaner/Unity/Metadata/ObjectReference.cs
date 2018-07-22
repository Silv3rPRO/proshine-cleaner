using System.IO;

namespace PROShine.Cleaner.Unity.Metadata
{
    public class ObjectReference
    {
        public int FileId { get; set; }
        public long ObjectId { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            FileId = reader.ReadInt32();

            reader.Align4();
            ObjectId = reader.ReadInt64();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(FileId);

            writer.Align4();
            writer.Write(ObjectId);
        }
    }
}
