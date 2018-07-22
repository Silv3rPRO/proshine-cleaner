using System.Collections.Generic;
using System.IO;

namespace PROShine.Cleaner.Unity.Metadata
{
    public class BundleMetadata
    {
        public RTTIClassHierarchyDescriptor Hierarchy { get; set; }

        public IList<ObjectInfo> Objects { get; set; }
        public IList<ObjectReference> PreloadedObjects { get; set; }
        public IList<FileReference> Dependencies { get; set; }
        
        public byte[] Unknown { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            Hierarchy = new RTTIClassHierarchyDescriptor();
            Hierarchy.Deserialize(reader);

            int count = reader.ReadInt32();
            Objects = new List<ObjectInfo>(count);
            for (int i = 0; i < count; ++i)
            {
                var objectInfo = new ObjectInfo();
                objectInfo.Deserialize(reader);
                Objects.Add(objectInfo);
            }

            count = reader.ReadInt32();
            PreloadedObjects = new List<ObjectReference>(count);
            for (int i = 0; i < count; ++i)
            {
                var objectRef = new ObjectReference();
                objectRef.Deserialize(reader);
                PreloadedObjects.Add(objectRef);
            }

            count = reader.ReadInt32();
            Dependencies = new List<FileReference>(count);
            for (int i = 0; i < count; ++i)
            {
                var dependency = new FileReference();
                dependency.Deserialize(reader);
                Dependencies.Add(dependency);
            }

            Unknown = reader.ReadBytes(8);
            reader.Align4();
        }

        public void Serialize(BinaryWriter writer)
        {
            Hierarchy.Serialize(writer);

            writer.Write(Objects.Count);
            foreach (var objectInfo in Objects)
            {
                objectInfo.Serialize(writer);
            }

            writer.Write(PreloadedObjects.Count);
            foreach (var objectRef in PreloadedObjects)
            {
                objectRef.Serialize(writer);
            }

            writer.Write(Dependencies.Count);
            foreach (var dependency in Dependencies)
            {
                dependency.Serialize(writer);
            }

            writer.Write(Unknown);
            writer.Align4();
        }
    }
}
