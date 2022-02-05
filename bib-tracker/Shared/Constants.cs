using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bib_tracker.Shared
{
    public static class Constants
    {
        public const string PARTICIPANT = "PARTICIPANT";
        public const string STATION = "STATION";
        public const string CHECKIN = "CHECKIN";
        public const string FILE_INPUT_ALL_GOOD = "All data was read in correctly.";
        public const string FILE_INPUT_PARTIAL = "Info read in, but not all rows matched the following format: ";
        public const string FILE_INPUT_NO_GOOD = "There was no valid info in this file. Rows should be in the following format: ";
        public const string PROPER_PARTICIPANT_FILE = "BIB# <TAB> FIRSTNAME <TAB> LASTNAME <NEWLINE>";
        public const string PROPER_STATION_FILE = "STATION# <TAB> STATIONNAME <NEWLINE>";
        public const string PROPER_CHECKIN_FILE = "BIB# <TAB> STATION# <TAB> TIMESTAMP <NEWLINE>";
    }
}
