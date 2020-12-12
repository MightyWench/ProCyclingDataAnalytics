using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cycling1._1
{
    class DictionaryResultsMatcher
    {
        public static Dictionary<string, raceresults> StageRaceDictionary = new Dictionary<string, raceresults>();

        //This matches the race results to a property of the riders called RaceResult. This saves time in the future and stops having to do a search algorithm everytime you want to search for a result. 
        public static void resultsmatcher()
        {
            foreach (var stages in Singleton_Class.ListofStageRaces)
            {
                for (int x = 0; x < stages.Stageresults.Count; x++)
                {
                    for (int y = 0; y < stages.Stageresults[x].Results.Count; y++)
                    {
                        if(stages.Stageresults[x].Results[y].Position.Equals(""))
                        {
                            stages.Stageresults[x].Results[y].Position = "DNS";
                        }
                    }
                }
            }
            foreach (var stages in Singleton_Class.ListofPCTStageRaces)
            {
                for (int x = 0; x < stages.Stageresults.Count; x++)
                {
                    for (int y = 0; y < stages.Stageresults[x].Results.Count; y++)
                    {
                        if (stages.Stageresults[x].Results[y].Position.Equals(""))
                        {
                            stages.Stageresults[x].Results[y].Position = "DNS";
                        }
                    }
                }
            }

            foreach (var stages in Singleton_Class.ListofStageRaces)
            {
                stages.timecorrector(Singleton_Class.ListofStageRaces);
                stages.timecorrector(Singleton_Class.ListofPCTStageRaces);
                break; 
            }

           foreach (var stages in Singleton_Class.ListofStageRaces)
            {
                stages.GCRiderCounter(Singleton_Class.ListofStageRaces);
                break;
            }

           foreach (var stages in Singleton_Class.ListofStageRaces)
            {
                for(int x = 0; x < stages.Stageresults.Count; x++)
                {
                    if (stages.Stageresults[x].Racename.Contains("France") && stages.Stageresults[x].Yearedition == 2019 && x ==18)
                    {
                        for(int y = 0; y < stages.Stageresults[x].Results.Count; y++)
                        {
                            stages.Stageresults[x].Results[y].Position = (y + 1).ToString();
                        }
                    }
                }
            }

           


           foreach(var rider in Singleton_Class.ListofRiders)
           {
                List<RiderResults> ResultsofRider = new List<RiderResults>();
                List<RiderResults> RiderStageRaceResults = new List<RiderResults>();
                List<RiderGCResults> RiderGCResultsList = new List<RiderGCResults>();
                List<RiderResults> PCTOneDayResults = new List<RiderResults>();
                List<RiderResults> PCTStageRaceStageResults = new List<RiderResults>();
                List<RiderGCResults> PCTStageRaceGCResults = new List<RiderGCResults>();
                string name = rider.Fullname.ToLower().Trim();
                

                foreach(var race in Singleton_Class.ListofWTOneDayRaces)
                {
                    
                   
                    for (int x = 0; x < race.Results.Count; x++)
                    {

                        string ridermatcher = race.Results[x].Riderinrace.ToLower().Trim();
                        if(ridermatcher.Equals(name))
                        {
                            string position = race.Results[x].Position;
                            string team = race.Results[x].Team;
                            date date = race.Dates;
                            string racename = race.Racename;
                            float racelength = race.Racelength;
                            int difficulty = race.Racedifficulty;
                            string type = race.Racetype;
                            int cobbled = race.Cobbledifficulty;
                            float normalizedlength = race.Lengthdifficulty;
                            float normalizedclimb = race.Climbdifficultynormalized;

                            RiderResults searchedresults = new RiderResults(team, racename, position, date, racelength, difficulty, type, cobbled,normalizedclimb,normalizedlength);
                            ResultsofRider.Add(searchedresults);
                            
                        }
                    }
                }

                

                rider.Riderresult = ResultsofRider;

                foreach (var stagerace in Singleton_Class.ListofStageRaces)
                {
                    for (int y =0; y < stagerace.Stageresults.Count; y++)
                    {
                        for (int z =0; z < stagerace.Stageresults[y].Results.Count; z++)
                        {
                            string ridermatch = stagerace.Stageresults[y].Results[z].Riderinrace.ToLower().Trim();
                            if(ridermatch.Equals(name))
                            {
                                string position = stagerace.Stageresults[y].Results[z].Position;
                                string team = stagerace.Stageresults[y].Results[z].Team;
                                date date = stagerace.Stageresults[y].Dates;
                                string racename = stagerace.Stageresults[y].Racename;
                                float racelength = stagerace.Stageresults[y].Racelength;
                                int difficulty = stagerace.Stageresults[y].Racedifficulty;
                                string type = stagerace.Stageresults[y].Racetype;
                                int cobbled = stagerace.Stageresults[y].Cobbledifficulty;
                                float normalizedlength = stagerace.Stageresults[y].Lengthdifficulty;
                                float normalizedclimb = stagerace.Stageresults[y].Climbdifficultynormalized;

                                RiderResults stageresults = new RiderResults(team, racename, position, date, racelength, difficulty, type, cobbled, normalizedclimb, normalizedlength);
                                RiderStageRaceResults.Add(stageresults);
                                
                            }
                        }
                    }
                }



                rider.RiderStageRaceResult = RiderStageRaceResults;

                foreach (var stagerace in Singleton_Class.ListofStageRaces)
                {
                    for (int x = 0; x < stagerace.Gcclassification.GcResults.Count; x++)

                    {
                        string riderGCMatch = stagerace.Gcclassification.GcResults[x].Riderinrace.ToLower().Trim();
                        if(riderGCMatch.Equals(name))
                        {
                            string position = stagerace.Gcclassification.GcResults[x].Position;
                            string team = stagerace.Gcclassification.GcResults[x].Team;
                            string nameofGCrace = stagerace.Stageresults[0].Racename + " " + stagerace.Stageresults[0].Yearedition;
                            string formattedGCname = nameofGCrace.Replace("stage-1", "General-Classification");
                            date DateofGC = stagerace.Stageresults[0].Dates;
                            int numberofstages = stagerace.Stageresults.Count;

                            RiderGCResults GCresults = new RiderGCResults(formattedGCname, team, position, DateofGC, numberofstages);
                            RiderGCResultsList.Add(GCresults);
         
                        }
                    }
                }

                

                foreach (var PCTOneDayRace in Singleton_Class.ListofPCTOneDayRaces)
                {
                    for(int x =0; x < PCTOneDayRace.Results.Count; x++)
                    {
                        string ridermatcher = PCTOneDayRace.Results[x].Riderinrace.ToLower().Trim();
                        if (ridermatcher.Equals(name))
                        {
                            string position = PCTOneDayRace.Results[x].Position;
                            string team = PCTOneDayRace.Results[x].Position;
                            date date = PCTOneDayRace.Dates;
                            string racename = PCTOneDayRace.Racename;
                            float racelength = PCTOneDayRace.Racelength;
                            int difficulty = PCTOneDayRace.Racedifficulty;
                            string type = PCTOneDayRace.Racetype;
                            int cobbled = PCTOneDayRace.Cobbledifficulty;
                            float normalizedlength = PCTOneDayRace.Lengthdifficulty;
                            float normalizedclimb = PCTOneDayRace.Climbdifficultynormalized;

                            RiderResults searchedresults = new RiderResults(team, racename, position, date, racelength, difficulty, type, cobbled, normalizedclimb, normalizedlength);
                            PCTOneDayResults.Add(searchedresults);
                        }
                    }
                }

                rider.PCTOneDayRaceResults = PCTOneDayResults;

                foreach (var PCTStage in Singleton_Class.ListofPCTStageRaces)
                {
                    for(int x=0; x < PCTStage.Stageresults.Count; x++)
                    {
                        for (int y = 0; y < PCTStage.Stageresults[x].Results.Count; y++)
                        {
                            string ridermatcher = PCTStage.Stageresults[x].Results[y].Riderinrace.ToLower().Trim();
                            if (ridermatcher.Equals(name))
                            {
                                string position = PCTStage.Stageresults[x].Results[y].Position;
                                string team = PCTStage.Stageresults[x].Results[y].Team;
                                date date = PCTStage.Stageresults[x].Dates;
                                string racename = PCTStage.Stageresults[x].Racename;
                                float racelength = PCTStage.Stageresults[x].Racelength;
                                int difficulty = PCTStage.Stageresults[x].Racedifficulty;
                                string type = PCTStage.Stageresults[x].Racetype;
                                int cobbled = PCTStage.Stageresults[x].Cobbledifficulty;
                                float normalizedlength = PCTStage.Stageresults[x].Lengthdifficulty;
                                float normalizedclimb = PCTStage.Stageresults[x].Climbdifficultynormalized;

                                RiderResults StageResults = new RiderResults(team, racename, position, date, racelength, difficulty, type, cobbled,
                                    normalizedclimb, normalizedclimb);
                                PCTStageRaceStageResults.Add(StageResults);
                            }
                        }
                    }
                }

                rider.PCTStageRaceStageResults = PCTStageRaceStageResults;

                foreach (var PCTGC in Singleton_Class.ListofPCTStageRaces)
                {
                    for(int x=0; x < PCTGC.Gcclassification.GcResults.Count; x++)
                    {
                        string riderGCmatch = PCTGC.Gcclassification.GcResults[x].Riderinrace.ToLower().Trim();
                        if (riderGCmatch.Equals(name))
                        {

                            string position = PCTGC.Gcclassification.GcResults[x].Position;
                            string team = PCTGC.Gcclassification.GcResults[x].Team;

                            string formattedGCname;
                            date DateofGC = new date();
                            try
                            {
                                string nameofGCRace = PCTGC.Stageresults[0].Racename + " " + PCTGC.Stageresults[0].Yearedition;
                                formattedGCname = nameofGCRace.Replace("stage-1", "General-Classification");
                            }
                            catch(Exception NameError)
                            {
                               formattedGCname = "Name Error";
                            }

                            try
                            {
                                DateofGC = PCTGC.Stageresults[0].Dates;
                            }
                            catch (Exception Datesexception)
                            {
                                DateofGC.Day = 1;
                                DateofGC.Month = 1;
                                DateofGC.Year = 2011;
                            }
                            int numberofstage = PCTGC.Stageresults.Count;

                            RiderGCResults ResultsOfPCTGC = new RiderGCResults(formattedGCname, team, position, DateofGC, numberofstage);
                            RiderGCResultsList.Add(ResultsOfPCTGC);
                        }
                           
                    }
                }

                rider.RiderGC = RiderGCResultsList;
           }
           foreach (var rider in Singleton_Class.ListofRiders)
            {
                rider.StandardDeviationOfMountainStages(Singleton_Class.ListofRiders);
                break;
            }
           TeamMatcher();

           
            
        }

        public static void TeamMatcher()
        {
            foreach (var team in Singleton_Class.InstancesofTeams)
            {
                string teamname = team.Teamname;
                string hyphenremover = teamname.Replace("-", "");
                string whitespaceremover = hyphenremover.Replace(" ", "");
                
                foreach(var rider in Singleton_Class.ListofRiders)
                {
                    string RiderTeam = rider.Team;
                    string RiderTeamHyphen = RiderTeam.Replace("-", "");
                    string RiderTeamWhitespace = RiderTeamHyphen.Replace(" ", "");
                    if (RiderTeamWhitespace == whitespaceremover) 
                    {
                        team.RidersinTeam.Add(rider);
                    }
                }
                
            }
            
            variablescalculator.Calculations();
        }

        
            
        
    }

    public class variablescalculator
    {
        public static void Calculations()
        {
            foreach(var rider in Singleton_Class.ListofRiders)
            {
                List<RiderResults> StoreofAllresults = new List<RiderResults>();
                StoreofAllresults.AddRange(rider.PCTOneDayRaceResults);
                StoreofAllresults.AddRange(rider.PCTStageRaceStageResults);
                StoreofAllresults.AddRange(rider.RiderStageRaceResult);
                StoreofAllresults.AddRange(rider.Riderresult);

                rider.ClimbingPearsonsrankcoefficient = regressionmodelclimbing(StoreofAllresults);
                rider.RaceLengthPearsonsCoefficient(rider);
                
            }

            Riders riders = new Riders();
            riders.CobbledConsistency(Singleton_Class.ListofRiders);

            foreach (var rider in Singleton_Class.ListofRiders)
            {
                rider.FurtherMetrics.ClimbingRating = rider.ClimbingRating(rider);
            }
            Console.ReadLine();
        }

        public static float regressionmodelclimbing(List<RiderResults> Inputresults)
        {
            
            //x variable is the average position. 
            float standarddeviationX;
            //y variable is the average climbing difficulty. 
            float standarddeviationY;

            //number of inputs "N" for X variables
            float xN = Inputresults.Count;

            //Declaration of the mean; 
            float xMean = 0;

            float worstresult = 0;
            foreach (RiderResults result in Inputresults)
            {
                if (result.Position.Equals("DNF") || result.Position.Equals("OTL") || result.Position.Equals("")|| result.Position.Equals("DNS"))
                {
                    continue;
                }

                float floatresult = float.Parse(result.Position);
                
                if (floatresult > worstresult)
                {
                    worstresult = floatresult;
                }
            }

            int counter = 0;
            float[] positions = new float[Inputresults.Count];
            foreach (RiderResults result in Inputresults)
            {
                if (result.Position.Equals("DNF") || result.Position.Equals("OTL") || result.Position.Equals("") || result.Position.Equals("DNS"))
                {
                    float xValue = worstresult;
                    xMean = xMean + xValue;
                    positions[counter] = xValue;
                    counter = counter + 1; 
                    
                }
                else
                {
                    float xValue = float.Parse(result.Position);
                    xMean = xMean + xValue;
                    positions[counter] = xValue;
                    counter = counter + 1;

                }
            }

            xMean = xMean / xN;

            float SigmaDifferenceSquared = 0;

            foreach(float position in positions)
            {
                //(x- mean)^2  here 
                float difference = position - xMean;
                float differencesquared = difference * difference;
                SigmaDifferenceSquared = SigmaDifferenceSquared + differencesquared;
            }

            double UnRootedVar = SigmaDifferenceSquared / xN;
            double xSD = Math.Sqrt(UnRootedVar);
            standarddeviationX = (float)xSD;

            //below is to work out S.D for Y values. 

            float yN = Inputresults.Count;
            float yMean = 0;
            foreach(RiderResults results in Inputresults)
            {
                yMean = yMean + results.Racedifficulty;
            }

            yMean = yMean / yN;

            float[] racedifficultyvalues = new float[Inputresults.Count];

            float ySigmaDifferenceSquared = 0;

            int yCounter = 0;
            foreach(RiderResults results in Inputresults)
            {
                
                racedifficultyvalues[yCounter] = results.Racedifficulty;
                yCounter = yCounter + 1;
                float difference = results.Racedifficulty - yMean;
                float differencesquared = difference * difference;
                ySigmaDifferenceSquared = ySigmaDifferenceSquared + differencesquared;
            }

            double UnRootedYVar = ySigmaDifferenceSquared / yN;
            double ySD = Math.Sqrt(UnRootedYVar);
            standarddeviationY = (float)ySD;

            //Sigma(Xi - xbar)(Yi - Ybar)
            float covariance = 0; 

            for (int pearsonscounter = 0; pearsonscounter < racedifficultyvalues.Length; pearsonscounter++)
            {
                float Xsingularcovariance = positions[pearsonscounter] - xMean;
                float YSingularcovariance = racedifficultyvalues[pearsonscounter] - yMean;
                float Singularcovariance = Xsingularcovariance * YSingularcovariance;
                covariance = covariance + Singularcovariance;
                    
            }

            //Sum of X and Y standard deviations 
            float sumofSD = standarddeviationX * standarddeviationY;
            float sumofSDn = sumofSD * Inputresults.Count;

            float PearsonsRankCoefficient = covariance / sumofSDn;

            return PearsonsRankCoefficient;
        }
    }
}
