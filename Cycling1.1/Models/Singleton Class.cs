using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cycling1._1
{
    class Singleton_Class
    {
        public static List<Teamdetails> InstancesofTeams = new List<Teamdetails>();
        public static List<Riders> ListofRiders = new List<Riders>();
        public static List<Races> ListofWTOneDayRaces = new List<Races>();
        public static List<Stageraces> ListofStageRaces = new List<Stageraces>();
        public static List<Races> ListofPCTOneDayRaces = new List<Races>();
        public static List<Stageraces> ListofPCTStageRaces = new List<Stageraces>();
    }
}
