namespace DotIGC.Records
{
    using DotIGC.Annotations;
    
    public enum ThreeLetterCode
    {
        [Description("Linear accelerations in X")]
        ACX,
        [Description("Linear accelerations in Y")]
        ACY,
        [Description("Linear accelerations in Z")]
        ACZ,
        [Description("Angular accelerations in X")]
        ANX,
        [Description("Angular accelerations in Y")]
        ANY,
        [Description("Angular accelerations in Z")]
        ANZ,
        [Description("Altimeter pressure setting in hectoPascals")]
        ATS,
        [Description("Competion class")]
        CCL,
        [Description("Camera connect")]
        CCN,
        [Description("Compass course")]
        CCO,
        [Description("Camera disconnect")]
        CDC,
        [Description("Change of geodetic datum")]
        CGD,
        [Description("Competition ID")]
        CID,
        [Description("Club or organisation, and country, from which flown or operated")]
        CLB,
        [Description("Second Crew Member's Name")]
        CM2,
        [Description("Displacement east, metres")]
        DAE,
        [Description("Displacement north")]
        DAN,
        [Description("Date of Birth of the pilot-in-charge")]
        DB1,
        [Description("Date of Birth of second crew member")]
        DB2,
        [Description("Obsolete code, now use DB1")]
        DOB,
        [Description("Date")]
        DTE,
        [Description("Geodetic Datum in use for lat/long records")]
        DTM,
        [Description("Engine down")]
        EDN,
        [Description("Environmental Noise Level")]
        ENL,
        [Description("Engine off")]
        EOF,
        [Description("Engine on")]
        EON,
        [Description("Engine up")]
        EUP,
        [Description("Finish")]
        FIN,
        [Description("Flap position")]
        FLP,
        [Description("Flight Recorder Security")]
        FRS,
        [Description("Flight recorder type")]
        FTY,
        [Description("Fix accuracy")]
        FXA,
        [Description("Galileo (European GNSS system)")]
        GAL,
        [Description("GNSS (Separate module) Connect")]
        GCN,
        [Description("GNSS (Separate module) Disconnect")]
        GDC,
        [Description("Glider ID")]
        GID,
        [Description("GLONASS (Russian GNSS system)")]
        GLO,
        [Description("GPS (US GNSS system)")]
        GPS,
        [Description("Groundspeed")]
        GSP,
        [Description("Glider type")]
        GTY,
        [Description("Heading Magnetic")]
        HDM,
        [Description("Heading True")]
        HDT,
        [Description("Airspeed")]
        IAS,
        [Description("The last places of decimal minutes of latitude")]
        LAD,
        [Description("The last places of decimal minutes of longitude")]
        LOD,
        [Description("Low voltage")]
        LOV,
        [Description("MacCready setting for rate of climb/speed-to-fly (m/sec)")]
        MAC,
        [Description("Means of Propulsion")]
        MOP,
        [Description("Position of other aircraft")]
        OA1,
        [Description("Position of other aircraft")]
        OA2,
        [Description("Position of other aircraft")]
        OA3,
        [Description("Position of other aircraft")]
        OA4,
        [Description("Position of other aircraft")]
        OA5,
        [Description("Position of other aircraft")]
        OA6,
        [Description("Position of other aircraft")]
        OA7,
        [Description("Position of other aircraft")]
        OA8,
        [Description("Position of other aircraft")]
        OA9,
        [Description("Outside air temperature (Celsius)")]
        OAT,
        [Description("On Task – attempting task")]
        ONT,
        [Description("OO ID – OO equipment observation")]
        OOI,
        [Description("Pilot Event")]
        PEV,
        [Description("Post-Flight Claim")]
        PFC,
        [Description("Photo taken")]
        PHO,
        [Description("Pilot-in-charge")]
        PLT,
        [Description("Pressure Altitude Sensor")]
        PRS,
        [Description("RAIM - GPS Parameter")]
        RAI,
        [Description("Record addition")]
        REX,
        [Description("Firmware Revision Version of flight recorder")]
        RFW,
        [Description("Hardware Revision Version of flight recorder")]
        RHW,
        [Description("Obsolete code, now use CM2")]
        SCM,
        [Description("Security")]
        SEC,
        [Description("Site, Name, region, nation")]
        SIT,
        [Description("Satellites in use")]
        SIU,
        [Description("Start event")]
        STA,
        [Description("Airspeed True, give units (kt, kph, etc.)")]
        TAS,
        [Description("Decimal seconds of UTC time")]
        TDS,
        [Description("Total Energy Altitude in metres")]
        TEN,
        [Description("Turn point confirmation")]
        TPC,
        [Description("Track Magnetic")]
        TRM,
        [Description("Track True")]
        TRT,
        [Description("Time Zone Offset, hours from UTC to local time")]
        TZN,
        [Description("Undercarriage (landing gear)")]
        UND,
        [Description("Units of Measure")]
        UNT,
        [Description("Uncompensated variometer vertical speed in metres per second")]
        VAR,
        [Description("Compensated variometer vertical speed in metres per second")]
        VAT,
        [Description("Vertical Fix Accuracy")]
        VXA,
        [Description("Wind Direction")]
        WDI,
        [Description("Wind speed")]
        WSP,
        [Description("A manufacturer-selected code")]
        X,
        [Description("Unkown TLC")]
        Unkown
    }
}
