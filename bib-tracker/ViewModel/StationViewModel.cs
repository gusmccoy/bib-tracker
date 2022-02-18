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
    public class StationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Station station;
        private StationService stationService;
        private bool skipOnPropChanged = false;

        public StationViewModel(Station station)
        {
            this.station = station;
            stationService = new StationService();
        }

        public StationViewModel()
        {
            skipOnPropChanged = true;
        }

        public int Number
        {
            get
            {
                return station.Number;
            }
            private set
            {
                station.Number = value;
                OnPropertyChanged("Number");
            }
        }

        public string Name 
        { 
            get 
            { 
                return station.Name;
            }
            set 
            {
                station.Name = value;
                OnPropertyChanged("Name");
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (!skipOnPropChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
                stationService.UpdateStation(station);
            }
        }
    }
}
