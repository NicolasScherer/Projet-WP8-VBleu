using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VéloBleu
{
    class Station_Item
    {
        string id, disp, tc, ac, ap, ab, name, wcom, lng, lat = "";
        string colorVelo, colorPlace = "";

        public string ColorPlace
        {
            get { return colorPlace; }
            set { colorPlace = value; }
        }

        public string ColorVelo
        {
            get { return colorVelo; }
            set { colorVelo = value; }
        }

        public string Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        public string Lng
        {
            get { return lng; }
            set { lng = value; }
        }

        public string Wcom
        {
            get { return wcom; }
            set { wcom = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Ab
        {
            get { return ab; }
            set { ab = value; }
        }

        public string Ap
        {
            get { return ap; }
            set { ap = value; }
        }

        public string Ac
        {
            get { return ac; }
            set { ac = value; }
        }

        public string Tc
        {
            get { return tc; }
            set { tc = value; }
        }

        public string Disp
        {
            get { return disp; }
            set { disp = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
