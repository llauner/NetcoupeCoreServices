namespace DotIGC.Tests
{
    using DotIGC;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class IgcDocumentTest
    {
        [TestMethod]
        public void Read_header_from_simple_igc_succeeds()
        {
            var path = @"Data/Simple.igc";
            var header = IgcDocumentHeader.Load(path);

            Assert.AreEqual(header.Date, new DateTime(2001, 7, 16));
            Assert.AreEqual(header.Accuracy, 35);
            Assert.AreEqual(header.PilotInCharge, "Bloggs Bill D");
            Assert.AreEqual(header.CrewMember, "Smith-Barry John A");
            Assert.AreEqual(header.GliderId, "ABCD-1234");
            Assert.AreEqual(header.GliderType, "Schleicher ASH-25");
            Assert.AreEqual(header.CompetitionId, "XYZ-78910");
            Assert.AreEqual(header.CompetitionClass, "15m Motor Glider");
            Assert.AreEqual(header.GPS, "Superstar,12ch, max10000m");
            Assert.AreEqual(header.FirmwareVersion, "6.4");
            Assert.AreEqual(header.HardwareVersion, "3.0");
            Assert.AreEqual(header.GpsDatum, 100);
            Assert.AreEqual(header.PressureAltitudeSensor, "Sensyn, XYZ1111, max11000m");
            Assert.AreEqual(header.FlightRecorder, "Manufacturer, Model");
        }

        [TestMethod]
        public void Read_header_from_2bff3xl1_igc_succeeds()
        {
            var path = @"Data/2bff3xl1.igc";
            var header = IgcDocumentHeader.Load(path);

            Assert.AreEqual(header.Date, new DateTime(2012, 11, 15));
            Assert.AreEqual(header.Accuracy, 100);
            Assert.AreEqual(header.PilotInCharge, "BERND_FABIAN");
            Assert.AreEqual(header.GliderId, "D_KUEL");
            Assert.AreEqual(header.GliderType, "VENTUS2CM18");
            Assert.AreEqual(header.CompetitionId, "D1");
            //Assert.AreEqual(header.CompetitionClass, "18M_FAI*");
            Assert.AreEqual(header.GPS, "FDK/GSU-14");
            Assert.AreEqual(header.FirmwareVersion, "5.1");
            Assert.AreEqual(header.HardwareVersion, "2.1");
            Assert.AreEqual(header.GpsDatum, 100);
            Assert.AreEqual(header.FlightRecorder, "FILSER,LX20");
        }

        [TestMethod]
        public void Load_document_from_2bff3xl1_igc_succeeds()
        {
            var path = @"Data/2bff3xl1.igc";
            var document = IgcDocument.Load(path);
            var header = document.Header;

            Assert.AreEqual(header.Date, new DateTime(2012, 11, 15));
            Assert.AreEqual(header.Accuracy, 100);
            Assert.AreEqual(header.PilotInCharge, "BERND_FABIAN");
            Assert.AreEqual(header.GliderId, "D_KUEL");
            Assert.AreEqual(header.GliderType, "VENTUS2CM18");
            Assert.AreEqual(header.CompetitionId, "D1");
            //Assert.AreEqual(header.CompetitionClass, "18M_FAI*");
            Assert.AreEqual(header.GPS, "FDK/GSU-14");
            Assert.AreEqual(header.FirmwareVersion, "5.1");
            Assert.AreEqual(header.HardwareVersion, "2.1");
            Assert.AreEqual(header.GpsDatum, 100);
            Assert.AreEqual(header.FlightRecorder, "FILSER,LX20");

            Assert.IsTrue(document.Records.Count > 0);
        }
    }
}
