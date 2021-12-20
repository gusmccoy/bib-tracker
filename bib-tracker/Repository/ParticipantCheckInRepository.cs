using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.DataAccess
{
    class ParticipantCheckInRepository
    {
        public void Add(ParticipantCheckIn checkIn)
        {
            SqliteDb.AddParticipantCheckIn(checkIn);
        }

        public void Update(ParticipantCheckIn checkIn)
        {
            
        }

        public void Delete(int checkInId)
        {
            
        }

        //public ParticipantCheckIn GetCheckInById(int checkInId)
        //{
        //    return new ParticipantCheckIn();
        //}

        public List<ParticipantCheckIn> GetAllCheckIns()
        {
            return SqliteDb.GetAllParticipantsCheckIns();
        }

        public List<ParticipantCheckIn> GetAllCheckInsByParticipantId(int participantId)
        {
            return SqliteDb.GetCheckinsByParticipantId(participantId);
        }

        public List<ParticipantCheckIn> GetAllCheckInsByStationId(int stationId)
        {
            return SqliteDb.GetCheckinsByStationId(stationId);
        }

        public void ReadCheckInFile(StorageFile file)
        {
            SqliteDb.ReadFileData("CHECKIN", file);
        }

        public void ExportCurrentRecords(StorageFile file)
        {
            SqliteDb.WriteDataBaseRecord("CHECKIN", file);
        }
    }
}
