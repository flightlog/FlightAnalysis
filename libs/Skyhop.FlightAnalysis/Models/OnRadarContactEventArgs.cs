﻿namespace Skyhop.FlightAnalysis.Models
{
    public class OnRadarContactEventArgs
    {
        public OnRadarContactEventArgs(Flight flight)
        {
            Flight = flight;
        }

        public Flight Flight { get; }
    }
}