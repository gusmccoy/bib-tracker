using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.Model
{
    class ParticipantCheckIn
    {
        public int Id { get; set; }

        public int ParticipantId { get; set; }

        public int StationId { get; set; }

    }
}
