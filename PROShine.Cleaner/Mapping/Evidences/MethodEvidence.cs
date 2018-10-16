namespace PROShine.Cleaner.Mapping.Evidences
{
    public class MethodEvidence
    {
        public string Name { get; set; }
        public string ParamType { get; set; }

        public MethodEvidence()
        {
        }

        public static implicit operator MethodEvidence(string name)
        {
            return new MethodEvidence
            {
                Name = name
            };
        }
    }
}
