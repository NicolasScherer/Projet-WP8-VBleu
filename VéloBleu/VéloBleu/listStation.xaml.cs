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


        //public async void getLocation()
        //{
        //    geolocator = new Geolocator();
        //    geolocator.DesiredAccuracyInMeters = 25;
        //    try
        //    {
        //        Geoposition geoposition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10));
        //        GeoCoordinate pos = new GeoCoordinate(Convert.ToDouble(geoposition.Coordinate.Latitude), Convert.ToDouble(geoposition.Coordinate.Longitude));
        //        foreach (var query in stations)
        //        {
        //            GeoCoordinate posStation = new GeoCoordinate(Convert.ToDouble(query.Lat), Convert.ToDouble(query.Lng));
        //            double distanceInMeter;
        //            distanceInMeter = pos.GetDistanceTo(posStation);
        //            string distance = distanceInMeter.ToString();
        //            query.DistanceInMeter = distance;
        //        }
                
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        MessageBox.Show("Le service de location est désactivé dans les paramètres du téléphone.");
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.High, MovementThreshold = 20 };
           // geolocator.StatusChanged += geolocator_StatusChanged;
            geolocator.PositionChanged += geolocator_PositionChanged;
            listBox.SelectedIndex = -1;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            geolocator.PositionChanged -= geolocator_PositionChanged;
            //geolocator.StatusChanged -= geolocator_StatusChanged;
            geolocator = null;
            base.OnNavigatedFrom(e);
        }

        //private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        //{
        //    string status = "";
        //    switch (args.Status)
        //    {
        //        case PositionStatus.Ready:
        //            status = "Location is available.";
        //            break;

        //        case PositionStatus.Initializing:
        //            status = "Geolocation service is initializing.";
        //            break;

        //        case PositionStatus.NoData:
        //            status = "Location service data is not available.";
        //            break;

        //        case PositionStatus.Disabled:
        //            status = "Location services are disabled. Use the " +
        //                        "Settings charm to enable them.";
        //            break;

        //        case PositionStatus.NotInitialized:
        //            status = "Location status is not initialized because " +
        //                        "the app has not yet requested location data.";
        //            break;

        //        case PositionStatus.NotAvailable:
        //            status = "Location services are not supported on your system.";
        //            break;

        //        default:
        //            status = "Unknown PositionStatus value.";
        //            break;
        //    }
        //    //pour debug
        //   // Console.WriteLine("status GPS : "+ status);
        //}

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                GeoCoordinate pos = new GeoCoordinate(Convert.ToDouble(args.Position.Coordinate.Latitude), Convert.ToDouble(args.Position.Coordinate.Longitude));
                foreach (var query in stationsDispo)
                {
                    query.Lat = query.Lat.Replace('.', ',');
                    double lat = Convert.ToDouble(query.Lat);
                    query.Lng = query.Lng.Replace('.', ',');
                    double lng = Convert.ToDouble(query.Lng);

                    GeoCoordinate posStation = new GeoCoordinate(lat, lng);
                    double distanceInMeter;
                    distanceInMeter = pos.GetDistanceTo(posStation);
                    string distance = distanceInMeter.ToString("f0");
                    query.DistanceInMeter = distance;

                    //distance color
                    if (distanceInMeter <= 300)
                    {
                        query.ColorDistance = "Green";
                    }
                    else if (distanceInMeter > 300 && distanceInMeter <= 800)
                    {
                        query.ColorDistance = "Orange";
                    }
                    else if (distanceInMeter > 800)
                    {
                        query.ColorDistance = "Red";
                    }
                    //distance text km/m
                    if (distanceInMeter == 0)
                    {
                        query.DistanceInMeter = "gps failed";
                    }
                    else if (distanceInMeter > 1000)
                    {
                        double distanceInKilometers = (Math.Round(distanceInMeter));
                        distanceInKilometers = distanceInKilometers / 1000;
                        query.DistanceInMeter = distanceInKilometers.ToString("f1") + " km";
                    }
                    else
                    {
                        query.DistanceInMeter += " m";
                    }
                }
                listBox.ItemsSource = null;
                listBox.ItemsSource = stationsDispo;
            });
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


        //retour sur la page de chargement impossible (donc quitter app ou annuler)
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous vraiment quitter l'application 'Vélo Bleu' ?", "Fermeture de l'application", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Terminate();
            }
            
        }

        //station sélectionné
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //gere le bouton arrière lorqu'aucun item n'est réellement sélectionné
            if (listBox.SelectedIndex != -1)
            {
                //Region region = (Region)lbRegions.SelectedItem;
                Station_Item oneStationDetails = (Station_Item)listBox.SelectedItem;
                //dictionnaire d'état de l'application
                PhoneApplicationService.Current.State["oneStationDetails"] = oneStationDetails;
                //changement de page
                NavigationService.Navigate(new Uri("/DetailsStation.xaml", UriKind.Relative));
            }
            
        }

    }
}