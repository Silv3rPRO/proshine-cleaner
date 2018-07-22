using System.IO;

namespace PROShine.Cleaner.Unity.Metadata
{
    public class RTTIBaseClassDescriptor
    {
        private const int MonoBehaviourType = 114;

        public int ClassId { get; set; }

        public byte Unknown { get; set; }
        public short ScriptId { get; set; }

        public byte[] ScriptHash { get; set; }
        public byte[] TypeHash { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            ClassId = reader.ReadInt32();
            Unknown = reader.ReadByte();
            ScriptId = reader.ReadInt16();
            if (ClassId == MonoBehaviourType)
            {
                ScriptHash = reader.ReadBytes(16);
            }
            TypeHash = reader.ReadBytes(16);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ClassId);
            writer.Write(Unknown);
            writer.Write(ScriptId);
            if (ClassId == MonoBehaviourType)
            {
                writer.Write(ScriptHash);
            }
            writer.Write(TypeHash);
        }
    }
}
