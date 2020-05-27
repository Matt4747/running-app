using System;
using System.Collections.Generic;
using Xamarin.Essentials;

using Xamarin.Forms;

namespace Running
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey("Gender"))
            {
                Preferences.Set("Gender", "Female");
            }
            if (!Preferences.ContainsKey("Unit"))
            {
                Preferences.Set("Unit", false);
            }           
            bool b = Preferences.Get("Unit", false);
            isKilometers.IsToggled = Preferences.Get("Unit", false);
            if (Preferences.Get("Gender", "Female") == "Female")
            {
                gender.SelectedIndex = 0;
            }
            else
            {
                gender.SelectedIndex = 1;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Preferences.Get("Gender", "Female") == "Female")
            {
                gender.SelectedIndex = 0;
            }
            else
            {
                gender.SelectedIndex = 1;
            }
        }

        void gender_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            Preferences.Set("Gender", gender.SelectedItem.ToString());
            //DOB.Text = gender.SelectedItem.ToString();
        }

        void UnitToggled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            
            EventPage.checkUnit();
            bool b = Preferences.Get("Unit", false);
            if (isKilometers.IsToggled)
            {
                EventPage.unit = "Kilometers";
                if (b != true)
                {
                    EventPage.conn.Execute("UPDATE " + "event " + "SET " + "Distance = Distance * 1.609344");
                }
            }
            else
            {
                EventPage.unit = "Miles";
                EventPage.conn.Execute("UPDATE " + "event " + "SET " + "Distance = Distance * 0.621371192237334");
            }
            Preferences.Set("Unit", isKilometers.IsToggled);
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.miamioh.edu"));
        }

    }
}
