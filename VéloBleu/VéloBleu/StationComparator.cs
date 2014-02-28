using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VéloBleu
{
    class StationComparator : Comparer<Station_Item>
    {
         //enum permettant de gérer le type de tri
        public enum CompareType { VELO, PLACE, DISTANCE };
	    private CompareType type;

        // constructeur
        public StationComparator(CompareType t)
        {
            this.type = t;
        }

        public override int Compare(Station_Item station1, Station_Item station2)
        {
            //tri vélo
            if (this.type == CompareType.VELO)
            {
                string numA = station2.Ab.Substring(0, 2);
                int a = Convert.ToInt32(numA);
                string numB = station1.Ab.Substring(0, 2);
                int b = Convert.ToInt32(numB);
                if (a != b)
                    return (a < b ? -1 : 1);
            }
            //tri place
            if (this.type == CompareType.PLACE)
            {
                string numA = station2.Ap.Substring(0, 2);
                int a = Convert.ToInt32(numA);
                string numB = station1.Ap.Substring(0, 2);
                int b = Convert.ToInt32(numB);
                if (a != b)
                    return (a < b ? -1 : 1);

            }
            //tri distance
            if (this.type == CompareType.DISTANCE)
            {
               
                //float a = station1.distanceInMeters;
                //float b = station2.distanceInMeters;
                //Log.v("valeur a", "station1.distance = " + a);
                //Log.v("valeur b", "station2.distance = " + b);
                //if (a != b)
                //    return (a < b ? -1 : 1);
            }
            return 0;
        }
    }
}
