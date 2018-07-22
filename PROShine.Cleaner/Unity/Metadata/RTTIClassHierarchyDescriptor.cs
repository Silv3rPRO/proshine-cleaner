using System.Collections.Generic;
using System.IO;

namespace PROShine.Cleaner.Unity.Metadata
{
    public class RTTIClassHierarchyDescriptor
    {
        public string Version { get; set; }
        public uint Platform { get; set; }
        public bool HasSerializedTypeTrees { get; set; }

        public IList<RTTIBaseClassDescriptor> Types { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            Version = reader.ReadNullTerminatedUtf8String();
            Platform = reader.ReadUInt32();
            HasSerializedTypeTrees = reader.ReadBoolean();

            int count = reader.ReadInt32();
            Types = new List<RTTIBaseClassDescriptor>(count);
            for (int i = 0; i < count; ++i)
            {
                var classDescriptor = new RTTIBaseClassDescriptor();
                classDescriptor.Deserialize(reader);
                Types.Add(classDescriptor);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteNullTerminatedUtf8String(Version);
            writer.Write(Platform);
            writer.Write(HasSerializedTypeTrees);

            writer.Write(Types.Count);
            foreach (var classDescriptor in Types)
            {
                classDescriptor.Serialize(writer);
            }
        }
    }
}
