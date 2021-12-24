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
    public class StationService
    {
        private StationRepository stationRepository;

        public StationService()
        {
            stationRepository = new StationRepository();
        }

        public void UpdateStation(Station station)
        {
            stationRepository.Update(station);
        }

        public void DeleteStation(int stationNumber)
        {
            stationRepository.Delete(stationNumber);
        }

        public List<StationViewModel> GetAllStations()
        {
            List<StationViewModel> stationViewModels = new List<StationViewModel>();
            var stations = stationRepository.GetAllStations();

            foreach(var station in stations)
            {
                stationViewModels.Add(new StationViewModel(station));
            }

            return stationViewModels;
        }

        public void ReadStationFile(StorageFile file)
        {
            stationRepository.LoadStationsFile(file);
        }

        public void WriteCurrentData(StorageFile file)
        {
            stationRepository.ExportCurrentRecords(file);
        }
    }
}
