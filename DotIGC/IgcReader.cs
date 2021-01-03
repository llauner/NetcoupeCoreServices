namespace DotIGC
{
    using System;
    using System.IO;
    using DotIGC.Records;
    
    public class IgcReader : IDisposable
    {
        IRecordReader recordReader;
        StreamReader reader;
        bool disposed;
      
        public IgcReader(Stream stream, IRecordReader recordReader)
        {
            if (recordReader == null)
                throw new ArgumentNullException("recordReader");

            this.recordReader = recordReader;
            this.reader = new StreamReader(stream);
        }

        public bool Read(out Record record)
        { 
            record = null;

            if (!this.reader.BaseStream.CanRead || this.reader.EndOfStream)
                return false;

            var text = reader.ReadLine();
            record = this.recordReader.Read(text);
            return true;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            this.reader.Dispose();
            this.reader = null;      
            disposed = true;
        }
    }
}
