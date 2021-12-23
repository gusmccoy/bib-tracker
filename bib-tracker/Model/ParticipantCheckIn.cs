using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.Model
{
    public class ParticipantCheckIn
    {
        public ParticipantCheckIn(CheckInViewModel checkIn)
        {
            ParticipantBib = checkIn.ParticipantBib;
            StationNumber = checkIn.StationId;
            Timestamp = checkIn.Timestamp;
        }

        public ParticipantCheckIn()
        {}

        public int Id { get; set; }

        public int ParticipantBib { get; set; }

        public int StationNumber { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
