namespace DotIGC.Records
{
    /// <summary>
    /// Represents a header record in a IGC data file.
    /// </summary>
    public class HeaderRecord : Record
    {
        public HeaderRecord(DataSource source, ThreeLetterCode code, string text, string value) : base(RecordType.H, text)
        {
            DataSource = source;
            ThreeLetterCode = code;
            Value = value ?? string.Empty;
        }

        public DataSource DataSource { get; private set; }

        public ThreeLetterCode ThreeLetterCode { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", RecordType, ThreeLetterCode, Text); 
        }
    }
}
