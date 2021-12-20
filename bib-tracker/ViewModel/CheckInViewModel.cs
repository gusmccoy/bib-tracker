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
                ParticipantId = bibNum,
                Timestamp = timestamp,
            };

        }

        public int ParticipantId
        {
            get
            {
                return checkIn.ParticipantId;
            }
            set
            {
                checkIn.ParticipantId = value;
                OnPropertyChanged("ParticipantId");
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
