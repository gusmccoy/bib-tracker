using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.DataAccess
{
    class ParticipantCheckInRepository
    {
        public void Add()
        {
        }

        public void Update()
        {

        }

        public void Delete()
        {

        }

        public ParticipantCheckIn GetCheckInById()
        {
            return new ParticipantCheckIn();
        }

        public List<ParticipantCheckIn> GetAllCheckInsByParticipantId()
        {
            return new List<ParticipantCheckIn>();
        }

        public List<ParticipantCheckIn> GetAllCheckInsByStationId()
        {
            return new List<ParticipantCheckIn>();
        }
    }
}
