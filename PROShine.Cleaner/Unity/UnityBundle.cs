using PROShine.Cleaner.Unity.Header;
using PROShine.Cleaner.Unity.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PROShine.Cleaner.Unity
{
    public class UnityBundle
    {
        public BundleHeader Header { get; set; }
        public BundleMetadata Metadata { get; set; }

        public string ContentData { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            Header = new BundleHeader();
            Header.Deserialize(reader);

            Metadata = new BundleMetadata();
            Metadata.Deserialize(reader);
            
            ContentData = Encoding.Default.GetString(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)));
        }

        public void Serialize(BinaryWriter writer)
        {
            Header.Serialize(writer);

            long metadataStart = writer.BaseStream.Position;
            Metadata.Serialize(writer);
            Header.MetadataSize = (int)(writer.BaseStream.Position - metadataStart);

            writer.Write(Encoding.Default.GetBytes(ContentData));
        }

        public static UnityBundle ReadFromFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    UnityBundle bundle = new UnityBundle();
                    bundle.Deserialize(reader);
                    return bundle;
                }
            }
        }

        public void WriteToFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    Serialize(writer);
                }
            }
        }
    }
}
