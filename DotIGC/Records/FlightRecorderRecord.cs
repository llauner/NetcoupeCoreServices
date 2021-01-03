namespace DotIGC.Records
{
    using System;
    
    public class FlightRecorderRecord : Record
    {
        /// <summary>
        /// Represents the A-record.
        /// </summary>
        /// <param name="manufacturer">The three letter manufacturer manufacturer.</param>
        /// <param name="id">The flight recorder unique id.</param>
        /// <param name="additionalData">Additonal data.</param>        
        /// <exception cref="ArgumentException">If the manufacturer manufacturer is not three letters.</exception>        
        public FlightRecorderRecord(string manufacturer, string id, string additionalData) : base(RecordType.A)
        {
            if (string.IsNullOrWhiteSpace(manufacturer) || manufacturer.Length != 3)
                throw new ArgumentException("Manufacturer manufacturer must contain 3 letters");
            
            Manufacturer = manufacturer;
            Id = id;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// Gets the three letter manufacturer manufacturer.
        /// </summary>
        public string Manufacturer { get; private set; }

        /// <summary>
        /// Gets the flight record unique id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the additonal data.
        /// </summary>
        public string AdditionalData { get; private set; }

        public override string ToString()
        {
            return string.Format("Manufacturer: {0}, FR: {1}, {2}", Manufacturer, Id, AdditionalData); 
        }
    }
}
