using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cycling1._1
{

    [Serializable]
    public class Riders
    {
        private string fullname;

        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }

        private int riderseasonpoints;

        public int Riderseasonpoitns
        {
            get { return riderseasonpoints; }
            set { riderseasonpoints = value; }
        }

        private int age; 

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        private string team;

        public string Team
        {
            get { return team; }
            set { team = value; }
        }

        private List<RiderResults> riderresult; 

        public List<RiderResults> Riderresult
        {
            get { return riderresult; }
            set { riderresult = value; }
        }

        private List<RiderResults> riderstageraceresult;

        public List<RiderResults> RiderStageRaceResult
        {
            get { return riderstageraceresult; }
            set { riderstageraceresult = value; }
        }

        private List<RiderResults> _PCTStageRaceStageResults;

        public List<RiderResults> PCTStageRaceStageResults
        {
            get { return _PCTStageRaceStageResults; }
            set { _PCTStageRaceStageResults = value; }
        }

        private List<RiderResults> _PCTOneDayRaceResults;

        public List<RiderResults> PCTOneDayRaceResults
        {
            get { return _PCTOneDayRaceResults; }
            set { _PCTOneDayRaceResults = value; }
        }

        private List<RiderGCResults> riderGC;

        public List<RiderGCResults> RiderGC
        {
            get { return riderGC; }
            set { riderGC = value; }
        }

        private float climbingPearsonsrankcoefficient;

        public float ClimbingPearsonsrankcoefficient
        {
            get { return climbingPearsonsrankcoefficient; }
            set { climbingPearsonsrankcoefficient = value; }
        }

        private float _WTPearsonsRankLength;

        public float WTPearsonsRankLength
        {
            get { return _WTPearsonsRankLength; }
            set { _WTPearsonsRankLength = value; }
        }


        private float _PCTPearsonsRankLength;
        public float PCTPearsonsRankLength
        {
            get { return _PCTPearsonsRankLength; }
            set { _PCTPearsonsRankLength = value; }
        }

        private AdvancedMetrics _FurtherMetrics;

        public AdvancedMetrics FurtherMetrics
        {
            get { return _FurtherMetrics; }
            set { _FurtherMetrics = value; }
        }


        public Riders(string nFullname, int nRiderseasonpoints, int nAge, string nTeam)
        {
            this.Fullname = nFullname;
            this.Riderseasonpoitns = nRiderseasonpoints;
            this.Age = nAge;
            this.Team = nTeam;
        }

        public Riders()
        {

        }

        public void RaceLengthPearsonsCoefficient(Riders iRider)
        {
            List<RiderResults> PCTTotalResults = iRider._PCTStageRaceStageResults;
            List<RiderResults> PCTOneDayResults = iRider._PCTOneDayRaceResults;
            PCTTotalResults.AddRange(PCTOneDayResults);
            iRider._PCTPearsonsRankLength = RaceLengthPearsonsCalculator(PCTTotalResults);

            List<RiderResults> WTTotalResults = iRider.riderstageraceresult;
            List<RiderResults> WTOneDayResults = iRider.riderresult;
            WTTotalResults.AddRange(WTOneDayResults);
            iRider.WTPearsonsRankLength = RaceLengthPearsonsCalculator(WTTotalResults);

        }

        public float RaceLengthPearsonsCalculator(List<RiderResults> iRiderResult)
        {
            float StandardDeviationX;
            float StandardDeviationY;
            float xN = iRiderResult.Count;
            float xMean;
            float SigmaX= 0;

            float[] RaceLengths = new float[iRiderResult.Count];

            int counter = 0; 
            foreach (RiderResults result in iRiderResult)
            {
                float LengthOfResult = result.Lengthdifficulty;
                SigmaX = SigmaX + LengthOfResult;
                RaceLengths[counter] = LengthOfResult;
                counter = counter + 1; 
            }

            xMean = SigmaX / xN;

            float SigmaDifferenceSquared = 0;
            foreach (float length in RaceLengths)
            {
                float difference = length - xMean;
                float differenceSquared = difference * difference;
                SigmaDifferenceSquared = SigmaDifferenceSquared + differenceSquared;
            }

            double unRootedVar = SigmaDifferenceSquared / xN;
            StandardDeviationX = (float)Math.Sqrt(unRootedVar);
            //Standard Deviation for Y (Positions) below 
            float yMean = 0;
            float WorstResultY = 0;
            foreach (RiderResults result in iRiderResult)
            {
                if (result.Position.Equals("DNF") || result.Position.Equals("OTL")|| result.Position.Equals("DNS")|| result.Position.Equals(""))
                {
                    continue;
                }
                float floatresult = float.Parse(result.Position);
                if (floatresult > WorstResultY)
                {
                    WorstResultY = floatresult;
                }
            }

            int counter2 = 0;
            float[] positions = new float[iRiderResult.Count];
            foreach (RiderResults result in iRiderResult)
            {
                if (result.Position.Equals("DNF") || result.Position.Equals("OTL") || result.Position.Equals("")|| result.Position.Equals("DNS"))
                {
                    float yValue = WorstResultY;
                    yMean = yMean + yValue;
                    positions[counter2] = yValue;
                    counter2 = counter2 + 1;
                }
                else
                {
                    float yValue = float.Parse(result.Position);
                    yMean = yMean + yValue;
                    positions[counter2] = yValue;
                    counter2 = counter2 + 1;
                }
            }

            yMean = yMean / xN;

            float YSigmaDifferenceSquared = 0;

            foreach(float position in positions)
            {
                float difference = position - yMean;
                float differencesquared = difference * difference;
                YSigmaDifferenceSquared = YSigmaDifferenceSquared + differencesquared;
            }

            double UnRootedYVar = YSigmaDifferenceSquared / xN;
            StandardDeviationY = (float)Math.Sqrt(UnRootedYVar);

            //Signa(Xi - Xbar)(Yi - YBar)
            float CoVariance = 0;

            for (int PearsonsRankCounter = 0; PearsonsRankCounter < positions.Length; PearsonsRankCounter++)
            {
                float XsingularCovariance = RaceLengths[PearsonsRankCounter] - xMean;
                float YsingularCovariance = positions[PearsonsRankCounter] - yMean;
                float SingularCovariance = XsingularCovariance * YsingularCovariance;
                CoVariance = CoVariance + SingularCovariance;
            }
            float SumOfSD = StandardDeviationX * StandardDeviationY;
            float SumOfSDn = SumOfSD * xN;
            float PearsonsRankCoefficient = CoVariance / SumOfSDn;

            return PearsonsRankCoefficient;

        }

        public void StandardDeviationOfMountainStages(List<Riders> iRider)
        {
            float StandardDeviation;
            foreach(var rider in iRider)
            {
                List<RiderResults> MountainStages = new List<RiderResults>(); 
                List<RiderResults> AllStageRaceResults = rider.RiderStageRaceResult;
                AllStageRaceResults.AddRange(rider.PCTStageRaceStageResults);

                foreach(var result in AllStageRaceResults)
                {
                    if(result.Racedifficulty > 170)
                    {
                        MountainStages.Add(result);

                    }
                }

                float NValue = MountainStages.Count;
                float mean = 0; 

                float positionmax = 100;
                float positionmin = 100;

                foreach(var stage in MountainStages)
                {
                    if (stage.Position.Contains("OTL") || stage.Position.Contains("DNF") || stage.Position.Contains("DNS"))
                    {
                        continue;
                    }
                    if (float.Parse(stage.Position) > positionmax)
                    {
                        positionmax = float.Parse(stage.Position);
                    }
                    if (float.Parse(stage.Position) < positionmin)
                    {
                        positionmin = float.Parse(stage.Position);
                    }
                }

                float range = positionmax - positionmin;

                foreach(var stage in MountainStages)
                {
                    float position;
                    if (stage.Position.Contains("OTL") || stage.Position.Contains("DNF") || stage.Position.Contains("DNS"))
                    {
                        position = range/2;
                        mean = mean + position;
                        continue;
                    }
                    else
                    {
                        position = float.Parse(stage.Position);
                        mean = mean + position;
                    }
                }

                mean = mean / NValue;
                float XMinusMean = 0;
                foreach (var stage in MountainStages)
                {
                    float position;
                    if (stage.Position.Contains("OTL") || stage.Position.Contains("DNF") || stage.Position.Contains("DNS"))
                    {
                        position = range/2;
                        position = position - mean;
                        position = position * position;
                        XMinusMean = XMinusMean + position;
                        continue;
                    }
                    else
                    {
                        position = float.Parse(stage.Position);
                        position = position - mean;
                        position = position * position;
                        XMinusMean = XMinusMean + position;
                    }
                }
                
                float variance = XMinusMean / NValue;
                double SD = Math.Sqrt(variance);
                StandardDeviation = Convert.ToSingle(SD);

                AdvancedMetrics Metrics = new AdvancedMetrics();
                Metrics.StandardDeviationInMountainStages = StandardDeviation;
                Metrics.VarianceOfMountainStages = variance;

                rider.FurtherMetrics = Metrics;
                

            }

        }
        public void CobbledConsistency(List<Riders> iRiderList)
        {
            foreach(var rider in iRiderList)
            {
                List<RiderResults> MergedOneDayRaces = rider.PCTOneDayRaceResults;
                MergedOneDayRaces.AddRange(rider.Riderresult);
                List<RiderResults> CobbledResults = new List<RiderResults>();
                
                foreach (var result in MergedOneDayRaces)
                {
                    float cobbledifficulty = result.Cobbledifficulty;
                    if(cobbledifficulty > 0)
                    {
                        CobbledResults.Add(result);
                    }
                }

                float nValue = CobbledResults.Count;
                float xMeanPosition = 0;

                foreach(var CobbleResult in CobbledResults)
                {
                    if (CobbleResult.Position.Contains("DNF") || CobbleResult.Position.Equals("") || CobbleResult.Position.Contains("OTL"))
                    {
                        xMeanPosition = xMeanPosition + 100;
                    }
                    else
                    {
                        xMeanPosition = xMeanPosition + float.Parse(CobbleResult.Position);
                    }
                }

                xMeanPosition = xMeanPosition / nValue;

                float SigmaDifferenceSquared = 0;

                foreach(var CobbleResult in CobbledResults)
                {
                    float position;
                    if (CobbleResult.Position.Contains("DNF") || CobbleResult.Position.Contains("OTL") || CobbleResult.Position.Equals(""))
                    {
                        position = 100;
                    }
                    else
                    {
                        position = float.Parse(CobbleResult.Position);
                    }
                    float PositionMinusMean = position - xMeanPosition;
                    PositionMinusMean = PositionMinusMean * PositionMinusMean;
                    SigmaDifferenceSquared = SigmaDifferenceSquared + PositionMinusMean;
                }

                double Variance = SigmaDifferenceSquared / nValue;
                float StandardDeviation = Convert.ToSingle(Math.Sqrt(Variance));
                rider.FurtherMetrics.StandardDeviationOfCobbledRaces = StandardDeviation;

            }
        }

        public float ClimbingRating(Riders iRider)
        {
           
            float StandardDeviation = iRider.FurtherMetrics.StandardDeviationInMountainStages;
            float averageclimbingposition;

            List<RiderResults> ResultsofRider = iRider.RiderStageRaceResult;
            ResultsofRider.AddRange(PCTStageRaceStageResults);

            List<RiderResults> StageResults = new List<RiderResults>();

            foreach(var result in ResultsofRider)
            {
                if(result.Racedifficulty > 175)
                {
                    StageResults.Add(result);
                }
            }

            float resultsmean = 0;

            for (int x = 0; x < StageResults.Count; x++)
            {
                if (StageResults[x].Position.Contains("DNF") || StageResults[x].Position.Contains("OTL") || StageResults[x].Position.Equals("") || StageResults[x].Position.Contains("DNS"))
                {
                    float BadResult = 100;
                    resultsmean = resultsmean + BadResult;
                }
                else
                {
                    float Result = float.Parse(StageResults[x].Position);
                    resultsmean = resultsmean + Result;
                }
            }

            averageclimbingposition = resultsmean / StageResults.Count;

            float ClimbingRating = (150 - averageclimbingposition) - (iRider.FurtherMetrics.StandardDeviationInMountainStages / 2) * (iRider.ClimbingPearsonsrankcoefficient + 1);
            return ClimbingRating;
        }



    }

    [Serializable]
    public class RiderResults : raceresults

    {
        private string nameofrace; 

        public string Nameofrace
        {
            get { return nameofrace; }
            set { nameofrace = value; }
        }

        private date dateofresult; 

        public date Dateofresult
        {
            get { return dateofresult; }
            set { dateofresult = value; }
        }

        private float racelength;


        public float Racelength
        {
            get { return racelength; }
            set { racelength = value; }
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


        public RiderResults(string nNameofTeam, string nNameofRace, string nPosition, date ndate, float nRaceLength, int nRacedifficulty, string nRaceType, int nCobbledifficulty, float nClimbDifficultyNormalised, float nLengthDifficultyNormalized )
        {
            this.Team = nNameofTeam;
            this.Nameofrace = nNameofRace;
            this.Position = nPosition;
            this.Dateofresult = ndate;
            this.Racelength = nRaceLength;
            this.Racedifficulty = nRacedifficulty;
            this.Racetype = nRaceType;
            this.Cobbledifficulty = nCobbledifficulty;
            this.Climbdifficultynormalized = nClimbDifficultyNormalised;
            this.Lengthdifficulty = nLengthDifficultyNormalized;
        }



        public RiderResults()
        {

        }

      
    }


    [Serializable]
    public class RiderGCResults : RiderResults
    {
        private string nameofGCRace; 

        public string NameofGCRace
        {
            get { return nameofGCRace; }
            set { nameofGCRace = value; }
        }

        private date dateofGC;

        public date DateofGC
        {
            get { return dateofGC; }
            set { dateofGC = value; }
        }

        private int numberofstages;

        public int Numberofstages
        {
            get { return numberofstages; }
            set { numberofstages = value; }
        }

        private List<GCGapsForRiderOnMountainStage> _TimeLossPerStage;

        public List<GCGapsForRiderOnMountainStage> TimeLossPerStage
        {
            get { return _TimeLossPerStage; }
            set { _TimeLossPerStage = value; }
        }

        public RiderGCResults(string nNameofGCRace, string nNameofTeam, string nPosition, date nDate, int nNumberofStages)
        {
            this.NameofGCRace = nNameofGCRace;
            this.Team = nNameofTeam;
            this.Position = nPosition;
            this.DateofGC = nDate;
            this.Numberofstages = nNumberofStages;
        }

        public RiderGCResults()
        {

        }

        public float GCRatingCalculator(List<raceresults> iResultsOfRider, List<RiderGCResults> iRiderGCResults)
        {

            float GCRating = 0;
            return GCRating;
        }

        public void GCResultsGapsMatcher(List<RiderGCResults> iGCResults, Riders iRider)
        {
            List<Stageraces> AllStageRaces = Singleton_Class.ListofPCTStageRaces;
            AllStageRaces.AddRange(Singleton_Class.ListofStageRaces);

            foreach(var stagerace in AllStageRaces)
            {
                List<GCGapsForRiderOnMountainStage> GapsOfRidersInRace = new List<GCGapsForRiderOnMountainStage>();
                foreach(var result in iGCResults)
                {
                    if (result.NameofGCRace.Contains(stagerace.Racename) && result.DateofGC.Year == stagerace.Stageresults[0].Dates.Year)
                    {
                        for(int x =0; x < stagerace.GeneralClassificationBreakdown.Count; x++)
                        {
                            if(stagerace.GeneralClassificationBreakdown[x].NameofRider.Contains(iRider.Fullname))
                            {
                                foreach(int gap in stagerace.GeneralClassificationBreakdown[x].ListOfGCGapsPerStage)
                                {
                                    GCGapsForRiderOnMountainStage GapOnstage = new GCGapsForRiderOnMountainStage();
                                    GapOnstage.GapOnStage = gap;
                                }
                            }
                        }

                    }
                }
            }
        }
    }

    public class GCGapsForRiderOnMountainStage
    {
        private string _StageNumber;

        public string StageNumber
        {
            get { return _StageNumber; }
            set { _StageNumber = value; }
        }

        private float _ClimbingRating;

        public float ClimbingRating
        {
            get { return _ClimbingRating; }
            set { _ClimbingRating = value; }
        }

        private int _GapOnStage;

        public int GapOnStage
        {
            get { return _GapOnStage; }
            set { _GapOnStage = value; }
        }

        public GCGapsForRiderOnMountainStage()
        {

        }
    }



    public class AdvancedMetrics
    {
        private int _AverageClimbingInGCRatingThroughoutCareer;

        public int AverageClimbingInGCRatingThroughoutCareer
        {
            get { return _AverageClimbingInGCRatingThroughoutCareer; }
            set { _AverageClimbingInGCRatingThroughoutCareer = value; }
        }

        private float _StandardDeviationInMountainStages;

        public float StandardDeviationInMountainStages
        {
            get { return _StandardDeviationInMountainStages; }
            set { _StandardDeviationInMountainStages = value; }
        }

        private float _VarianceOfMountainStages;

        public float VarianceOfMountainStages
        {
            get { return _VarianceOfMountainStages; }
            set { _VarianceOfMountainStages = value; }
        }

        private float _StandardDeviationOfCobbledRaces;

        public float StandardDeviationOfCobbledRaces
        {
            get { return _StandardDeviationOfCobbledRaces; }
            set { _StandardDeviationOfCobbledRaces = value; }
        }

        private float _ClimbingRating; 

        public float ClimbingRating
        {
            get { return _ClimbingRating; }
            set { _ClimbingRating = value; }
        }
        public AdvancedMetrics()
        {

        }
         
    }
        


    

}
