using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using System.Device.Location;

namespace VéloBleu
{
    public partial class listStation : PhoneApplicationPage
    {
        List<Station_Item> stations;
        List<Station_Item> stationsDispo;
        private Geolocator geolocator = null;


        public async void getLocation()
        {
            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 25;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10));
                GeoCoordinate pos = new GeoCoordinate(Convert.ToDouble(geoposition.Coordinate.Latitude), Convert.ToDouble(geoposition.Coordinate.Longitude));
                foreach (var query in stations)
                {
                    GeoCoordinate posStation = new GeoCoordinate(Convert.ToDouble(query.Lat), Convert.ToDouble(query.Lng));
                    double distanceInMeter;
                    distanceInMeter = pos.GetDistanceTo(posStation);
                    query.DistanceInMeter = distanceInMeter;                 
                }
                
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Le service de location est désactivé dans les paramètres du téléphone.");
            }
            catch (Exception ex)
            {
            }
        }

        public listStation()
        {
            InitializeComponent();
            stations = (List<Station_Item>) PhoneApplicationService.Current.State["stations"];
            stationsDispo = new List<Station_Item>();	//nouvelle liste
            
            //vérif stations disponible
            foreach (var query in stations)
            {
                int placesDispo = Convert.ToInt32(query.Ap);
                int velosDispo = Convert.ToInt32(query.Ab);
                if (query.Disp == "1")
                {
                    //code couleur places
                    if (placesDispo > 3)
                    {
                        query.ColorPlace = "Green";
                    }
                    else if (placesDispo > 0 && placesDispo <= 3)
                    {
                        query.ColorPlace = "Orange";
                    }
                    else if (placesDispo == 0)
                    {
                        query.ColorPlace = "Red";
                    }
                    //pluriel
                    if (placesDispo > 1)
                        query.Ap += " places";
                    else if (placesDispo <= 1)
                        query.Ap += " place";
                    //code couleur vélos
                    if (velosDispo > 3)
                    {
                        query.ColorVelo = "Green";
                    }
                    else if (velosDispo > 0 && velosDispo <= 3)
                    {
                        query.ColorVelo = "Orange";
                    }
                    else if (velosDispo == 0)
                    {
                        query.ColorVelo = "Red";
                    }
                    //pluriel
                    if (velosDispo > 1)
                        query.Ab += " vélos";
                    else if (velosDispo <= 1)
                        query.Ab += " vélo";

                    stationsDispo.Add(query);
                }
                listBox.ItemsSource = stationsDispo;             
            }
        }
        private string GetStatusString(PositionStatus status)
        {
            var strStatus = "";

            switch (status)
            {
                case PositionStatus.Ready:
                    strStatus = "Location is available.";
                    break;

                case PositionStatus.Initializing:
                    strStatus = "Geolocation service is initializing.";
                    break;

                case PositionStatus.NoData:
                    strStatus = "Location service data is not available.";
                    break;

                case PositionStatus.Disabled:
                    strStatus = "Location services are disabled. Use the " +
                                "Settings charm to enable them.";
                    break;

                case PositionStatus.NotInitialized:
                    strStatus = "Location status is not initialized because " +
                                "the app has not yet requested location data.";
                    break;

                case PositionStatus.NotAvailable:
                    strStatus = "Location services are not supported on your system.";
                    break;

                default:
                    strStatus = "Unknown PositionStatus value.";
                    break;
            }

            return (strStatus);

        }

        private void triVelo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // tri de la liste en fonction des vélos
            stationsDispo.Sort(new StationComparator(StationComparator.CompareType.VELO));
            // maj listebox
            listBox.ItemsSource = null;
            listBox.ItemsSource = stationsDispo;
            //il aurait été préférable d'utiliser un ObservableCollection et d'implémenter INotifyPropertyChanged

        }

        private void triPlace_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // tri de la liste en fonction des places
            stationsDispo.Sort(new StationComparator(StationComparator.CompareType.PLACE));
            // maj listebox
            listBox.ItemsSource = null;
            listBox.ItemsSource = stationsDispo;
            //il aurait été préférable d'utiliser un ObservableCollection et d'implémenter INotifyPropertyChanged
        }

        private void triDistance_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // tri de la liste en fonction des distances
            stationsDispo.Sort(new StationComparator(StationComparator.CompareType.DISTANCE));
            // maj listebox
            listBox.ItemsSource = null;
            listBox.ItemsSource = stationsDispo;
            //il aurait été préférable d'utiliser un ObservableCollection et d'implémenter INotifyPropertyChanged
        }

    }
}