namespace DotIGC.Records
{
    using System;
    using DotIGC.Annotations;
    
    public enum RecordType
    {
        [Description("FR manufacturer and identification")]
        A,
        [Description("Fix")]
        B,
        [Description("Task/declaration")]
        C,
        [Description("Differential GPS")]
        D,
        [Description("Event", "The E-record is used to record specific events on the IGC file that occur at irregular intervals.")]
        E,
        [Description("Satellite constellation", "Satellites used in B record fixes")]
        F,
        [Description("Security")]
        G,
        [Description("File header")]
        H,
        [Description("List of additional data included at end of each B-record")]
        I,
        [Description("List of additional data included at end of each K-record")]
        J,
        [Description("Frequent data, additional to the B-record")]
        K,
        [Description("Logbook/comments", "The L-Record allows free format text lines to be added to the flight data records at any time in the time-sequence")]
        L
    }

    public static class RecordTypeExtension
    {
        public static string ToString(this RecordType recordType)
        {
            var attributes = recordType.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Short;
        }

        public static RecordType Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException("text");

            char c = text.ToUpper()[0];
            switch (c)
            {
                case 'A':
                    return RecordType.A;
                case 'B':
                    return RecordType.B;
                case 'C':
                    return RecordType.C;
                case 'D':
                    return RecordType.D;
                case 'E':
                    return RecordType.E;
                case 'F':
                    return RecordType.F;
                case 'G':
                    return RecordType.G;
                case 'H':
                    return RecordType.H;
                case 'I':
                    return RecordType.I;
                case 'J':
                    return RecordType.J;
                case 'K':
                    return RecordType.K;
                case 'L':
                    return RecordType.L;
                default:
                    throw new Exception("Unsupported record type");
            }
        }
    }
}
