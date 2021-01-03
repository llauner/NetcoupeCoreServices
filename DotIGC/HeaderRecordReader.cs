namespace DotIGC
{
    using System;
    using DotIGC.Records;
    
    public class HeaderRecordReader : IRecordReader
    {
        public Records.Record Read(string text)
        {
            var recordType = RecordTypeExtension.Parse(text);

            if (recordType != RecordType.H)
                throw new ArgumentException("Wrong record type");

            var source = text[1] == 'F' ? DataSource.FlightRecorder : DataSource.Other;
            var values = text.Split(new[] { ':' });
            var value = values.Length > 1 ? values[1] : string.Empty;

            // Parse the Three letters code
            if (Enum.TryParse<ThreeLetterCode>(text.Substring(2, 3), out ThreeLetterCode tlc))
            {

                return new HeaderRecord(source, tlc, text, value);
            }
            else
            {
                // Unknown three letter code found
                return new HeaderRecord(source, ThreeLetterCode.X, text, value);
            }
        }
    }
}
