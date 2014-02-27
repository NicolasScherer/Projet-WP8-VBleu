using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace VéloBleu
{
    public partial class listStation : PhoneApplicationPage
    {
        List<Station_Item> stations;
        List<Station_Item> stationsDispo;
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
                        query.Ap += " places";
                    }
                    else if (placesDispo > 0 && placesDispo <= 3)
                    {
                        query.ColorPlace = "Orange";
                        query.Ap += " places";
                    }
                    else if (placesDispo == 0)
                    {
                        query.ColorPlace = "Red";
                        query.Ap += " place";
                    }
                    //code couleur vélos
                    if (velosDispo > 3)
                    {
                        query.ColorVelo = "Green";
                       // query.Ab += " vélos";
                    }
                    else if (velosDispo > 0 && velosDispo <= 3)
                    {
                        query.ColorVelo = "Orange";
                       // query.Ab += " vélos";
                    }
                    else if (velosDispo == 0)
                    {
                        query.ColorVelo = "Red";
                       // query.Ab += " vélo";
                    }
                    stationsDispo.Add(query);
                }
                listBox.ItemsSource = stationsDispo;
            }
        }

        private void triVelo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // tri de la liste en fonction des places
            stationsDispo.Sort(new StationComparator(StationComparator.CompareType.VELO));
            // maj listebox
            listBox.ItemsSource = stationsDispo;
        }

        private void triPlace_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO : ajoutez ici l'implémentation du gestionnaire d'événements.
        }

        private void triDistance_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO : ajoutez ici l'implémentation du gestionnaire d'événements.
        }
    }
}