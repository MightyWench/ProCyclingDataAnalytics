using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace Cycling1._1
{

    [Serializable]
    public class Races
    {
        private string racename; 
        
        public string Racename
        {
            get { return racename;  }
            set { racename = value;  }
        }

        private string racestatus;
        
        public string Racestatus
        {
            get { return racestatus; }
            set { racestatus = value; }
        }

        private int racedifficulty;

        public int Racedifficulty
        {
            get { return racedifficulty; }
            set { racedifficulty = value; }
        }

        private string racetype;

        public string Racetype
        {
            get { return racetype; }
            set { racetype = value; }
        }

        private int cobbledifficutly;
        public int Cobbledifficulty
        {
            get { return cobbledifficutly; }
            set { cobbledifficutly = value; }
        }

        private float climbdifficultynormalized; 

        public float Climbdifficultynormalized
        {
            get { return climbdifficultynormalized; }
            set { climbdifficultynormalized = value; }
        }

        private float lengthdifficulty; 
        
        public float Lengthdifficulty
        {
            get { return lengthdifficulty; }
            set { lengthdifficulty = value; }
        }


        private int yearedition; 
        public int Yearedition
        {
            get { return yearedition; }
            set { yearedition = value; }
        }

        private List<raceresults> results;

        public List<raceresults> Results
        {
            get { return results; }
            set { results = value; }
        }

        private date dates; 

        public date Dates
        {
            get { return dates; }
            set { dates = value; }
        }

        private float racelength;


        public float Racelength
        {
            get { return racelength; }
            set { racelength = value; }
        }

        public Races(string nRacename, string nRacestatus, string nRacetype, int nYearedition, List<raceresults> nResults, date nDate, float nRacelength, int nRacedifficulty, int nCobbledifficulty)
        {
            this.Racename = nRacename;
            this.Racestatus = nRacestatus;
            this.Racetype = nRacetype;
            this.Yearedition = nYearedition;
            this.Results = nResults;
            this.Dates = nDate;
            this.Racelength = nRacelength;
            this.Racedifficulty = nRacedifficulty;
            this.Cobbledifficulty = nCobbledifficulty;
        }

        public Races()
        {
            

        }

       

      
    }

    [Serializable]

    public class stages: Races
    {
        private List<TTTformat> tttresults;

        public List<TTTformat> TTTResults
        {
            get { return tttresults; }
            set { tttresults = value; }
        }

        public stages(string nRacename, string nRacestatus, string nRacetype, int nYearedition, List<raceresults> nResults, date nDate, float nRacelength, int nRacedifficulty, int nCobbledifficulty)
        {
            this.Racename = nRacename;
            this.Racestatus = nRacestatus;
            this.Racetype = nRacetype;
            this.Yearedition = nYearedition;
            this.Results = nResults;
            this.Dates = nDate;
            this.Racelength = nRacelength;
            this.Racedifficulty = nRacedifficulty;
            this.Cobbledifficulty = nCobbledifficulty;
        }

        public stages(string nRacename, string nRaceStatus, string nRacetype, int nYearEdition, List<TTTformat> nTTTresults, date nDate)
        {
            this.Racename = nRacename;
            this.Racestatus = nRaceStatus;
            this.Racetype = nRacetype;
            this.Yearedition = nYearEdition;
            this.TTTResults = nTTTresults;
            this.Dates = nDate;

        }

        public stages()
        {

        }


    }

    [Serializable]

    public class TTTriders
    {
        private string ridername; 

        public string Ridername
        {
            get { return ridername; }
            set { ridername = value; }
        }

        public float timelost;

        private float Timelost
        {
            get { return timelost; }
            set { timelost = value; }
        }

        public TTTriders()
        {

        }
            

    }

    [Serializable]
    public class TTTformat
    {
        private string teamname; 

        public string Teamname
        {
            get { return teamname; }
            set { teamname = value; }
        }

        private string time; 

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        private string gaps;

        private string Gaps
        {
            get { return gaps; }
            set { gaps = value; }
        }

        private float speed; 

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private Teamresult teamRidersinTT;

        public Teamresult TeamRidersinTT
        {
            get { return teamRidersinTT; }
            set { teamRidersinTT = value; }
        }

        public TTTformat(string nTeamname, string nTime, string nGaps, float nSpeed, Teamresult nTeamRidersinTT)
        {
            this.Teamname = nTeamname;
            this.Time = nTime;
            this.Gaps = nGaps;
            this.Speed = nSpeed;
            this.TeamRidersinTT = nTeamRidersinTT;
        }

        public TTTformat()
        {

        }
    }

    public class Teamresult
    {
        private List<TTTriders> ridersinTT;

        public List<TTTriders> RidersinTT
        {
            get { return ridersinTT; }
            set { ridersinTT = value; }
        }

        public Teamresult()
        {

        }

        
    }
        


    [Serializable]
    public class raceresults
    {
        private string position;

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private string riderinrace;

        public string Riderinrace
        {
            get { return riderinrace; }
            set { riderinrace = value; }
        }

        private string time;

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        private string team;

        public string Team
        {
            get { return team; }
            set { team = value; }
        }

        public raceresults(string nPosition, string nRiderinrace, string nTime, string nTeam)
        {
            this.Position = nPosition;
            this.Riderinrace = nRiderinrace;
            this.Time = nTime;
            this.Team = nTeam;
        }

        public raceresults()
        {

        }




    }

    [Serializable]
    public class date
    {
        private int day;

        public int Day
        {
            get { return day; }
            set { day = value; }
        }

        private int month; 

        public int Month
        {
            get { return month; }
            set { month = value; }
        }

        private int year; 

        public int Year
        {
            get { return year;  }
            set { year = value; }
        }

        public date()
        {

        }
    }

    [Serializable]

    public class Generalclassification
    {


        private List<raceresults> gcResults;

        public List<raceresults> GcResults
        {
            get { return gcResults; }
            set { gcResults = value; }

        }

        public Generalclassification(bool nisfinalGC, List<raceresults> ngcResults)
        {

            this.GcResults = ngcResults;
        }

        private List<Riders> _GCRidersinRace;

        public List<Riders> GCRidersinRace
        {
            get { return _GCRidersinRace; }
            set { _GCRidersinRace = value; }
        }
        



        public Generalclassification()
        {

        }
            

            


    }

    [Serializable]
    public class Stageraces
    {

        public string Racename => Stageresults[0].Racename + " " + Stageresults[0].Yearedition;

        private Generalclassification gcclassification; 

        public Generalclassification Gcclassification
        {
            get { return gcclassification;}
            set { gcclassification = value; }
        }

        private List<stages> stageresults;

        public List<stages> Stageresults
        {
            get { return stageresults; }
            set { stageresults = value; }
        }

        public Stageraces(List<stages> nStageResults, Generalclassification nGC)
        {
            this.Gcclassification = nGC;
            this.Stageresults = nStageResults;
        }

        private List<GCGaps> _GeneralClassificationBreakdown;

        public List<GCGaps> GeneralClassificationBreakdown
        {
            get { return _GeneralClassificationBreakdown; }
            set { _GeneralClassificationBreakdown = value; }
        }

        public class GCGaps
        {
            private string _NameofRider;

            public string NameofRider
            {
                get { return _NameofRider; }
                set { _NameofRider = value; }
            }

            private List<int> _ListOfGCGapsPerStage;

            public List<int> ListOfGCGapsPerStage
            {
                get { return _ListOfGCGapsPerStage; }
                set { _ListOfGCGapsPerStage = value; }
            }

            public GCGaps()
            {

            }
        }
        
        public Stageraces()
        {

        }

        public void timecorrector(List<Stageraces> iStageRaces)
        {
            foreach (var race in iStageRaces)
            { 
               for(int x = 0; x < race.Stageresults.Count; x++)
                {
                    string PlaceHolderTime;
                    if (race.Stageresults[x].TTTResults.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        PlaceHolderTime = "0:00";
                    }

                    for (int y = 0; y < race.Stageresults[x].Results.Count; y++)
                    {
                        if(race.Stageresults[x].Results[y].Position.Contains("OTL") || race.Stageresults[x].Results[y].Position.Contains("DNF"))
                        {
                            race.Stageresults[x].Results[y].Time = "120:00";
                        }
                        else
                        {
                            if (y == 0)
                            {
                                race.Stageresults[x].Results[y].Time = "0:00";
                            }
                            else
                            {
                               if (race.Stageresults[x].Results[y].Time.Contains(",,") || race.Stageresults[x].Results[y].Time.Contains(" "))
                                {
                                    race.Stageresults[x].Results[y].Time = PlaceHolderTime;
                                }
                               else
                                {
                                    PlaceHolderTime = race.Stageresults[x].Results[y].Time;   
                                }
                            }
                        }
                    }
                }
            }
        }
        public int TimeCorrectorFormat(string iTime)
        {
            string SplitTimeMinutes;
            string SplitTimeSeconds;
            try
            {
                SplitTimeMinutes = iTime.Split(new string[] { ":" }, 2, StringSplitOptions.None)[0];
            }
            catch (Exception parsingError)
            {
                SplitTimeMinutes = "0";
            }

            try
            {
                SplitTimeSeconds = iTime.Split(new string[] { ":" }, 2, StringSplitOptions.None)[1];
            }
            catch (Exception SecondsParsingError)
            {
                SplitTimeSeconds = "0";
            }


            int Minutes = int.Parse(SplitTimeMinutes);
            Minutes = Minutes * 60;
            int Seconds = int.Parse(SplitTimeSeconds);
            int TotalTime = Minutes + Seconds;
            return TotalTime;
        }

        public void GCRiderCounter(List<Stageraces> StageRacesList)
        {
            int AverageStageDifficulty;
            foreach (var stagerace in StageRacesList)
            {
                List<stages> ListofGCStages = new List<stages>();
                List<int> CoefficientsOfGCWinner = new List<int>();
                List<string> ProvisionalListofGCRiders = new List<String>();
                List<List<int>> ListOfTimeGaps = new List<List<int>>();

                if (stagerace.Gcclassification.GcResults.Count > 0)
                {
                    for (int x = 0; x < 15; x++)
                    {
                        ProvisionalListofGCRiders.Add(stagerace.Gcclassification.GcResults[x].Riderinrace.Trim().ToLower());
                    }
                }
                else
                {
                    continue;
                }

                int SigmaRaceDifficulty = 0;
                for (int StageSpecifier = 0; StageSpecifier < stagerace.Stageresults.Count; StageSpecifier++)
                {
                    SigmaRaceDifficulty = SigmaRaceDifficulty + stagerace.stageresults[StageSpecifier].Racedifficulty;
                }
                AverageStageDifficulty = SigmaRaceDifficulty / stagerace.Stageresults.Count;

                for (int StageSpecifierSecond = 0; StageSpecifierSecond < stagerace.Stageresults.Count; StageSpecifierSecond++)
                {
                    if(stagerace.Stageresults[StageSpecifierSecond].Racedifficulty >= AverageStageDifficulty)
                    {
                        ListofGCStages.Add(stagerace.Stageresults[StageSpecifierSecond]);
                    }
                }

                foreach (var stage in ListofGCStages)
                {
                    for (int GCWinnerIterator = 0; GCWinnerIterator < stage.Results.Count; GCWinnerIterator++)
                    {
                        if (stage.Results[GCWinnerIterator].Riderinrace.Trim().ToLower() == ProvisionalListofGCRiders[0])
                        {
                            int defaultbonus = 0;
                            switch (GCWinnerIterator)
                            {
                                case var _ when GCWinnerIterator == 0:
                                    defaultbonus = -10;
                                    break;

                                case var _ when GCWinnerIterator == 1:
                                    defaultbonus = -5;
                                    break;

                                case var _ when GCWinnerIterator == 2:
                                    defaultbonus = -3;
                                    break;

                                default:
                                    break;
                            }

                            
                            CoefficientsOfGCWinner.Add(TimeCorrectorFormat(stage.Results[GCWinnerIterator].Time) + defaultbonus);
                              
                            
                        }
                    }
                }

                foreach (string GCRider in ProvisionalListofGCRiders)
                {
                    GCGaps GCGapsThisRace = new GCGaps();
                    List<int> TimeGapsThisRace = new List<int>();
                    List<int> TimeGapsToGCWinner = new List<int>();
                    foreach (var stage in ListofGCStages)
                    {
                        
                        for (int NormalGCRiderIterator = 0; NormalGCRiderIterator < stage.Results.Count; NormalGCRiderIterator++)
                        {
                            if(GCRider == stage.Results[NormalGCRiderIterator].Riderinrace.Trim().ToLower())
                            {
                                int defaultbonus = 0;
                                switch (NormalGCRiderIterator)
                                {
                                    case var _ when NormalGCRiderIterator == 0:
                                        defaultbonus = -10;
                                        break;

                                    case var _ when NormalGCRiderIterator == 1:
                                        defaultbonus = -5;
                                        break;

                                    case var _ when NormalGCRiderIterator == 2:
                                        defaultbonus = -3;
                                        break;

                                    default:
                                        break;
                                }
                                TimeGapsThisRace.Add(TimeCorrectorFormat(stage.Results[NormalGCRiderIterator].Time)+ defaultbonus);
                            }

                        }
                    }
                    for (int Differences = 0; Differences < TimeGapsThisRace.Count; Differences++)
                    {
                        TimeGapsToGCWinner.Add(TimeGapsThisRace[Differences] - CoefficientsOfGCWinner[Differences]);
                    }

                    ListOfTimeGaps.Add(TimeGapsToGCWinner);

                }

                for (int GCGapsAdder = 0; GCGapsAdder < ProvisionalListofGCRiders.Count; GCGapsAdder++)
                {
                    GCGaps GapsForGCRider = new GCGaps();
                    GapsForGCRider.ListOfGCGapsPerStage = ListOfTimeGaps[GCGapsAdder];
                    GapsForGCRider.NameofRider = ProvisionalListofGCRiders[GCGapsAdder];
                    stagerace.GeneralClassificationBreakdown.Add(GapsForGCRider);
                }
                Console.ReadLine();
            }




           
        }
        

        
    }

    
    
}
