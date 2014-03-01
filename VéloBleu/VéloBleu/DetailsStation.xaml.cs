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
    public partial class DetailsStation : PhoneApplicationPage
    {
        Station_Item oneStationDetails = null;
        public DetailsStation()
        {
            InitializeComponent();
            oneStationDetails = (Station_Item)PhoneApplicationService.Current.State["oneStationDetails"];
            if (oneStationDetails != null)
            {
                stationNum.Text = oneStationDetails.Name;
                txtAdress.Text = oneStationDetails.Wcom;
            }
            
        }


    }
}