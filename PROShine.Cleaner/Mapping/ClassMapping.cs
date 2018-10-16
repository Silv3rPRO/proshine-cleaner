using PROShine.Cleaner.Mapping.Evidences;
using System.Collections.Generic;

namespace PROShine.Cleaner.Mapping
{
    public class ClassMapping
    {
        public string Name { get; set; }

        public AttributeEvidence Attribute { get; set; }

        public IList<MethodEvidence> Methods { get; set; }
    }
}
