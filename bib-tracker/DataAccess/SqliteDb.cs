using bib_tracker.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace bib_tracker.DataAccess
{
    class SqliteDb
    {
        public const string DB_FILENAME = "bib_tracker.db";

        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(DB_FILENAME, CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                var sql = "CREATE TABLE IF NOT EXISTS participant " +
                    "(id INTEGER PRIMARY KEY, " +
                    "bib INTEGER, " +
                    "firstName NVARCHAR(20), " +
                    "lastName NVARCHAR(20));" +

                    "CREATE TABLE IF NOT EXISTS station " +
                    "(id INTEGER PRIMARY KEY, " +
                    "name NVARCHAR(100));" +

                    "CREATE TABLE IF NOT EXISTS participant_check_in " +
                    "(id INTEGER PRIMARY KEY, " +
                    "participantId INTEGER, " +
                    "stationId INTEGER, " +
                    "timestamp DATETIME, " +
                    "FOREIGN KEY(participantId) REFERENCES participant(id), " +
                    "FOREIGN KEY(stationId) REFERENCES station(id));";

                var cmd = new SqliteCommand(sql, conn);
                cmd.ExecuteReader();
            }
        }

        public async static void ReadFileData(string fileType, StorageFile file)
        {
            switch (fileType)
            {
                case "PARTICIPANT":
                    ImportParticipantData(file);
                    break;
                case "STATION":
                    ImportStationData(file);
                    break;
                case "CHECKIN":
                    ImportCheckinData(file);
                    break;
            }
        }

        private async static void ImportParticipantData(StorageFile file)
        {
            string contents = await Windows.Storage.FileIO.ReadTextAsync(file);
            string[] lines = contents.Split('\r');
            foreach (string row in lines)
            {
                string[] line = row.Split('\t');
                AddParticipant(new Participant()
                {
                    Bib = Int32.Parse(line[0]),
                    FirstName = line[1],
                    LastName = line[2]
                });
            }
        }

        private async static void ImportStationData(StorageFile file)
        {
            string contents = await Windows.Storage.FileIO.ReadTextAsync(file);
            string[] lines = contents.Split('\r');
            foreach (string row in lines)
            {
                AddStation(new Station()
                {
                    Name = row
                });
            }
        }

        private async static void ImportCheckinData(StorageFile file)
        {
            string contents = await Windows.Storage.FileIO.ReadTextAsync(file);
            string[] lines = contents.Split('\r');
            foreach (string row in lines)
            {
                string[] line = row.Split('\t');
                AddParticipantCheckIn(new ParticipantCheckIn()
                {
                    ParticipantId = Int32.Parse(line[0]),
                    StationId = Int32.Parse(line[1])
                });
            }
        }

        public static long AddParticipant(Participant participant)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                long newId = 0;

                if (GetParticipantById(participant.Bib).FirstName == null)
                {
                    // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
                    cmd.CommandText = "INSERT INTO participant (bib, firstName, lastName) VALUES (@Bib, @FirstName, @LastName); SELECT last_insert_rowid()";
                    cmd.Parameters.AddWithValue("@Bib", participant.Bib);
                    cmd.Parameters.AddWithValue("@FirstName", participant.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", participant.LastName);

                    // Get ID that was automatically assigned
                    newId = (long)cmd.ExecuteScalar();
                }

                conn.Close();

                return newId;
            }
        }

        public static Participant GetParticipantById(int id)
        {
            var participant = new Participant();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, bib, firstName, lastName FROM participant WHERE bib = @bib", conn);
                cmd.Parameters.AddWithValue("@bib", id);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participant = new Participant
                    {
                        Id = query.GetInt32(0),
                        Bib = query.GetInt32(1),
                        FirstName = query.GetString(2),
                        LastName = query.GetString(3)
                    };
                }
                conn.Close();
            }

            return participant;
        }

        public static List<Participant> GetAllParticipants()
        {
            var participants = new List<Participant>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, bib, firstName, lastName FROM participant", conn);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participants.Add(new Participant
                    {
                        Id = query.GetInt32(0),
                        Bib = query.GetInt32(1),
                        FirstName = query.GetString(2),
                        LastName = query.GetString(3)
                    });
                }
                conn.Close();
            }

            return participants;
        }

        public static void UpdateParticipant(Participant participant)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                cmd.CommandText = "UPDATE participant SET firstName = @FirstName, lastName = @LastName, bib = @Bib WHERE id = @Id";
                cmd.Parameters.AddWithValue("@FirstName", participant.FirstName);
                cmd.Parameters.AddWithValue("@LastName", participant.LastName);
                cmd.Parameters.AddWithValue("@Bib", participant.Bib);
                cmd.Parameters.AddWithValue("@Id", participant.Id);

                cmd.ExecuteReader();

                conn.Close();
            }
        }

        public static void DeleteParticipant(long participantId)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM participant WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Id", participantId);
                cmd.ExecuteReader();

                conn.Close();
            }
        }

        public static long AddStation(Station station)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                long newId = 0;

                if (GetStationById(station.Id).Name == null)
                {

                    // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
                    cmd.CommandText = "INSERT INTO station (name) VALUES (@Name); SELECT last_insert_rowid()";
                    cmd.Parameters.AddWithValue("@Name", station.Name);

                    // Get ID that was automatically assigned
                    newId = (long)cmd.ExecuteScalar();

                }
                conn.Close();

                return newId;
            }
        }

        public static Station GetStationById(long stationId)
        {
            var station = new Station();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, name FROM station WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", stationId);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    station = new Station
                    {
                        Id = query.GetInt32(0),
                        Name = query.GetString(1)
                    };
                }
                conn.Close();
            }
            return station;
        }

        public static List<Station> GetAllStations()
        {
            var stations = new List<Station>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, name FROM station", conn);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    stations.Add(new Station
                    {
                        Id = query.GetInt32(0),
                        Name = query.GetString(1)
                    });
                }
                conn.Close();
            }

            return stations;
        }

        public static void UpdateStation(Station station)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                cmd.CommandText = "UPDATE station SET name = @Name WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Name", station.Name);
                cmd.Parameters.AddWithValue("@Id", station.Id);

                cmd.ExecuteReader();

                conn.Close();
            }
        }

        public static void DeleteStation(long stationId)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM station WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Id", stationId);
                cmd.ExecuteReader();

                conn.Close();
            }
        }

        public static List<ParticipantCheckIn> GetAllParticipantsCheckIns()
        {
            var participantCheckIns = new List<ParticipantCheckIn>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, participantId, stationId, timestamp FROM participant_check_in", conn);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participantCheckIns.Add(new ParticipantCheckIn
                    {
                        Id = query.GetInt32(0),
                        ParticipantId = query.GetInt32(1),
                        StationId = query.GetInt32(2),
                        Timestamp = query.GetDateTime(3)
                    });
                }
                conn.Close();
            }
            return participantCheckIns;
        }

        public static long AddParticipantCheckIn(ParticipantCheckIn participantCheckIn)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();
                var newId = (long)0;

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                //if (GetCheckinByStationAndParticipant(participantCheckIn.StationId, participantCheckIn.ParticipantId).ParticipantId == 0)
                //{
                    // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
                    cmd.CommandText = "INSERT INTO participant_check_in (participantId, stationId, timestamp) VALUES (@ParticipantId, @StationId, @Timestamp); SELECT last_insert_rowid()";
                    cmd.Parameters.AddWithValue("@ParticipantId", participantCheckIn.ParticipantId);
                    cmd.Parameters.AddWithValue("@StationId", participantCheckIn.StationId);
                    cmd.Parameters.AddWithValue("@Timestamp", participantCheckIn.Timestamp);

                    // Get ID that was automatically assigned
                    newId = (long)cmd.ExecuteScalar();
                //}
                conn.Close();

                return newId;
            }
        }

        public static List<ParticipantCheckIn> GetCheckinsByStationId(int stationName)
        {
            var participantCheckIns = new List<ParticipantCheckIn>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, participantId, stationId, timestamp FROM participant_check_in WHERE stationId = @StationId", conn);
                cmd.Parameters.AddWithValue("@StationId", stationName);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participantCheckIns.Add(new ParticipantCheckIn
                    {
                        Id = query.GetInt32(0),
                        ParticipantId = query.GetInt32(1),
                        StationId = query.GetInt32(2),
                        Timestamp = query.GetDateTime(3)
                    });
                }
                conn.Close();
            }
            return participantCheckIns;
        }

        public static List<ParticipantCheckIn> GetCheckinsByParticipantId(int participantId)
        {
            var participantCheckIns = new List<ParticipantCheckIn>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, participantId, stationId, timestamp FROM participant_check_in WHERE participantId = @ParticipantId" +
                    "AND stationId = @StationId", conn);
                cmd.Parameters.AddWithValue("@ParticipantId", participantId);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participantCheckIns.Add(new ParticipantCheckIn
                    {
                        Id = query.GetInt32(0),
                        ParticipantId = query.GetInt32(1),
                        StationId = query.GetInt32(2),
                        Timestamp = query.GetDateTime(3)
                    });
                }
                conn.Close();
            }
            return participantCheckIns;
        }

        public static ParticipantCheckIn GetCheckinByStationAndParticipant(int stationId, int participantId)
        {
            var checkIn = new ParticipantCheckIn();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, participantId, stationId, timestamp FROM participant_check_in WHERE participantId = @ParticipantId AND @stationId = StationId" +
                    "AND stationId = @StationId", conn);
                cmd.Parameters.AddWithValue("@ParticipantId", participantId);
                cmd.Parameters.AddWithValue("@StationId", stationId);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    checkIn = new ParticipantCheckIn
                    {
                        Id = query.GetInt32(0),
                        ParticipantId = query.GetInt32(1),
                        StationId = query.GetInt32(2),
                        Timestamp = query.GetDateTime(3)
                    };
                }
                conn.Close();
            }
            return checkIn;
        }

        public static void DeleteParticipantCheckInByParticipantId(long id)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM participant_check_in WHERE participant = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteReader();

                conn.Close();
            }
        }

        public static void DeleteParticipantCheckInByStationId(long id)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM participant_check_in WHERE stationId = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteReader();

                conn.Close();
            }
        }
    }
}
