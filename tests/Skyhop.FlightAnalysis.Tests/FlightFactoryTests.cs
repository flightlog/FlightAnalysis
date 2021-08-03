using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Skyhop.FlightAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Skyhop.FlightAnalysis.Tests
{
    [TestClass]
    public class FlightFactoryTests
    {
        [TestMethod]
        public void TestFlightFactory()
        {
            var flightContextFactory = InitializeFlightContextWithData();
        }

        [TestMethod]
        public void AttachEmptyMetadataObject()
        {
            try
            {
                var ff = new FlightContextFactory();

                var flightMetadata = new Flight
                {
                    Aircraft = "FLRDD056A",
                    Id = Guid.NewGuid()
                };

                ff.Attach(flightMetadata);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AttachEmptyMetadataObjectToFactory()
        {
            try
            {
                var ff = new FlightContextFactory();

                var metadata = new Flight
                {
                    Aircraft = "FLRDD056A"
                };

                ff.Attach(metadata);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AttachEmptyFlightContextObjectToFactory()
        {
            try
            {
                var ff = new FlightContextFactory();

                var context = new FlightContext(new Flight
                {
                    Aircraft = "FLRDD056A"
                });

                ff.Attach(context);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void FindNearbyAircraft()
        {
            var flightContext = InitializeFlightContextWithData();

            // X, Y, Z: Longiutde, Latitude, Altitude
            var nearby = flightContext
                .FindNearby(new Point(4.356565, 51.452385), 2.2)
                .ToList();

            Assert.AreEqual(2, nearby.Count());
            Assert.AreEqual("6770", nearby.First().Flight.Aircraft);
        }

        [TestMethod]
        public void ProcessOneDayOfData()
        {
            var ff = new FlightContextFactory();

            using (var reader = new StreamReader(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Dependencies",
                "2020-03-07_EHWO.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var lines = csv.GetRecords<dynamic>();

                var positionUpdates = new List<PositionUpdate>();

                foreach (var line in lines)
                {
                    dynamic position = JsonConvert.DeserializeObject(line.location);

                    positionUpdates.Add(new PositionUpdate(
                        line.transponderCode as string,
                        DateTime.ParseExact(line.timestamp as string, "MMM d, yyyy @ H:mm:ss.FFF", CultureInfo.InvariantCulture),
                        (double)position.lat,
                        (double)position.lon,
                        Convert.ToDouble(line.altitude as string),
                        Convert.ToDouble(line.speed as string),
                        Convert.ToDouble(line.heading as string)));
                }

                var departureCounter = 0;
                var arrivalCounter = 0;

                ff.OnTakeoff += (sender, args) =>
                {
                    Console.WriteLine($"{args.Flight.Aircraft}: {args.Flight.DepartureTime}");

                    departureCounter++;
                };

                ff.OnLanding += (sender, args) =>
                {
                    Console.WriteLine($"{args.Flight.Aircraft}: {args.Flight.DepartureTime} - {args.Flight.ArrivalTime}");
                    arrivalCounter++;
                };

                ff.Process(positionUpdates);

                Assert.AreEqual(50, departureCounter);
                Assert.AreEqual(50, arrivalCounter);
            }
        }

        public FlightContextFactory InitializeFlightContextWithData()
        {
            try
            {
                var ff = new FlightContextFactory();

                // ToDo: Verify that all the data is being processed correctly.

                /*
                 * The definition of correct is that there should be different context instances for different aircraft.
                 * Next the start/end times, heading and locations should be correct and the events should be firing.
                 * The context factory should be capable of processing a lot of mixed information from different aircraft.
                 */

                ff.OnTakeoff += (sender, args) =>
                {

                };

                ff.OnLanding += (sender, args) =>
                {

                };

                ff.Process(Common.ReadFlightPoints("2017-04-08_D-1908.csv"));
                ff.Process(Common.ReadFlightPoints("2017-04-21_PH-1387.csv"));
                ff.Process(Common.ReadFlightPoints("2017-04-19_2017-04-21_PH-1387.csv"));
                ff.Process(Common.ReadFlightPoints("2017-04-25_PH-1384.csv"));

                return ff;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
                throw;
            }
        }
    }
}
