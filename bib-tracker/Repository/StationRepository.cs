using bib_tracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.DataAccess
{
    class StationRepository
    {
        public void Add(Station newStation)
        {
            SqliteDb.AddStation(newStation);
        }

        public void Update(Station updatedStation)
        {
            SqliteDb.UpdateStation(updatedStation);
        }

        public void Delete(int stationId)
        {
            SqliteDb.DeleteStation(stationId);
        }

        public Station GetStationById(int stationId)
        {
            return SqliteDb.GetStationByNumber(stationId);
        }

        public List<Station> GetAllStations()
        {
            return SqliteDb.GetAllStations();
        }

        public void ExportCurrentRecords(StorageFile file)
        {
            SqliteDb.WriteDataBaseRecord("STATION", file);
        }

    }
}
