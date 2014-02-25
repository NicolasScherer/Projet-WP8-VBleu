using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VéloBleu.Resources;
using System.Xml.Linq;
using System.Diagnostics;

namespace VéloBleu
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructeur
        List<Station_Item> stations;
        public MainPage()
        {
            InitializeComponent();

            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();

            ProgBar.Opacity = 100;
            txtProgBar.Text = "récupération des données...";
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri("http://www.velo-vision.com/nice/oybike/stands.nsf/getsite?site=nice&format=xml&key=veolia"));

        }
        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null){
                string responseXml = e.Result;
                txtProgBar.Text = "traitement des données...";
                try{
                    parseVelo(responseXml);
                }catch (Exception exception){
                    ProgBar.Opacity = 0;
                    txtProgBar.Text = "Erreur de lecture des données. Précision sur l'exception : "+ exception.HelpLink;
                    //throw;
                }
                txtProgBar.Text = "affichage des données...";
                //dictionnaire d'état de l'application
                PhoneApplicationService.Current.State["stations"] = stations;
                //changement de page
                NavigationService.Navigate(new Uri("/listStation.xaml", UriKind.Relative));
            }else{
                ProgBar.Opacity = 0;
                txtProgBar.Text = "Impossible de récupérer les données sur internet.";
            }
        }

        private void parseVelo(string responseXml)
        {
            XDocument doc = XDocument.Parse(responseXml);
            stations = (from query in doc.Descendants("stand")
                        select new Station_Item
                        {
                            Name = HttpUtility.UrlDecode( (string)query.Attribute("name") ),
                            Id = (string)query.Attribute("id"),
                            Wcom = HttpUtility.UrlDecode( (string)query.Element("wcom") ),
                            Disp = (string)query.Element("disp"),
                            Lng = (string)query.Element("lng"),
                            Lat = (string)query.Element("lat"),
                            Tc = (string)query.Element("tc"),
                            Ac = (string)query.Element("ac"),
                            Ap = (string)query.Element("ap"),
                            Ab = (string)query.Element("ab")
                        }).ToList();
        }


        // Exemple de code pour la conception d'une ApplicationBar localisée
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}