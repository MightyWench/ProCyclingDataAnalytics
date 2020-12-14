using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cycling1._1
{

    //All the properties of the Team here. 
    //constructor only has three inputs as properties are input via algorithms later 
    [Serializable]
    public class Teamdetails 
    {
        private string teamname;

        public string Teamname
        {
            get { return teamname; }
            set { teamname = value; }
        }

        private int points;

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        private string status;
        
        public string Status
        {
            get { return status; }
            set { status = value; }

        }

        private int victories; 

        public int Victories
        {
            get { return victories; }
            set { victories = value; }
        }

        private int ranking;

        public int Ranking
        {
            get { return ranking; }
            set { ranking = value; }
        }

        private List<Riders> _RidersinTeam;
        
        public List<Riders> RidersinTeam
        {
            get { return _RidersinTeam; }
            set { _RidersinTeam = value; }
        }


           
         

        public Teamdetails(string name, int score, string teamstatus)
        {
            this.Teamname = name;
            this.Points = score;
            this.Status = teamstatus;
        }

        public Teamdetails()
        {

        }
    }
}
