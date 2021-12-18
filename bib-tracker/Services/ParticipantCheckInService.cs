using bib_tracker.DataAccess;
using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.Services
{
    class ParticipantCheckInService
    {
        ParticipantCheckInRepository participantCheckInRepository;

        public void Update(ParticipantCheckIn checkIn)
        {
            participantCheckInRepository.Update(checkIn);
        }
    }
}
