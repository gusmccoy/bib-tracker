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
        private ParticipantCheckIn checkIn;

        public ParticipantCheckIn(CheckInViewModel checkIn)
        {
            Id = checkIn.Id;
            ParticipantId = checkIn.ParticipantId;
            StationId = checkIn.StationId;
            Timestamp = checkIn.Timestamp;
        }

        public ParticipantCheckIn()
        {}

        public int Id { get; set; }

        public int ParticipantId { get; set; }

        public int StationId { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
