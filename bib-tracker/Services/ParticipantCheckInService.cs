using bib_tracker.DataAccess;
using bib_tracker.Model;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.Services
{
    public class ParticipantCheckInService
    {
        ParticipantCheckInRepository participantCheckInRepository;
        ParticipantRepository ParticipantRepository;

        public ParticipantCheckInService()
        {
            participantCheckInRepository = new ParticipantCheckInRepository();
            ParticipantRepository = new ParticipantRepository();
        }

        public void Add(CheckInViewModel checkInViewModel)
        {

            participantCheckInRepository.Add(new ParticipantCheckIn(checkInViewModel));
        }

        public void Update(ParticipantCheckIn checkIn)
        {
            participantCheckInRepository.Update(checkIn);
        }

        public void DeleteAllRecordsByStationNumber(int stationNumber)
        {
            participantCheckInRepository.DeleteByStationId(stationNumber);
        }

        public List<CheckInViewModel> GetAllParticipantCheckIns()
        {
            var checkinViewModels = new List<CheckInViewModel>();
            var checkIns = participantCheckInRepository.GetAllCheckIns();

            foreach(var checkIn in checkIns)
            {
                checkinViewModels.Add(new CheckInViewModel(checkIn));
            }

            return checkinViewModels;
        }

        public List<CheckInViewModel> GetAllParticipantCheckInsByStation(int stationId)
        {
            var checkinViewModels = new List<CheckInViewModel>();
            var checkIns = participantCheckInRepository.GetAllCheckInsByStationId(stationId);

            foreach (var checkIn in checkIns)
            {
                checkinViewModels.Add(new CheckInViewModel(checkIn));
            }

            return checkinViewModels;
        }

        public List<ParticipantViewModel> GetAllRemainingParticipantsByStationId(int stationId)
        {
            var checkIns = GetAllParticipantCheckInsByStation(stationId);
            var checkedInBibs = new List<int>();
            foreach(CheckInViewModel checkIn in checkIns)
            {
                checkedInBibs.Add(checkIn.ParticipantBib);
            }
            var remainingParticipants = ParticipantRepository.GetRemainingParticipants(checkedInBibs);
            List<ParticipantViewModel> participantViewModels = new List<ParticipantViewModel>();

            foreach (var participant in remainingParticipants)
            {
                participantViewModels.Add(new ParticipantViewModel(participant));
            }

            return participantViewModels;
        }


        public void ReadCheckInFile(StorageFile file)
        {
            participantCheckInRepository.ReadCheckInFile(file);
        }

        public void WriteCurrentData(StorageFile file)
        {
            participantCheckInRepository.ExportCurrentRecords(file);
        }
    }
}
