using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Running
{
    public class Month
    {
        public int MonthNum { get; set; }
        public double Distance { get; set; }
        public Month(int monthNum, double distance)
        {
            MonthNum = monthNum;
            Distance = distance;
        }
        public override string ToString()
        {
            string monthString = "";
            if (this.MonthNum == 1)
            {
                monthString = "January";
            }
            else if (this.MonthNum == 2)
            {
                monthString = "February";
            }
            else if (this.MonthNum == 3)
            {
                monthString = "March";
            }
            else if (this.MonthNum == 4)
            {
                monthString = "April";
            }
            else if (this.MonthNum == 5)
            {
                monthString = "May";
            }
            else if (this.MonthNum == 6)
            {
                monthString = "June";
            }
            else if (this.MonthNum == 7)
            {
                monthString = "July";
            }
            else if (this.MonthNum == 8)
            {
                monthString = "August";
            }
            else if (this.MonthNum == 9)
            {
                monthString = "September";
            }
            else if (this.MonthNum == 10)
            {
                monthString = "October";
            }
            else if (this.MonthNum == 11)
            {
                monthString = "November";
            }
            else if (this.MonthNum == 12)
            {
                monthString = "December";
            }
            return monthString + " - " + this.Distance + " " + EventPage.unit;
        }
    }

    public partial class MonthsPage : ContentPage
    {
        public static int monthClicked;
        public static List<Month> months;
        public MonthsPage()
        {
            InitializeComponent();
            //test.Text = YearsPage.yearClicked.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            months = new List<Month>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryMonth = date.Month;
                int entryYear = date.Year;
                bool monthExists = false;
                if (date.Year == YearsPage.yearClicked)
                {
                    foreach (Month entry in months)
                    {
                        if (entry.MonthNum == entryMonth)
                        {
                            monthExists = true;
                        }
                    }
                    if (!monthExists)
                    {
                        Month newMonth = new Month(entryMonth, double.Parse(toks[0]));
                        months.Add(newMonth);
                    }
                    else
                    {
                        double newDistance = double.Parse(toks[0]);
                        double oldDistance = months.First(d => d.MonthNum == entryMonth).Distance;
                        months.First(d => d.MonthNum == entryMonth).Distance = oldDistance + newDistance;
                    }
                }
                List<string> res = new List<string>();
                foreach (Month item in months)
                {
                    res.Add(item.ToString());
                }
                lvMonths.ItemsSource = res;
            }
        }
        WeeksPage weeks;
        private async void ItemClicked(object sender, EventArgs e)
        {
            string rowClicked = lvMonths.SelectedItem.ToString();
            string strMonth = rowClicked.Split(' ')[0];
            int tmp = 1;
            if (strMonth == "January")
            {
                tmp = 1;
            }
            else if (strMonth == "February")
            {
                tmp = 2;
            }
            else if (strMonth == "March")
            {
                tmp = 3;
            }
            else if (strMonth == "April")
            {
                tmp = 4;
            }
            else if (strMonth == "May")
            {
                tmp = 5;
            }
            else if (strMonth == "June")
            {
                tmp = 6;
            }
            else if (strMonth == "July")
            {
                tmp = 7;
            }
            else if (strMonth == "August")
            {
                tmp = 8;
            }
            else if (strMonth == "September")
            {
                tmp = 9;
            }
            else if (strMonth == "October")
            {
                tmp = 10;
            }
            else if (strMonth == "November")
            {
                tmp = 11;
            }
            else if (strMonth == "December")
            {
                tmp = 12;
            }
            monthClicked = tmp;
            weeks = new WeeksPage();
            await Navigation.PushAsync(weeks, true);
        }
        private void PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            months = new List<Month>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryMonth = date.Month;
                int entryYear = date.Year;
                bool monthExists = false;
                if (date.Year == YearsPage.yearClicked)
                {
                    foreach (Month entry in months)
                    {
                        if (entry.MonthNum == entryMonth)
                        {
                            monthExists = true;
                        }
                    }
                    if (!monthExists)
                    {
                        Month newMonth = new Month(entryMonth, double.Parse(toks[0]));
                        months.Add(newMonth);
                    }
                    else
                    {
                        double newDistance = double.Parse(toks[0]);
                        double oldDistance = months.First(d => d.MonthNum == entryMonth).Distance;
                        months.First(d => d.MonthNum == entryMonth).Distance = oldDistance + newDistance;
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
                foreach (Month item in months)
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
