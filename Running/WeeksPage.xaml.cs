using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Running
{
    public class Week
    {
        public int WeekNum { get; set; }
        public double Distance { get; set; }
        public Week(int weekNum, double distance)
        {
            WeekNum = weekNum;
            Distance = distance;
        }
        public override string ToString()
        {
            return "Week " + this.WeekNum + " - " + this.Distance + " " + EventPage.unit;
        }
    }

    public partial class WeeksPage : ContentPage
    {
        public static int weekClicked;
        public WeeksPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            List<Week> weeks = new List<Week>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryDay = date.Day;
                int entryWeek = 1;
                if (entryDay > 7 && entryDay <= 14)
                {
                    entryWeek = 2;
                }
                if (entryDay > 14 && entryDay <= 21)
                {
                    entryWeek = 3;
                }
                if (entryDay > 21 && entryDay <= 28)
                {
                    entryWeek = 4;
                }
                if (entryDay > 28 && entryDay <= 31)
                {
                    entryWeek = 5;
                }
                bool weekExists = false;
                if (date.Month == MonthsPage.monthClicked && date.Year == YearsPage.yearClicked)
                {
                    foreach (Week entry in weeks)
                    {
                        if (entry.WeekNum == entryWeek)
                        {
                            weekExists = true;
                        }
                    }
                    if (!weekExists)
                    {
                        Week newWeek = new Week(entryWeek, double.Parse(toks[0]));
                        weeks.Add(newWeek);
                    }
                    else
                    {
                        double newDistance = double.Parse(toks[0]);
                        double oldDistance = weeks.First(d => d.WeekNum == entryWeek).Distance;
                        weeks.First(d => d.WeekNum == entryWeek).Distance = oldDistance + newDistance;
                    }
                }
                List<string> res = new List<string>();
                foreach (Week item in weeks)
                {
                    res.Add(item.ToString());
                }
                lvWeeks.ItemsSource = res;
            }
        }
        DaysPage days;
        private async void ItemClicked(object sender, EventArgs e)
        {
            string rowClicked = lvWeeks.SelectedItem.ToString();
            string strWeek = rowClicked.Split(' ')[1];
            weekClicked = Int32.Parse(strWeek);
            days = new DaysPage();
            await Navigation.PushAsync(days, true);
        }
        private void PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            List<Week> weeks = new List<Week>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryDay = date.Day;
                int entryWeek = 1;
                if (entryDay > 7 && entryDay <= 14)
                {
                    entryWeek = 2;
                }
                if (entryDay > 14 && entryDay <= 21)
                {
                    entryWeek = 3;
                }
                if (entryDay > 21 && entryDay <= 28)
                {
                    entryWeek = 4;
                }
                if (entryDay > 28 && entryDay <= 31)
                {
                    entryWeek = 5;
                }
                bool weekExists = false;
                if (date.Month == MonthsPage.monthClicked && date.Year == YearsPage.yearClicked)
                {
                    foreach (Week entry in weeks)
                    {
                        if (entry.WeekNum == entryWeek)
                        {
                            weekExists = true;
                        }
                    }
                    if (!weekExists)
                    {
                        Week newWeek = new Week(entryWeek, double.Parse(toks[0]));
                        weeks.Add(newWeek);
                    }
                    else
                    {
                        double newDistance = double.Parse(toks[0]);
                        double oldDistance = weeks.First(d => d.WeekNum == entryWeek).Distance;
                        weeks.First(d => d.WeekNum == entryWeek).Distance = oldDistance + newDistance;
                    }
                }
                List<int> distList = new List<int>();
                int total = 0;
                int count = 0;
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;
                float sep = info.Width * 0.20f;
                canvas.Clear();
                SKPaint paintA = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Red.ToSKColor(),
                    StrokeWidth = 3
                };
                SKPaint paintB = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Blue.ToSKColor(),
                    StrokeWidth = 3
                };
                foreach (Week item in weeks)
                {
                    distList.Add((int)item.Distance);
                    total += (int)item.Distance;
                    float percItem = (int)item.Distance / (float)total;
                    float r = Math.Min(info.Width, info.Height) / 2.0f;
                    float barWidth = info.Width * 0.20f;
                    float height = info.Height * percItem;
                    if (count > 0)
                    {
                        sep = sep + 150;
                    }
                    if (count % 2 == 0)
                    {
                        if (count == 0)
                        {
                            canvas.DrawRect(barWidth, height, barWidth, info.Height, paintA);
                        }
                        else
                        {
                            canvas.DrawRect(sep, height, barWidth, info.Height, paintA);
                        }
                        count++;
                    }
                    else
                    {
                        if (count == 0)
                        {
                            canvas.DrawRect(barWidth, height, barWidth, info.Height, paintB);
                        }
                        else
                        {
                            canvas.DrawRect(sep, height - height, barWidth, info.Height, paintB);
                        }
                        count++;
                    }
                }
            }
        }
    }
}
