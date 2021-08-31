using bib_tracker.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public static long AddParticipant(Participant participant)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
            {
                conn.Open();

                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
                cmd.CommandText = "INSERT INTO participant (bib, firstName, lastName) VALUES (@Bib, @FirstName, @LastName); SELECT last_insert_rowid()";
                cmd.Parameters.AddWithValue("@Bib", participant.Bib);
                cmd.Parameters.AddWithValue("@FirstName", participant.FirstName);
                cmd.Parameters.AddWithValue("@LastName", participant.LastName);

                // Get ID that was automatically assigned
                var newId = (long)cmd.ExecuteScalar();

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

                SqliteCommand cmd = new SqliteCommand("SELECT bib, firstName, lastName FROM participant WHERE id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqliteDataReader query = cmd.ExecuteReader();
                while (query.Read())
                {
                    participant = new Participant
                    {
                        Id = id,
                        Bib = query.GetInt32(0),
                        FirstName = query.GetString(1),
                        LastName = query.GetString(2)
                    };
                }
                conn.Close();
            }

            return participant;
        }

//        public static List<Note> GetAllNotes()
//        {
//            var notes = new List<Note>();

//            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
//            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
//            {
//                conn.Open();

//                SqliteCommand cmd = new SqliteCommand("SELECT noteId, title, content FROM note", conn);
//                SqliteDataReader query = cmd.ExecuteReader();
//                while (query.Read())
//                {
//                    notes.Add(new Note
//                    {
//                        Id = query.GetInt32(0),
//                        Title = query.GetString(1),
//                        Content = query.GetString(2)
//                    });
//                }
//                conn.Close();
//            }

//            return notes;
//        }

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

//        public static long AddFolder(Folder folder)
//        {
//            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
//            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
//            {
//                conn.Open();

//                SqliteCommand cmd = new SqliteCommand();
//                cmd.Connection = conn;

//                // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
//                cmd.CommandText = "INSERT INTO folder (folderName) VALUES (@Name); SELECT last_insert_rowid()";
//                cmd.Parameters.AddWithValue("@Name", folder.Name);

//                // Get ID that was automatically assigned
//                var newId = (long)cmd.ExecuteScalar();

//                conn.Close();

//                return newId;
//            }
//        }

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

//        public static long AddNoteFolderAssociation(NoteFolderAssociation noteFolderAssociation)
//        {
//            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILENAME);
//            using (SqliteConnection conn = new SqliteConnection($"Filename={dbpath}"))
//            {
//                conn.Open();

//                SqliteCommand cmd = new SqliteCommand();
//                cmd.Connection = conn;

//                // NULL tells Sqlite to use autoincrement value. Parameterized query prevents SQL injection attacks
//                cmd.CommandText = "INSERT INTO note_folder_association (noteId, folderId) VALUES (@NoteId, @FolderId); SELECT last_insert_rowid()";
//                cmd.Parameters.AddWithValue("@NoteId", noteFolderAssociation.noteId);
//                cmd.Parameters.AddWithValue("@FolderId", noteFolderAssociation.folderId);

//                // Get ID that was automatically assigned
//                var newId = (long)cmd.ExecuteScalar();

//                conn.Close();

//                return newId;
//            }
//        }

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
