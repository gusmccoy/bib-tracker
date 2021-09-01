using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.DataAccess
{
    class ParticipantRepository
    {
        public void Add(Participant participant)
        {
            SqliteDb.AddParticipant(participant);
        }

        public void Update()
        {

        }

        public void Delete()
        {

        }

        public Participant GetParticipantById()
        {
            return new Participant();
        }

        public List<Participant> GetAllParticipants()
        {
            return new List<Participant>();
        }
    }
}
