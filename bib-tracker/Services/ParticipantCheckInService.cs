using bib_tracker.DataAccess;
using bib_tracker.Model;
using bib_tracker.Shared;
using bib_tracker.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.Services
{
    public class ParticipantCheckInService
    {
        ParticipantCheckInRepository participantCheckInRepository;
        ParticipantRepository ParticipantRepository;
        FileService fileService;

        public ParticipantCheckInService()
        {
            participantCheckInRepository = new ParticipantCheckInRepository();
            ParticipantRepository = new ParticipantRepository();
            fileService = new FileService();
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


        public async Task<string> ReadCheckInFile(StorageFile file)
        {
            return await fileService.InsertInfoIntoDatabase(Constants.CHECKIN, file);
        }

        public void WriteCurrentData(StorageFile file)
        {
            participantCheckInRepository.ExportCurrentRecords(file);
        }
    }
}
