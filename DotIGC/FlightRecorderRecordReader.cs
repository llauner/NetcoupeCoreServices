namespace DotIGC
{
    using System;
    using DotIGC.Records;
    
    public class FlightRecorderRecordReader : IRecordReader
    {
        public Record Read(string text)
        {
            var recordType = RecordTypeExtension.Parse(text);
            
            if (recordType != RecordType.A)
                throw new ArgumentException("Wrong record type");

            var code = text.Substring(1, 3);
            var substring = text.Substring(4).Split(new[] { ':' });
            var id = substring[0];

            return new FlightRecorderRecord(code, id, substring.Length > 0 ? substring[1] : string.Empty);
        }
    }
}
