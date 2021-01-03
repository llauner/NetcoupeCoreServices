namespace DotIGC
{
    using System;
    using DotIGC.Records;
    
    public class RecordReader : IRecordReader
    {
        Container<RecordType, IRecordReader> readers;

        public RecordReader(Container<RecordType, IRecordReader> readers)
        {
            this.readers = readers;
        }
        
        public Record Read(string text)
        {
            var recordType = RecordTypeExtension.Parse(text);
            return Read(recordType, text);
        }

        protected virtual Record Read(RecordType recordType, string text)
        {
            var reader = readers.Resolve(recordType);
            if (reader != null)
                return reader.Read(text);

            return new Record(recordType, text);
        }
    }
}
