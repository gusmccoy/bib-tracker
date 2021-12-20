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
    public class ParticipantService
    {
        private ParticipantRepository participantRepository;

        public ParticipantService()
        {
            participantRepository = new ParticipantRepository();
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

        public void LoadFile(StorageFile file)
        {
            participantRepository.LoadParticipantFile(file);
        }

        public void WriteCurrentData(StorageFile file)
        {
            participantRepository.ExportCurrentRecords(file);
        }
    }
}
