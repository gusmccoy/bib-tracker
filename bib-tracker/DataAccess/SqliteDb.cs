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

        //        public static void UpdateNote(Note note)
        //        {
        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand();
        //                cmd.Connection = conn;

        //                cmd.CommandText = "UPDATE note SET title = @Title, content = @Content WHERE noteId = @Id";
        //                cmd.Parameters.AddWithValue("@Title", note.Title);
        //                cmd.Parameters.AddWithValue("@Content", note.Content);
        //                cmd.Parameters.AddWithValue("@Id", note.Id);

        //                cmd.ExecuteReader();

        //                conn.Close();
        //            }
        //        }

        //        public static void DeleteNote(long noteId)
        //        {
        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand();
        //                cmd.Connection = conn;
        //                cmd.CommandText = "DELETE FROM note WHERE noteId = @Id";
        //                cmd.Parameters.AddWithValue("@Id", noteId);
        //                cmd.ExecuteReader();

        //                conn.Close();
        //            }
        //        }

        public static long AddStation(Station station)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;
                long newId = 0;

                if (GetStationByName(station.Name).Name == null)
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

        public static Station GetStationByName(string name)
        {
            var station = new Station();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, name FROM participant WHERE name = @name", conn);
                cmd.Parameters.AddWithValue("@name", name);
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

        //        public static List<Folder> GetAllFolders()
        //        {
        //            var folders = new List<Folder>();

        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand("SELECT folderId, folderName FROM folder", conn);
        //                SqliteDataReader query = cmd.ExecuteReader();
        //                while (query.Read())
        //                {
        //                    folders.Add(new Folder
        //                    {
        //                        Id = query.GetInt32(0),
        //                        Name = query.GetString(1)
        //                    });
        //                }
        //                conn.Close();
        //            }

        //            return folders;
        //        }

        //        public static void UpdateFolder(Folder folder)
        //        {
        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand();
        //                cmd.Connection = conn;

        //                cmd.CommandText = "UPDATE folder SET folderName = @Name WHERE folderId = @Id";
        //                cmd.Parameters.AddWithValue("@Name", folder.Name);
        //                cmd.Parameters.AddWithValue("@Id", folder.Id);

        //                cmd.ExecuteReader();

        //                conn.Close();
        //            }
        //        }

        //        public static void DeleteFolder(long folderId)
        //        {
        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand();
        //                cmd.Connection = conn;
        //                cmd.CommandText = "DELETE FROM folder WHERE folderId = @Id";
        //                cmd.Parameters.AddWithValue("@Id", folderId);
        //                cmd.ExecuteReader();

        //                conn.Close();
        //            }
        //        }

        //        public static List<int> GetNoteIdsByFolderId(int id)
        //        {
        //            var noteIds = new List<int>();

        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand("SELECT noteId FROM note_folder_association WHERE folderId = @Id", conn);
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                SqliteDataReader query = cmd.ExecuteReader();
        //                while (query.Read())
        //                {
        //                    noteIds.Add(query.GetInt32(0));
        //                }
        //                conn.Close();
        //            }

        //            return noteIds;
        //        }

        //        public static int GetFolderIdByNoteId(int id)
        //        {
        //            int folderId = 0;

        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand("SELECT folderId FROM note_folder_association WHERE noteId = @Id", conn);
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                SqliteDataReader query = cmd.ExecuteReader();
        //                while (query.Read())
        //                {
        //                    folderId = query.GetInt32(0);
        //                }
        //                conn.Close();
        //            }

        //            return folderId;
        //        }

        public static long AddParticipantCheckIn(ParticipantCheckIn participantCheckIn)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();
                var newId = (long)0;

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                if (GetCheckinByStationAndParticipant(participantCheckIn.StationId, participantCheckIn.ParticipantId).ParticipantId == 0)
                {
                    // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
                    cmd.CommandText = "INSERT INTO participant_check_in (participantId, stationId) VALUES (@ParticipantId, @StationId); SELECT last_insert_rowid()";
                    cmd.Parameters.AddWithValue("@ParticipantId", participantCheckIn.ParticipantId);
                    cmd.Parameters.AddWithValue("@StationId", participantCheckIn.StationId);

                    // Get ID that was automatically assigned
                    newId = (long)cmd.ExecuteScalar();
                }
                conn.Close();

                return newId;
            }
        }

        public static ParticipantCheckIn GetCheckinByStationAndParticipant(int stationName, int bib)
        {
            var participantCheckIn = new ParticipantCheckIn();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand("SELECT id, participantId, stationId FROM participant_check_in WHERE participantId = @ParticipantId" +
                    "AND stationId = @StationId", conn);
                cmd.Parameters.AddWithValue("@ParticipantId", bib);
                cmd.Parameters.AddWithValue("@StationId", stationName);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participantCheckIn = new ParticipantCheckIn
                    {
                        Id = query.GetInt32(0),
                        ParticipantId = query.GetInt32(1),
                        StationId = query.GetInt32(2)
                    };
                }
                conn.Close();
            }
            return participantCheckIn;
        }

        //        public static void DeleteNoteFolderAssociationByFolderId(long folderId)
        //        {
        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand();
        //                cmd.Connection = conn;
        //                cmd.CommandText = "DELETE FROM note_folder_association WHERE folderId = @Id";
        //                cmd.Parameters.AddWithValue("@Id", folderId);
        //                cmd.ExecuteReader();

        //                conn.Close();
        //            }
        //        }

        //        public static void DeleteNoteFolderAssociationByNoteId(long noteId)
        //        {
        //            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
        //            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
        //            {
        //                conn.Open();

        //                SqliteCommand cmd = new SqliteCommand();
        //                cmd.Connection = conn;
        //                cmd.CommandText = "DELETE FROM note_folder_association WHERE noteId = @Id";
        //                cmd.Parameters.AddWithValue("@Id", noteId);
        //                cmd.ExecuteReader();

        //                conn.Close();
        //            }
        //        }
        //    }
    }
}
