using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using SQLite;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace Running
{
    public partial class EventPage : ContentPage
    {
        public static string unit;
        public static SQLiteConnection conn;
        public EventPage()
        {
            InitializeComponent();
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, "Events.db");
            conn = new SQLiteConnection(fname);
            conn.CreateTable<Event>();
            GenerateRunData(2015, 5);
            unit = checkUnit();
            currentUnit.Text = unit;
        }
        public static string checkUnit()
        {
            bool b = Preferences.Get("Unit", false);
            if (b == true)
            {
                return "Kilometers";
            }
            else
            {
                return "Miles";
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool b = Preferences.Get("Unit", false);
            if (b == true)
            {
                currentUnit.Text="Kilometers";
            }
            else
            {
                currentUnit.Text="Miles";
            }
            lvActivities.ItemsSource = conn.Table<Event>().ToList();
        }

        void AddClicked(System.Object sender, System.EventArgs e)
        {
            string fullTime = hours.Text + ":" + minutes.Text + ":" + seconds.Text;
            string fullDate = datePicked.Date.Month + "/" + datePicked.Date.Day + "/" + datePicked.Date.Year;
            double.TryParse(distanceSecond.Text, out double dec);
            double distance = int.Parse(distanceFirst.Text) + (dec / 10);

            Event newEvent = new Event { Date = fullDate, Distance = distance, Time = fullTime};
            conn.Insert(newEvent);
            lvActivities.ItemsSource = conn.Table<Event>().ToList();
        }
        void DeleteClicked(object sender, EventArgs e)
        {
            Event event1 = lvActivities.SelectedItem as Event;
            if (event1 != null)
            {
                int v = conn.Delete(event1);
                if (v > 0)
                {
                    lvActivities.SelectedItem = null;
                    lvActivities.ItemsSource = conn.Table<Event>().ToList();
                }
            }
        }
        void UpdateClicked(object sender, EventArgs e)
        {
            string fullTime = hours.Text + ":" + minutes.Text + ":" + seconds.Text;
            string fullDate = datePicked.Date.Month + "/" + datePicked.Date.Day + "/" + datePicked.Date.Year;
            double.TryParse(distanceSecond.Text, out double dec);
            double distance = int.Parse(distanceFirst.Text) + (dec / 10);

            Event oldEvent = lvActivities.SelectedItem as Event;
            Event newEvent = new Event
            {
                Date = fullDate,
                Distance = distance,
                Time = fullTime
            };
            newEvent.ID = oldEvent.ID;
            conn.Update(newEvent);
            lvActivities.ItemsSource = conn.Table<Event>().ToList();
        }

        void TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (!String.IsNullOrEmpty(e.NewTextValue) && !Int32.TryParse(e.NewTextValue, out int parsedInt))
            {
                entry.Text = e.OldTextValue;
            }
        }
        /*
 * The following code should be used to populate the database
 * of runs. Do not change this code.
 */
        public static void GenerateRunData(int startYear, int numYears)
        {
            const double baseMileage = 3.0;
            for (int dy = 0; dy < numYears; dy++)
            {
                int year = startYear + dy;
                double yearAdjustment = 1.0 + dy * 0.01;
                for (int month = 1; month <= 12; month++)
                {
                    double monthAdjustment = 1.0 + month * 0.02;
                    for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
                    {
                        DateTime date = new DateTime(year, month, day);
                        double length = 0;
                        switch (date.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                continue;
                                break;
                            case DayOfWeek.Tuesday:
                                length = baseMileage;
                                break;
                            case DayOfWeek.Wednesday:
                                length = 1.5 * baseMileage;
                                break;
                            case DayOfWeek.Thursday:
                                length = 2 * baseMileage;
                                break;
                            case DayOfWeek.Friday:
                                length = 2 * baseMileage;
                                break;
                            case DayOfWeek.Saturday:
                                length = baseMileage;
                                break;
                            case DayOfWeek.Sunday:
                                length = 4 * baseMileage;
                                break;
                        }
                        int runLengthInMiles = (int)Math.Round(yearAdjustment * monthAdjustment * length);
                        int secondsToCompleteRun = runLengthInMiles * 480;      // 8 minutes per mile
                                                                                // Instead of printing, you should insert the run into your database.
                        int hours = secondsToCompleteRun / 3600;
                        int mins = (secondsToCompleteRun % 3600) / 60;
                        int secs = secondsToCompleteRun % 60;
                        string time = hours + ":" + mins + ":" + secs;
                        string fullDate = date.Date.Month + "/" + date.Date.Day + "/" + date.Date.Year;
                        Event newEvent = new Event { Date = fullDate, Distance = (double)runLengthInMiles, Time = time };
                        conn.Insert(newEvent);
                    }
                }
            }
        }
    }
}
