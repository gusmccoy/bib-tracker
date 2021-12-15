using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.DataAccess
{
    class ParticipantRepository
    {
        public void Add(Participant participant)
        {
            SqliteDb.AddParticipant(participant);
        }

        public void Update(Participant participant)
        {
            SqliteDb.UpdateParticipant(participant);
        }

        public void Delete(int participantId)
        {
            SqliteDb.DeleteParticipant(participantId);
        }

        public Participant GetParticipantById(int participantId)
        {
            return SqliteDb.GetParticipantById(participantId);
        }

        public List<Participant> GetAllParticipants()
        {
            return SqliteDb.GetAllParticipants();
        }

        public void LoadParticipantFile(StorageFile file)
        {
            SqliteDb.ReadFileData("PARTICIPANT", file);
        }
    }
}
