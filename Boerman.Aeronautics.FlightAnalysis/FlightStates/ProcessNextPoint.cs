﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Boerman.Aeronautics.FlightAnalysis.Models;

namespace Boerman.Aeronautics.FlightAnalysis.FlightStates
{
    /// <summary>
    /// The ProcessNextPoint state is being invoked to start processing of the next available data point.
    /// </summary>
    public class ProcessNextPoint : FlightState
    {
        public ProcessNextPoint(FlightContext context) : base(context)
        {
        }

        public override async Task Run()
        {
            if (Context.Flight.EndTime != null)
            {
                Context.QueueState(typeof(InitializeFlightState));
                Context.QueueState(typeof(ProcessNextPoint));
            }
            else
            {
                if (Context.Heap.IsEmpty) return;

                var id = Context.Heap.DeleteMin();

                Context.Data.TryRemove(id, out PositionUpdate positionUpdate);
                
                if (positionUpdate == null)
                {
                    Context.QueueState(typeof(ProcessNextPoint));
                    return;
                }

                Context.Flight.PositionUpdates.Add(positionUpdate);

                TimingChecks(positionUpdate.TimeStamp);
                
                Context.QueueState(typeof(DetermineFlightState));
            }
        }

        private void TimingChecks(DateTime currentTimeStamp)
        {
            if (Context.LatestTimeStamp == DateTime.MinValue) Context.LatestTimeStamp = currentTimeStamp;

            if (Context.Flight.StartTime == null)
            {
                // Just keep the buffer small by removing points older then 2 minutes
                Context.Flight.PositionUpdates
                        .Where(q => q.TimeStamp < currentTimeStamp.AddMinutes(-2))
                        .ToList()
                        .ForEach(q => Context.Flight.PositionUpdates.Remove(q));
            }
            else if (Context.LatestTimeStamp < currentTimeStamp.AddHours(-8))
            {
                Context.InvokeOnCompletedWithErrorsEvent();
                Context.QueueState(typeof(InitializeFlightState));
                Context.QueueState(typeof(ProcessNextPoint));
            }

            Context.LatestTimeStamp = currentTimeStamp;
        }
    }
}
