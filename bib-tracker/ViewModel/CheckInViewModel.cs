using bib_tracker.Model;
using bib_tracker.Services;
using System;
using System.ComponentModel;

namespace bib_tracker.ViewModel
{
    public class CheckInViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ParticipantCheckIn checkIn;
        ParticipantCheckInService participantCheckInService;

        public CheckInViewModel(ParticipantCheckIn checkIn)
        {
            this.checkIn = checkIn;
            participantCheckInService = new ParticipantCheckInService();
        }

        public CheckInViewModel(int stationId, int bibNum, DateTime timestamp)
        {
            checkIn = new ParticipantCheckIn()
            {
                StationId = stationId,
                ParticipantBib = bibNum,
                Timestamp = timestamp,
            };

        }

        public int ParticipantBib
        {
            get
            {
                return checkIn.ParticipantBib;
            }
            set
            {
                checkIn.ParticipantBib = value;
                OnPropertyChanged("ParticipantBib");
            }
        }

        public int StationId 
        { 
            get
            {
                return checkIn.StationId;
            }
            set
            {
                checkIn.StationId = value;
                OnPropertyChanged("StationId");
            }
        }

        public DateTime Timestamp 
        { 
            get
            {
                return checkIn.Timestamp;
            }
            set
            {
                checkIn.Timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            participantCheckInService.Update(checkIn);
        }
    }
}
