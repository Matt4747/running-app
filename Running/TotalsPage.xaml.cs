//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xamarin.Essentials;

//using Xamarin.Forms;

//namespace Running
//{
//    public class Week1
//    {
//        public string Monday { get; set; }
//        public double Distance { get; set; }
//        public string Time { get; set; }
//        public Week1(string monday, double distance, string time)
//        {
//            Monday = monday;
//            Distance = distance;
//            Time = time;
//        }
//        public override string ToString(){
//            return this.Monday + " " + this.Distance + " " + this.Time;
//            }
//    }
    
//    public partial class TotalsPage : ContentPage
//    {
//        public TotalsPage()
//        {
//            InitializeComponent();
//            //Page mainPage = new MainPage();
//            //mainPage = new NavigationPage(mainPage);
//        }
//        protected override void OnAppearing()
//        {
//            base.OnAppearing();
//            List<Running.Event> totals = new List<Running.Event>();
//            totals = EventPage.conn.Table<Event>().ToList();
//            List<Week> weeks = new List<Week>();
//            foreach (Running.Event row in totals)
//            {
//                string[] toks = row.ToString().Split(' ');
//                DateTime date = DateTime.Parse(toks[1]);
//                DateTime mondayDatetime = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
//                string[] tmp = mondayDatetime.ToString().Split(' ');
//                string monday = tmp[0];
//                bool weekExists = false;
//                foreach (Week1 entry in weeks)
//                {
//                    if (entry.Monday == monday)
//                    {
//                        weekExists = true;
//                    }
//                }
//                if (!weekExists)
//                {
//                    Week1 newWeek = new Week1(monday, double.Parse(toks[0]), toks[2]);
//                    weeks.Add(newWeek);
//                }
//                else
//                {
//                    double newDistance = double.Parse(toks[0]);
//                    double oldDistance = weeks.First(d => d.Monday == monday).Distance;

//                    string newTime = toks[2];
//                    string[] newTimeSplit = newTime.Split(':');
//                    int newHours = int.Parse(newTimeSplit[0]);
//                    int newMinutes = int.Parse(newTimeSplit[1]);
//                    int newSeconds = int.Parse(newTimeSplit[2]);
//                    int newTotalSeconds = (newHours * 3600) + (newMinutes * 60) + newSeconds;
//                    string oldTime = weeks.First(d => d.Monday == monday).Time;
//                    string[] oldTimeSplit = oldTime.Split(':');
//                    int oldHours = int.Parse(oldTimeSplit[0]);
//                    int oldMinutes = int.Parse(oldTimeSplit[1]);
//                    int oldSeconds = int.Parse(oldTimeSplit[2]);
//                    int oldTotalSeconds = (oldHours * 3600) + (oldMinutes * 60) + oldSeconds;
//                    int finalTotalSeconds = oldTotalSeconds + newTotalSeconds;
//                    string newTotalTime = (finalTotalSeconds / 3600) + ":" + (finalTotalSeconds / 60 % 60) + ":" + (finalTotalSeconds % 60);

//                    weeks.First(d => d.Monday == monday).Distance = oldDistance + newDistance;
//                    weeks.First(d => d.Monday == monday).Time = newTotalTime;
//                }
//                List<string> res = new List<string>();
//                foreach (Week item in weeks)
//                {
//                    res.Add(item.ToString());
//                }
//                    lvTotals.ItemsSource = res;
//            }
//        }
//        MonthsPage months;
//        private async void ItemClicked(object sender, EventArgs e)
//        {
//            months = new MonthsPage();
//            await Navigation.PushAsync(months, true);
//        }

//    }
//}
