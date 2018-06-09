﻿/*
 * This example shows how to get realtime flight information by subscribing to
 * a data source (APRS server) and processing this data in real time.
 * 
 * This application connects to the OGN APRS servers. Please note that most
 * activity will be seen during european daytime.
 */

using System;
using System.Linq;
using Boerman.AprsClient;
using Boerman.FlightAnalysis;
using Boerman.Core.Extensions;

namespace RealtimeFlightActivityMonitor
{
    class Program
    {
        public static Listener AprsClient;

        public static FlightContextFactory FlightContextFactory = new FlightContextFactory();

        static void Main(string[] args)
        {
            FlightContextFactory.OnTakeoff += (sender, e) => {
                Console.WriteLine($"{DateTime.UtcNow}: {e.Flight.Aircraft} - Took off from {e.Flight.DepartureLocation.Latitude}, {e.Flight.DepartureLocation.Longitude}");
            };

            FlightContextFactory.OnLanding += (sender, e) => {
                Console.WriteLine($"{DateTime.UtcNow}: {e.Flight.Aircraft} - Landed at {e.Flight.ArrivalLocation.Latitude}, {e.Flight.ArrivalLocation.Longitude}");
            };

            FlightContextFactory.OnRadarContact += (sender, e) => {
                var lastPositionUpdate = e.Flight.PositionUpdates.OrderByDescending(q => q.TimeStamp).First();

                Console.WriteLine($"{DateTime.UtcNow}: {e.Flight.Aircraft} - Radar contact at {lastPositionUpdate.Latitude}, {lastPositionUpdate.Longitude} @ {lastPositionUpdate.Altitude}ft {lastPositionUpdate.Heading.ToStringArrow()}");
            };


            AprsClient = new Listener(new Config()
            {
                Callsign = @"7CB0DBG",
                Password = "-1",
                Uri = "aprs.glidernet.org",
                UseOgnAdditives = true,
                Port = 10152
            });

            AprsClient.PacketReceived += (sender, e) => {
                if (e.AprsMessage.DataType == Boerman.AprsClient.Enums.DataType.Status) return;

                FlightContextFactory.Enqueue(new Boerman.FlightAnalysis.Models.PositionUpdate(
                    e.AprsMessage.Callsign,
                    e.AprsMessage.ReceivedDate,
                    e.AprsMessage.Latitude.AbsoluteValue,
                    e.AprsMessage.Longitude.AbsoluteValue,
                    e.AprsMessage.Altitude.FeetAboveSeaLevel,
                    e.AprsMessage.Speed.Knots,
                    e.AprsMessage.Direction.ToDegrees()));
            };

            AprsClient.Open();

            Console.WriteLine("Currently checking to see if we can receive some information!");


            Console.Read();
        }
    }
}
