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
                int a = Convert.ToInt32(station2.Ab);
                int b = Convert.ToInt32(station1.Ab);
                if (a != b)
                    return (a < b ? -1 : 1);
            }
            //tri place
            if (this.type == CompareType.PLACE)
            {
                int a = Convert.ToInt32(station2.Ap);
                int b = Convert.ToInt32(station1.Ap);
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
