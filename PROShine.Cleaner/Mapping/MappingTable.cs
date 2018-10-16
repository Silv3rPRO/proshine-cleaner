using System.Collections.Generic;

namespace PROShine.Cleaner.Mapping
{
    public class MappingTable
    {
        public IList<EnumMapping> Enums { get; set; }

        public IList<ClassMapping> Classes { get; set; }
    }
}
