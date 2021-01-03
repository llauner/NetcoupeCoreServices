namespace DotIGC
{
    using DotIGC.Records;
    
    public interface IRecordReader
    {
        Record Read(string text);
    }
}
