namespace DotIGC
{
    using DotIGC.Records;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class IgcDocumentHeader
    {
        public IgcDocumentHeader() { }

        public IgcDocumentHeader(IEnumerable<Record> records)
        {
            var mapper = GetHeaderMapper();
            foreach (var r in records.OfType<HeaderRecord>())
            {
                if (mapper.ContainsKey(r.ThreeLetterCode))
                    mapper[r.ThreeLetterCode](this, r);
            }
        }

        public DateTimeOffset Date { get; private set; }

        public string PilotInCharge { get; private set; }

        public string CrewMember { get; private set; }

        public string GliderType { get; private set; }

        public string GliderId { get; private set; }

        public string CompetitionId { get; private set; }

        public string CompetitionClass { get; private set; }

        public string FlightRecorder { get; private set; }

        public string GPS { get; private set; }

        public string FirmwareVersion { get; private set; }

        public string HardwareVersion { get; private set; }

        public int GpsDatum { get; private set; }

        public string PressureAltitudeSensor { get; private set; }

        public int Accuracy { get; private set; }

        #region Static members

        public static IgcDocumentHeader Load(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
                return Load(stream);
        }

        public static IgcDocumentHeader Load(Stream stream)
        {
            var container = new Container<RecordType, IRecordReader>();
            container.Bind(RecordType.H).To<HeaderRecordReader>();
            var headerRecords = GetHeaderRecords(stream, new RecordReader(container));
            return new IgcDocumentHeader(headerRecords.TakeWhile(r => r.RecordType == RecordType.H));
        }

        static IEnumerable<Record> GetHeaderRecords(Stream stream, IRecordReader recordReader)
        {
            using (var reader = new IgcReader(stream, recordReader))
            {
                Record record;
                if (!reader.Read(out record) || record.RecordType != RecordType.A)
                    throw new FileLoadException("Failed to read the A-record");

                while (reader.Read(out record))
                    yield return record;
            }
        }

        static Dictionary<ThreeLetterCode, Action<IgcDocumentHeader, Record>> GetHeaderMapper()
        {
            var mapper = new Dictionary<ThreeLetterCode, Action<IgcDocumentHeader, Record>>();

            mapper[ThreeLetterCode.DTE] = (header, record) =>
            {
                var recordText = record.Text.Replace("DATE:", "");
                var day = recordText.Substring(5, 2);
                var month = recordText.Substring(7, 2);
                var year = recordText.Substring(9, 2);

                var currentYearMillennium = (DateTime.Now.Year / 100 * 100);

                header.Date = new DateTimeOffset(new DateTime(currentYearMillennium + int.Parse(year), int.Parse(month), int.Parse(day)));
            };

            mapper[ThreeLetterCode.FXA] = (header, record) =>
            {
                var number = record.Text.Substring(5, 3);

                var parsingOk = int.TryParse(number, out var parsedNumber);
                header.Accuracy = (parsingOk) ? parsedNumber : 0;
            };

            mapper[ThreeLetterCode.PLT] = (header, record) =>
            {
                header.PilotInCharge = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.CM2] = (header, record) =>
            {
                header.CrewMember = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.GID] = (header, record) =>
            {
                header.GliderId = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.GTY] = (header, record) =>
            {
                header.GliderType = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.CID] = (header, record) =>
            {
                header.CompetitionId = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.CCL] = (header, record) =>
            {
                header.CompetitionClass = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.FTY] = (header, record) =>
            {
                header.FlightRecorder = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.GPS] = (header, record) =>
            {
                header.GPS = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.RFW] = (header, record) =>
            {
                header.FirmwareVersion = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.RHW] = (header, record) =>
            {
                header.HardwareVersion = ParseHeaderString(record.Text);
            };

            mapper[ThreeLetterCode.DTM] = (header, record) =>
            {
                var number = record.Text.Substring(5, 3);

                var parsingOk = int.TryParse(number, out var gpsDatum);
                header.GpsDatum = (parsingOk) ? gpsDatum : 0;
            };

            mapper[ThreeLetterCode.PRS] = (header, record) =>
            {
                header.PressureAltitudeSensor = ParseHeaderString(record.Text);
            };

            return mapper;
        }

        static string ParseHeaderString(string text)
        {
            var tokens = text.Split(new[] { ':' });
            return tokens.Length < 2 ? string.Empty : tokens[1].Trim();
        }

        #endregion
    }
}
