using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Running
{
    public class Day
    {
        public int DayNum { get; set; }
        public double Distance { get; set; }
        public string DayOfWeek { get; set; }
        public Day(int dayNum, double distance, string dayOfWeek)
        {
            DayNum = dayNum;
            Distance = distance;
            DayOfWeek = dayOfWeek;
        }
        public override string ToString()
        {
            return this.DayOfWeek + " - " + this.Distance + " " + EventPage.unit;
        }
    }

    public partial class DaysPage : ContentPage
    {
        public static int dayClicked;

        public DaysPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Test.Text = WeeksPage.weekClicked.ToString();
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            List<Day> days = new List<Day>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryDay = date.Day;
                bool correctWeek = false;
                if (WeeksPage.weekClicked == 1 && (entryDay <= 7))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 2 && (entryDay > 7 && entryDay <= 14))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 3 && (entryDay > 14 && entryDay <= 21))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 4 && (entryDay > 21 && entryDay <= 28))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 5 && entryDay > 28 && entryDay <= 31)
                {
                    correctWeek = true;
                }
                bool dayExists = false;
                if (date.Month == MonthsPage.monthClicked && date.Year == YearsPage.yearClicked && correctWeek)
                {
                    foreach (Day entry in days)
                    {
                        if (entry.DayNum == entryDay)
                        {
                            dayExists = true;
                        }
                    }
                    if (!dayExists)
                    {
                        Day newDay = new Day(entryDay, double.Parse(toks[0]), date.DayOfWeek.ToString());
                        days.Add(newDay);
                    }
                    else
                    {
                        double newDistance = double.Parse(toks[0]);
                        double oldDistance = days.First(d => d.DayNum == entryDay).Distance;
                        days.First(d => d.DayNum == entryDay).Distance = oldDistance + newDistance;
                    }
                }
                List<string> res = new List<string>();
                foreach (Day item in days)
                {
                    res.Add(item.ToString());
                }
                lvDays.ItemsSource = res;
            }
        }
        private void PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            List<Day> days = new List<Day>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryDay = date.Day;
                bool correctWeek = false;
                if (WeeksPage.weekClicked == 1 && (entryDay <= 7))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 2 && (entryDay > 7 && entryDay <= 14))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 3 && (entryDay > 14 && entryDay <= 21))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 4 && (entryDay > 21 && entryDay <= 28))
                {
                    correctWeek = true;
                }
                else if (WeeksPage.weekClicked == 5 && entryDay > 28 && entryDay <= 31)
                {
                    correctWeek = true;
                }
                bool dayExists = false;
                if (date.Month == MonthsPage.monthClicked && date.Year == YearsPage.yearClicked && correctWeek)
                {
                    foreach (Day entry in days)
                    {
                        if (entry.DayNum == entryDay)
                        {
                            dayExists = true;
                        }
                    }
                    if (!dayExists)
                    {
                        Day newDay = new Day(entryDay, double.Parse(toks[0]), date.DayOfWeek.ToString());
                        days.Add(newDay);
                    }
                    else
                    {
                        double newDistance = double.Parse(toks[0]);
                        double oldDistance = days.First(d => d.DayNum == entryDay).Distance;
                        days.First(d => d.DayNum == entryDay).Distance = oldDistance + newDistance;
                    }
                }
                List<string> res = new List<string>();
                foreach (Day item in days)
                {
                    res.Add(item.ToString());
                }
                lvDays.ItemsSource = res;
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
                foreach (Day item in days)
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
    
