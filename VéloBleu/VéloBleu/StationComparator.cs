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
                char[] delimiterChars = { ' ' };

                string distance1All = station1.DistanceInMeter;
                string distance2All = station2.DistanceInMeter;
                string[] words1 = distance1All.Split(delimiterChars);
                string[] words2 = distance2All.Split(delimiterChars);
                string distance1 = words1[0];
                string distance2 = words2[0];
                double a = Convert.ToDouble(distance1);
                double b = Convert.ToDouble(distance2);
                //test si km (si c'est le cas multiplié par mille)
                if (words1[1] == "km")
                {
                    a = a * 1000;
                }
                if (words2[1] == "km")
                {
                    b = b * 1000;
                }
                

                if (a != b)
                    return (a < b ? -1 : 1);
            }
            return 0;
        }
    }
}
