using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Services;

namespace VéloBleu
{
    public partial class DetailsStation : PhoneApplicationPage
    {
        Station_Item oneStationDetails = null;
        private List<GeoCoordinate> coordonnesTrajet;
        private GeoCoordinate positionUser = null;
        private GeoCoordinate posStation = null;
        RouteQuery MyQuery = null;
        GeocodeQuery Mygeocodequery = null;

        public DetailsStation()
        {
            InitializeComponent();
            oneStationDetails = (Station_Item)PhoneApplicationService.Current.State["oneStationDetails"];
            positionUser = (GeoCoordinate)PhoneApplicationService.Current.State["positionUser"];
            
            if (oneStationDetails != null)
            {
                stationNum.Text = oneStationDetails.Name;
                txtAdress.Text = oneStationDetails.Wcom;
            }

            //carte en mode "route" par défaut
            Map.CartographicMode = MapCartographicMode.Road;

            //zoom sur la station
            oneStationDetails.Lat = oneStationDetails.Lat.Replace('.', ',');
            double lat = Convert.ToDouble(oneStationDetails.Lat);
            oneStationDetails.Lng = oneStationDetails.Lng.Replace('.', ',');
            double lng = Convert.ToDouble(oneStationDetails.Lng);

            posStation = new GeoCoordinate(lat, lng);
            Map.ZoomLevel = 17;
            Map.Center = posStation;

            //point d'intérêt
            var elts = MapExtensions.GetChildren(Map);
            Pushpin pushpin = new Pushpin();
            pushpin.Content = oneStationDetails.Name;
            MapExtensions.Add(elts, pushpin, posStation);
  
            //ajout des informations pour les piétons (escaliers)
            Map.PedestrianFeaturesEnabled = true;

            //itinéraire
            coordonnesTrajet = new List<GeoCoordinate>();
            
        }
        //Zoom
        private void Zoom_Click(object sender, EventArgs e)
        {
            Map.ZoomLevel++;
        }
        //Dé-Zoom
        private void Dezoom_Click(object sender, EventArgs e)
        {
            Map.ZoomLevel--;
        }

        //type d'affichage de carte
        private void rBtPlan_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Map.CartographicMode = MapCartographicMode.Road;
        }

        private void rBtSat_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Map.CartographicMode = MapCartographicMode.Hybrid;
        }

        private void rBtRel_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Map.CartographicMode = MapCartographicMode.Terrain;
        }

        void Mygeocodequery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                MyQuery = new RouteQuery();
                try
                {
                    coordonnesTrajet.Add(e.Result[0].GeoCoordinate);
                    MyQuery.Waypoints = coordonnesTrajet;
                    MyQuery.QueryCompleted += MyQuery_QueryCompleted;
                    MyQuery.QueryAsync();
                    Mygeocodequery.Dispose();
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Impossible de calculer l'itinéraire, veuillez rééssayer plus tard.", "Erreur", MessageBoxButton.OKCancel);

                }
            }
        }
        void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                Map.AddRoute(MyMapRoute);
                MyQuery.Dispose();
            }
        }
       
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Mygeocodequery.QueryCompleted -= Mygeocodequery_QueryCompleted;
            MyQuery.QueryCompleted -= MyQuery_QueryCompleted;
            coordonnesTrajet.Clear();
            base.OnNavigatedFrom(e);
        }

        private void btRoute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            coordonnesTrajet.Add(positionUser);
            Mygeocodequery = new GeocodeQuery();
            Mygeocodequery.SearchTerm = oneStationDetails.Wcom + " , Alpes-Maritimes";
            Mygeocodequery.GeoCoordinate = posStation;

            Mygeocodequery.QueryCompleted += Mygeocodequery_QueryCompleted;
            Mygeocodequery.QueryAsync();
        }
    }
}