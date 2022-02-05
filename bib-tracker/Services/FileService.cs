using bib_tracker.DataAccess;
using bib_tracker.Model;
using bib_tracker.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace bib_tracker.Services
{
    class FileService
    {
        public FileService() { }
        public async Task<string> InsertInfoIntoDatabase(string filetype, StorageFile file)
        {
            string response = Constants.FILE_INPUT_ALL_GOOD;
            string contents = await FileIO.ReadTextAsync(file);
            contents = contents.Trim(new Char[] { '\r' });
            string[] lines = contents.Split('\n');
            int errorCount = 0;
            foreach (string row in lines)
            {
                if (row.Trim().Length > 0)
                {
                    try
                    {
                        string[] line = row.Split('\t');

                        switch (filetype)
                        {
                            case Constants.PARTICIPANT:
                                SqliteDb.AddParticipant(new Participant()
                                {
                                    Bib = Int32.Parse(line[0]),
                                    FirstName = line[1].Trim(),
                                    LastName = line[2].Trim()
                                });
                                break;
                            case Constants.STATION:
                                SqliteDb.AddStation(new Station()
                                {
                                    Number = Int32.Parse(line[0]),
                                    Name = line[1].Trim()
                                });
                                break;
                            case Constants.CHECKIN:
                                SqliteDb.AddParticipantCheckIn(new ParticipantCheckIn()
                                {
                                    ParticipantBib = int.Parse(line[0]),
                                    StationNumber = int.Parse(line[1]),
                                    Timestamp = DateTime.Parse(line[2].Trim())
                                });
                                break;
                        }
                    }
                    catch (FormatException e)
                    {
                        response = Constants.FILE_INPUT_PARTIAL;
                        errorCount++;
                    }
                }
            }

            if (errorCount >= lines.Length)
            {
                response = Constants.FILE_INPUT_NO_GOOD;
            }

            if(response == Constants.FILE_INPUT_NO_GOOD || response == Constants.FILE_INPUT_PARTIAL)
            {
                switch (filetype) 
                {
                    case Constants.PARTICIPANT:
                        response += Constants.PROPER_PARTICIPANT_FILE;
                        break;
                    case Constants.STATION:
                        response += Constants.PROPER_STATION_FILE;
                        break;
                    case Constants.CHECKIN:
                        response += Constants.PROPER_CHECKIN_FILE;
                        break;
                }
            }

            return response;
        }
    }
}
