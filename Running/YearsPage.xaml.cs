using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Diagnostics;
using System.ComponentModel;



namespace Running
{
    public class Year
    {
        public int YearNum { get; set; }
        public double Distance { get; set; }
        public Year(int yearNum, double distance)
        {
            YearNum = yearNum;
            Distance = distance;
        }
        public override string ToString()
        {
            return this.YearNum + " - " + this.Distance + " " + EventPage.unit;
        }
    }

    public partial class YearsPage : ContentPage
    {
        public static int yearClicked;
        public static List<Year> years;
        public YearsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, "Home");
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            years = new List<Year>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryYear = date.Year;
                bool yearExists = false;
                foreach (Year entry in years)
                {
                    if (entry.YearNum == entryYear)
                    {
                        yearExists = true;
                    }
                }
                if (!yearExists)
                {
                    Year newYear = new Year(entryYear, double.Parse(toks[0]));
                    years.Add(newYear);
                }
                else
                {
                    double newDistance = double.Parse(toks[0]);
                    double oldDistance = years.First(d => d.YearNum == entryYear).Distance;
                    years.First(d => d.YearNum == entryYear).Distance = oldDistance + newDistance;
                }
                List<string> res = new List<string>();
                foreach (Year item in years)
                {
                    res.Add(item.ToString());
                }
                lvYears.ItemsSource = res;
            }
        }
        MonthsPage months;
        private async void ItemClicked(object sender, EventArgs e)
        {
            string rowClicked = lvYears.SelectedItem.ToString();
            string strYear = rowClicked.Split(' ')[0];
            yearClicked = Int32.Parse(strYear);
            months = new MonthsPage();
            await Navigation.PushAsync(months, true);
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            bool isNowLandscape = width > height;
            if (isNowLandscape)
            {
                test.Text = "";
                lvYears.WidthRequest = 200;
                view2.WidthRequest = 500;
                outerLayout.Orientation = StackOrientation.Horizontal;
                outerLayout.Padding = 10;
            }
            else
            {
                test.Text = "Years";
                outerLayout.Orientation = StackOrientation.Vertical;
                lvYears.WidthRequest = 300;
                //view2.HorizontalOptions = LayoutOptions.CenterAndExpand;
            }
        }
            private void PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
            {
            List<Running.Event> totals = new List<Running.Event>();
            totals = EventPage.conn.Table<Event>().ToList();
            years = new List<Year>();
            foreach (Running.Event row in totals)
            {
                string[] toks = row.ToString().Split(' ');
                DateTime date = DateTime.Parse(toks[1]);
                int entryYear = date.Year;
                bool yearExists = false;
                foreach (Year entry in years)
                {
                    if (entry.YearNum == entryYear)
                    {
                        yearExists = true;
                    }
                }
                if (!yearExists)
                {
                    Year newYear = new Year(entryYear, double.Parse(toks[0]));
                    years.Add(newYear);
                }
                else
                {
                    double newDistance = double.Parse(toks[0]);
                    double oldDistance = years.First(d => d.YearNum == entryYear).Distance;
                    years.First(d => d.YearNum == entryYear).Distance = oldDistance + newDistance;
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
            foreach (Year item in years)
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