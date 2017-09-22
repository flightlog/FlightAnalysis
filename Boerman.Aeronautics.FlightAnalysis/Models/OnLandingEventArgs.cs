﻿namespace Boerman.Aeronautics.FlightAnalysis.Models
{
    public class OnLandingEventArgs
    {
        public OnLandingEventArgs(Flight flight)
        {
            Flight = flight;
        }

        public Flight Flight { get; }
    }
}