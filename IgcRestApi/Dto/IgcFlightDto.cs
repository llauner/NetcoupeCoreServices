namespace IgcRestApi.Dto
{
    public class IgcFlightDto : BaseDto<IgcFlightDto>
    {
        public int Id { get; set; }             // Netcoupe Id
        public string Name { get; set; }            // IGC file name inside Zip file
        public string ZipFileName { get; set; }     // Zip file name
        public FlightStatus Status { get; set; }
    }
}
