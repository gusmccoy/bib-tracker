﻿using bib_tracker.DataAccess;
using bib_tracker.Model;
using bib_tracker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.ViewModel
{
    public class ParticipantViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Participant participant;
        private ParticipantService participantService;


        public ParticipantViewModel(Participant participant)
        {
            this.participant = participant;
            participantService = new ParticipantService();
        }

        public int Id 
        {
            get 
            {
                return participant.Id;
            }
            set 
            {
                participant.Id = value;
            }
        }

        public int Bib
        {
            get
            {
                return participant.Bib;
            }
            set
            {
                participant.Bib = value;
            }
        }

        public string FirstName
        {
            get
            {
                return participant.FirstName;
            }
            set
            {
                participant.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get
            {
                return participant.LastName;
            }
            set
            {
                participant.LastName = value;
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            participantService.UpdateParticipant(participant);
        }
    }
}