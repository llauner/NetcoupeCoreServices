namespace DotIGC.Records
{
    /// <summary>
    /// Represents a record in a IGC data file.
    /// </summary>
    public class Record
    {
        public Record(RecordType recordType, string text)
        {
            RecordType = recordType;
            Text = text ?? string.Empty;
        }

        public Record(RecordType recordType) : this(recordType, string.Empty) { }

        /// <summary>
        /// Gets the <see cref="RecordType"/>.
        /// </summary>
        public RecordType RecordType { get; private set; }

        /// <summary>
        /// Gets the unformated text string from the IGC data file.
        /// </summary>
        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }       
    }
}
