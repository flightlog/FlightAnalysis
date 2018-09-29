﻿using GeoAPI.Geometries;
using System;
using System.Collections.Generic;

namespace Boerman.FlightAnalysis.Models
{
    public class Flight
    {
        public Flight() {
            Id = Guid.NewGuid();
            PositionUpdates = new List<PositionUpdate>();
        }

        public Flight(FlightMetadata metadata)
        {
            Id = metadata.Id ?? Guid.NewGuid();
            Aircraft = metadata.Aircraft;
            LastSeen = metadata.LastSeen;
            StartTime = metadata.DepartureTime;
            DepartureHeading = metadata.DepartureHeading;
            DepartureLocation = metadata.DepartureLocation;// != null ? new GeoCoordinate(metadata.DepartureCoordinate[0], metadata.DepartureCoordinate[1]) : null;
            DepartureInfoFound = metadata.DepartureInfoFound;
            EndTime = metadata.ArrivalTime;
            ArrivalHeading = metadata.ArrivalHeading;
            ArrivalInfoFound = metadata.ArrivalInfoFound;
            ArrivalLocation = metadata.ArrivalLocation;// != null ? new GeoCoordinate(metadata.ArrivalCoordinate[0], metadata.ArrivalCoordinate[1]) : null;

            if (metadata is FlightViewModel) 
                PositionUpdates = ((FlightViewModel)metadata).PositionUpdates ?? new List<PositionUpdate>();
            else 
                PositionUpdates = new List<PositionUpdate>();
        }

        public Guid Id { get; internal set; }

        public string Aircraft { get; internal set; }

        public DateTime? LastSeen { get; internal set; }

        public DateTime? StartTime { get; internal set; }
        public short DepartureHeading { get; internal set; }
        public Coordinate DepartureLocation { get; internal set; }
        public bool? DepartureInfoFound { get; internal set; }

        public DateTime? EndTime { get; internal set; }
        public short ArrivalHeading { get; internal set; }
        public Coordinate ArrivalLocation { get; internal set; }
        public bool? ArrivalInfoFound { get; internal set; }

        public ICollection<PositionUpdate> PositionUpdates { get; internal set; }

        public FlightViewModel ViewModel => new FlightViewModel(this);
        public FlightMetadata Metadata => new FlightMetadata(this);
    }
}
