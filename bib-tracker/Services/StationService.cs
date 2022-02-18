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
    public class StationService
    {
        private StationRepository stationRepository;
        private FileService fileService;

        public StationService()
        {
            stationRepository = new StationRepository();
            fileService = new FileService();
        }

        public void UpdateStation(Station station)
        {
            stationRepository.Update(station);
        }

        public void DeleteStation(int stationNumber)
        {
            stationRepository.Delete(stationNumber);
        }

        public void AddStation(int number)
        {
            stationRepository.Add(new Station()
            {
                Number = number,
                Name = "NA"
            });
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

        public StationViewModel GetStationById(int id)
        {
            List<StationViewModel> stationViewModels = new List<StationViewModel>();
            var stations = stationRepository.GetAllStations();

            foreach (var station in stations)
            {
                if(station.Number == id)
                {
                    return new StationViewModel(station);
                }
            }
            return new StationViewModel(new Station() {
                Number = 0
            });

        }

        public void WriteCurrentData(StorageFile file)
        {
            stationRepository.ExportCurrentRecords(file);
        }

        public async Task<string> ReadStationFile(StorageFile file)
        {
            return await fileService.InsertInfoIntoDatabase(Constants.STATION, file);
        }
    }
}
