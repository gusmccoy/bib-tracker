using bib_tracker.DataAccess;
using bib_tracker.Model;
using bib_tracker.Shared;
using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.Services
{
    public class ParticipantService
    {
        private ParticipantRepository participantRepository;
        private FileService fileService;

        public ParticipantService()
        {
            participantRepository = new ParticipantRepository();
            fileService = new FileService();
        }

        public List<ParticipantViewModel> GetAllParticipants()
        {
            List<ParticipantViewModel> participantViews = new List<ParticipantViewModel>();
            List<Participant> participants = participantRepository.GetAllParticipants();

            foreach(var participant in participants)
            {
                participantViews.Add(new ParticipantViewModel(participant));
            }

            return participantViews;
        }

        public ParticipantViewModel GetParticipantByBibNumber(int bibNumber)
        {
            return new ParticipantViewModel(participantRepository.GetParticipantByBibNumber(bibNumber));
        }

        public void AddParticipant(ParticipantViewModel participantView)
        {
            participantRepository.Add(new Participant(participantView));
        }

        public void UpdateParticipant(Participant participantView)
        {
            participantRepository.Update(participantView);
        }

        public void DeleteParticipant(int bib)
        {
            participantRepository.Delete(bib);
        }

        public void WriteCurrentData(StorageFile file)
        {
            participantRepository.ExportCurrentRecords(file);
        }

        public async Task<string> ReadParticipantFile(StorageFile file)
        {
            return await fileService.InsertInfoIntoDatabase(Constants.PARTICIPANT, file);
        }
    }
}
