//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using SQLite;
//using Xamarin.Essentials;
//using System.Reflection;

//namespace Running
//{
//    public class DB
//    {
//        private static string DBName = "log.db";
//        public static SQLiteConnection conn;
//        public static void OpenConnection()
//        {
//            string libFolder = FileSystem.AppDataDirectory;
//            string fname = System.IO.Path.Combine(libFolder, DBName);
//            conn = new SQLiteConnection(fname);
//            conn.CreateTable<Event>();
//        }
//        public static void DeleteTableContents(string tableName)
//        {
//            int v = conn.Execute("DELETE FROM " + tableName);
//        }
//        public static void RepopulateTables()
//        {
//            LoadActivities();
////            LoadLongDefinitions();
//        }
//        public static void LoadActivities()
//        {
//            try
//            {
//                DeleteTableContents("activity");
//                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
//                Stream stream = assembly.GetManifestResourceStream("TrainingDB.log.txt");
//                StreamReader input = new StreamReader(stream);
//                while (!input.EndOfStream)
//                {
//                    string line = input.ReadLine();
//                    Event event1 = Event.ParseCSV(line);
//                    conn.Insert(event1);
//                }
//            }
//            catch (Exception e)
//            {
//            }
//        }
//        //public static void LoadLongDefinitions()
//        //{
//        //    try
//        //    {
//        //        DeleteTableContents("longdefs");
//        //        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
//        //        Stream stream = assembly.GetManifestResourceStream("TrainingDB.long.txt");
//        //        StreamReader input = new StreamReader(stream);
//        //        while (!input.EndOfStream)
//        //        {
//        //            string line = input.ReadLine();
//        //            LongActivityDefinition activity = LongActivityDefinition.ParseCSV(line);
//        //            conn.Insert(activity);
//        //        }
//        //    }
//        //    catch (Exception e)
//        //    {
//        //    }
//        //}
//    }
//}
