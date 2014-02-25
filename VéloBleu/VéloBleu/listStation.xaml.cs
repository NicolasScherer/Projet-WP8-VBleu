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
            foreach(var query in stations){
            if(query.Disp == "1")
                stationsDispo.Add(query);
            }
            listBox.ItemsSource = stationsDispo;
        }
    }
}