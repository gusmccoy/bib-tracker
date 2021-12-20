using bib_tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.Model
{
    public class Participant
    {

        public Participant()
        {

        }

        public Participant(ParticipantViewModel participantView)
        {
            Bib = participantView.Bib;
            FirstName = participantView.FirstName;
            LastName = participantView.LastName;
        }

        public int Id { get; set; }

        public int Bib { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
