namespace DotIGC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DotIGC.Records;

    public class IgcDocument
    {
        public IgcDocument(IEnumerable<Record> records)
        {
            Records = records.ToList();
            Header = new IgcDocumentHeader(Records);
            GeoPositions = new GeoPositionCollection(Records);
        }

        public List<Record> Records { get; private set; }
        
        public IgcDocumentHeader Header { get; private set; }

        public GeoPositionCollection GeoPositions { get; private set; }

        #region Static members

        public static IgcDocument Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return Load(stream);
            }
        }

        public static IgcDocument Load(Stream stream)
        {
            var container = new Container<RecordType, IRecordReader>();
            container.Bind(RecordType.A).To<FlightRecorderRecordReader>();
            container.Bind(RecordType.B).To<FixRecordReader>();
            container.Bind(RecordType.H).To<HeaderRecordReader>();

            var records = ReadDocument(stream, new RecordReader(container));

            return new IgcDocument(records);
        }

        static IEnumerable<Record> ReadDocument(Stream stream, IRecordReader recordReader)
        {
            using (var reader = new IgcReader(stream, recordReader))
            {
                Record record;
                while (reader.Read(out record))
                    yield return record;
            }

            yield break;
        }

        #endregion
    }
}
