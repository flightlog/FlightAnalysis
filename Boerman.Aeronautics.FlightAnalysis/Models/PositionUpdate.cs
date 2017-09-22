﻿using System;
using System.Device.Location;

namespace Boerman.Aeronautics.FlightAnalysis.Models
{
    public class PositionUpdate
    {
        public PositionUpdate(string aircraft, DateTime timeStamp, double latitude, double longitude, double altitude, double speed, double heading)
        {
            Aircraft = aircraft;
            TimeStamp = timeStamp;
            GeoCoordinate = new GeoCoordinate(latitude, longitude, altitude, 0, 0, speed, heading);
        }

        public string Aircraft { get; }

        public DateTime TimeStamp { get; }

        public GeoCoordinate GeoCoordinate { get; }

        public double Latitude => GeoCoordinate.Latitude;
        public double Longitude => GeoCoordinate.Longitude;
        public double Altitude => GeoCoordinate.Altitude;
        public double Speed => GeoCoordinate.Speed;
        public double Heading => GeoCoordinate.Course;
    }
}
