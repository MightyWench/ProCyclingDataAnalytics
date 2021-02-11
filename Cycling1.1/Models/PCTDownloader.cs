using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cycling1._1
{
    class PCTDownloader
    {
      
       //This Class downloads both the PCT Stage Races and One Day Races from PCS Internal Data Stores. 

        public static async void PCTOneDayRacesDownloader()
        {

            bool filechecker = false;

            if (File.Exists(@"C:\Users\Wenqing Huang\c#\PCTOneDayRaces.xml"))
            {
                filechecker = true;
            }

            while (filechecker == false)
            {
                var PCTListOfOneDayRaces = "https://www.procyclingstats.com/races.php?s=races-database&name=&nation=&class=1.Pro&category=1&filter=Filter";

                var PCTRacesClient = new HttpClient();
                var PCTRacesInHTML = await PCTRacesClient.GetStringAsync(PCTListOfOneDayRaces);
                var PCTRacesInDocument = new HtmlDocument();
                PCTRacesInDocument.LoadHtml(PCTRacesInHTML);

                var PCTOneDayListRaces = PCTRacesInDocument.DocumentNode.Descendants("table")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Contains("basic")).ToList();

                var PCTOneDayRaces = PCTOneDayListRaces[0].Descendants("a")
                    .Where(node => node.GetAttributeValue("href", "")
                    .Contains("race")).ToList();

                int counter = 0;
                string[] PCTOneDayRaceNames = new string[PCTOneDayRaces.Count];
                string[] PCTOneDayRaceStatus = new string[PCTOneDayRaces.Count];

                foreach (var race in PCTOneDayRaces)
                {

                    PCTOneDayRaceNames[counter] = race.InnerText.ToString();
                    PCTOneDayRaceStatus[counter] = "1.Pro";
                    counter = counter + 1;
                }

                string[] PCTOneDayRaceUrlArray = { "tre-valli-varesine", "bredene-koksijde-classic", "clasica-de-almeria", "coppa-bernocchi", "nokere-koers", "brabantse-pijl", "les-boucles-du-dus-ardeche",
            "giro-dell-emilia", "gp-de-fourmies", "gp-industria-artigianato", "gran-piemonte", "gp-miguel-indurain", "gp-de-denain", "gp-de-wallonie", "gp-de-plumelec", "japan-cup", "kuurne-brussel-kuurne",
            "maryland-cycling-classic", "milano-torino", "paris-tours", "gp-impanis-van-petegem", "la-drome-classic", "scheldeprijs", "munsterland-giro", "tro-bro-leon", "trofeo-laigueglia", "brussels-cycling-classic",
            "gp-citta-di-peccioli", "dwars-door-het-hageland", "circuit-franco-belge", "gran-trittico-lombardo"};

                for (int a = 0; a < 31; a++)
                {
                    for (int year = 2011; year < 2020; year++)
                    {
                        string race = PCTOneDayRaceUrlArray[a];

                        if (race == "nokere-koers" && year == 2013 || race == "milano-torino" && year == 2011 || race == "la-drome-classic" && year < 2014 || race == "dwars-door-het-hageland" && (year > 2012 && year < 2016))
                        {
                            continue;
                        }
                        if (race == "gp-industria-artigianato" && year == 2015 || race == "kuurne-brussel-kuurne" && year == 2013 || race == "maryland-cycling-classic")
                        {
                            continue;
                        }
                        if (race == "gran-piemonte" && year == 2013 || race == "gran-piemonte" && year == 2014 || race == "gran-piemonte" && year == 2017 || race == "gran-trittico-lombardo" || race == "circuit-franco-belge" && year < 2016)
                        {
                            continue;
                        }

                        var SpecificPCTOneDayUrl = "https://www.procyclingstats.com/race/" + race + "/" + year;

                        var PCTRaceDownloaderClient = new HttpClient();
                        var PCTRaceOneDayHTML = await PCTRaceDownloaderClient.GetStringAsync(SpecificPCTOneDayUrl);
                        var PCTRaceOneDayInDocument = new HtmlDocument();
                        PCTRaceOneDayInDocument.LoadHtml(PCTRaceOneDayHTML);

                        var SpecificPCTOneDayDatesURL = SpecificPCTOneDayUrl + "/" + "overview";

                        var PCTOneDayDatesClient = new HttpClient();
                        var PCTOneDayDatesHTML = await PCTOneDayDatesClient.GetStringAsync(SpecificPCTOneDayDatesURL);
                        var PCTOneDayDatesDocument = new HtmlDocument();
                        PCTOneDayDatesDocument.LoadHtml(PCTOneDayDatesHTML);

                        var PCTOneDayRaceDateSection = PCTOneDayDatesDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("w36 left")).ToList();

                        var PCTOneDayRaceLengthInitial = PCTOneDayDatesDocument.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("entry race")).ToList();

                        var PCTOneDayRaceLengthParsed = PCTOneDayRaceLengthInitial[0].Descendants("span")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("red distance")).ToList();

                        string PCTLengthofRace = Regex.Replace(PCTOneDayRaceLengthParsed[0].InnerText.ToString(), @"[^0-9\.]", "");
                        float _PCTLengthofRace = float.Parse(PCTLengthofRace);




                        string DayofRace = PCTOneDayRaceDateSection[0].InnerText.ToString().Substring(18, 2);
                        int day = int.Parse(DayofRace);
                        string MonthofRace = PCTOneDayRaceDateSection[0].InnerText.ToString().Substring(15, 2);
                        int month = int.Parse(MonthofRace);

                        date racedate = new date();
                        racedate.Day = day;
                        racedate.Month = month;
                        racedate.Year = year;

                        var PCTOneDayRacesParsed = PCTRaceOneDayInDocument.DocumentNode.Descendants("table")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("basic results")).ToList();

                        var PCTOneDayRacesRiders = PCTOneDayRacesParsed[0].Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("rider")).ToList();

                        var PCTOneDayRaceResultRankings = PCTOneDayRacesParsed[0].Descendants("tr")
                            .Where(node => node.GetAttributeValue("data-id", "")
                            .Contains("")).ToList();

                        string[] placerholderrankings = new string[PCTOneDayRaceResultRankings.Count - 1];
                        string[] parsedrankings = new string[PCTOneDayRaceResultRankings.Count - 1];

                        for (int FirstRankingLoop = 1; FirstRankingLoop < PCTOneDayRaceResultRankings.Count; FirstRankingLoop++)
                        {
                            placerholderrankings[FirstRankingLoop - 1] = PCTOneDayRaceResultRankings[FirstRankingLoop].InnerHtml;
                        }

                        for (int SecondRankingLoop = 0; SecondRankingLoop < placerholderrankings.Length; SecondRankingLoop++)
                        {
                            string RankingParsed = placerholderrankings[SecondRankingLoop].Substring(0, 7);
                            string result = "";

                            for (int RankingThirdLoop = 0; RankingThirdLoop < 7; RankingThirdLoop++)
                            {
                                if (char.IsDigit(RankingParsed[RankingThirdLoop]))
                                {
                                    result = result + RankingParsed[RankingThirdLoop];
                                }
                            }

                            if (RankingParsed.Contains("OTL"))
                            {
                                result = "OTL";
                            }
                            if (RankingParsed.Contains("DNF"))
                            {
                                result = "DNF";
                            }
                            parsedrankings[SecondRankingLoop] = result;
                        }

                        string[] ParsedPCTOneDayRiderNames = new string[PCTOneDayRacesRiders.Count];

                        for (int i = 0; i < PCTOneDayRacesRiders.Count; i++)
                        {
                            ParsedPCTOneDayRiderNames[i] = PCTOneDayRacesRiders[i].InnerText.ToString();
                        }

                        var ParsedPCTOneDayRaceTime = PCTOneDayRacesParsed[0].Descendants("span")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("timelag")).ToList();

                        var ParsedPCTOneDayRaceTeam = PCTOneDayRacesParsed[0].Descendants("td")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("cu600")).ToList();

                        string[] PCTOneDayRaceTimes = new string[ParsedPCTOneDayRiderNames.Length];
                        string[] PositionInRace = new string[ParsedPCTOneDayRiderNames.Length];
                        string[] TeamOfRider = new string[ParsedPCTOneDayRiderNames.Length];

                        for (int j = 0; j < ParsedPCTOneDayRiderNames.Length; j++)
                        {
                            PCTOneDayRaceTimes[j] = ParsedPCTOneDayRaceTime[j].InnerText.ToString();
                            PositionInRace[j] = parsedrankings[j];
                            TeamOfRider[j] = ParsedPCTOneDayRaceTeam[j].InnerText.ToString();
                        }

                        List<raceresults> ResultsofRace = new List<raceresults>();

                        for (int k = 0; k < ParsedPCTOneDayRiderNames.Length; k++)
                        {
                            ResultsofRace.Add(new raceresults(PositionInRace[k], ParsedPCTOneDayRiderNames[k], PCTOneDayRaceTimes[k], TeamOfRider[k]));
                        }

                        var ClimbDifficulty = PCTRaceOneDayInDocument;

                        var ClimbDifficultyParsed = ClimbDifficulty.DocumentNode.Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("profile-score")).ToList();

                        string DifficultyOfClimb = ClimbDifficultyParsed[0].InnerText.ToString();
                        string RegexDifficulty = Regex.Replace(DifficultyOfClimb, @"[^0-9\.]+", "");
                        int difficultyinteger = int.Parse(RegexDifficulty);

                        int cobbleddifficulty = 0;

                        string racetype = "Default";
                        switch (difficultyinteger)
                        {
                            case var _ when difficultyinteger <= 20:
                                racetype = "Sprinters Race";
                                break;

                            case var _ when difficultyinteger > 20 && difficultyinteger <= 50:
                                racetype = "Medium Hilly Race";
                                break;

                            case var _ when difficultyinteger > 50 && difficultyinteger <= 125:
                                racetype = "Hard Hilly Race";
                                break;

                            case var _ when difficultyinteger > 125 && difficultyinteger <= 225:
                                racetype = "Medium Mountain Race";
                                break;

                            case var _ when difficultyinteger > 225:
                                racetype = "Hardest Mountain Race/High-Cols";
                                break;

                        }

                        Singleton_Class.ListofPCTOneDayRaces.Add(new Races(race, PCTOneDayRaceStatus[1], racetype, year, ResultsofRace, racedate, _PCTLengthofRace, difficultyinteger, cobbleddifficulty));

                    }

                }

                float maxlength = Singleton_Class.ListofPCTOneDayRaces[0].Racelength;
                float minlength = Singleton_Class.ListofPCTOneDayRaces[0].Racelength;

                for (int maxminchecker = 0; maxminchecker < Singleton_Class.ListofPCTOneDayRaces.Count(); maxminchecker++)
                {
                    if (Singleton_Class.ListofPCTOneDayRaces[maxminchecker].Racelength > maxlength)
                    {
                        maxlength = Singleton_Class.ListofPCTOneDayRaces[maxminchecker].Racelength;
                    }
                    if (Singleton_Class.ListofPCTOneDayRaces[maxminchecker].Racelength < minlength)
                    {
                        minlength = Singleton_Class.ListofPCTOneDayRaces[maxminchecker].Racelength;
                    }
                }

                float racelengthnormalized(float racelength)
                {
                    float normalizedlength = ((racelength - minlength) / (maxlength - minlength));
                    return normalizedlength;
                }

                float maxdifficulty = Singleton_Class.ListofPCTOneDayRaces[0].Racedifficulty;
                float mindifficulty = Singleton_Class.ListofPCTOneDayRaces[0].Racedifficulty;

                for (int difficultychecker = 0; difficultychecker < Singleton_Class.ListofPCTOneDayRaces.Count(); difficultychecker++)
                {
                    if (Singleton_Class.ListofPCTOneDayRaces[difficultychecker].Racedifficulty > maxdifficulty)
                    {
                        maxdifficulty = Singleton_Class.ListofPCTOneDayRaces[difficultychecker].Racedifficulty;
                    }
                    if (Singleton_Class.ListofPCTOneDayRaces[difficultychecker].Racedifficulty < mindifficulty)
                    {
                        mindifficulty = Singleton_Class.ListofPCTOneDayRaces[difficultychecker].Racedifficulty;
                    }
                }

                float racedifficultynormalized(float racedifficulty)
                {
                    float normalizeddifficulty = ((racedifficulty - mindifficulty) / (maxdifficulty - mindifficulty));
                    return normalizeddifficulty;
                }

                foreach (var race in Singleton_Class.ListofPCTOneDayRaces)
                {
                    float racelengthdifficulty = race.Racelength;
                    race.Lengthdifficulty = racelengthnormalized(racelengthdifficulty);

                    float normalizedclimbdifficulty = race.Racedifficulty;
                    race.Climbdifficultynormalized = racedifficultynormalized(normalizedclimbdifficulty);
                }

                using (TextWriter PCTOneDayTextWriter = new StreamWriter(@"C:\Users\Wenqing Huang\c#\PCTOneDayRaces.xml"))
                {
                    XmlSerializer PCTOneDaySerializer = new XmlSerializer(typeof(List<Races>));
                    PCTOneDaySerializer.Serialize(PCTOneDayTextWriter, Singleton_Class.ListofPCTOneDayRaces);
                }
                filechecker = true;
            }

            Console.ReadLine();

            if (filechecker == true)
            {
                XmlSerializer PCTOneDayLoader = new XmlSerializer(typeof(List<Races>));

                using (FileStream PCTLoader = File.OpenRead(@"C:\Users\Wenqing Huang\c#\PCTOneDayRaces.xml"))
                {
                    Singleton_Class.ListofPCTOneDayRaces = (List<Races>)PCTOneDayLoader.Deserialize(PCTLoader);
                }
            }

            PCTStageRaceDownloader();
        }

        public static async void PCTStageRaceDownloader()
        {

            bool filechecker = false;

            if (File.Exists(@"C:\Users\Wenqing Huang\c#\PCTStageRaces.xml"))
            {
                filechecker = true;
            }

            while (filechecker == false)
            {
                var PCTStageRacesListURL = "https://www.procyclingstats.com/races.php?s=races-database&name=&nation=&class=2.Pro&category=1&filter=Filter";

                var PCTStageRaceClient = new HttpClient();
                var PCTStageRaceHTML = await PCTStageRaceClient.GetStringAsync(PCTStageRacesListURL);
                var PCTStageRaceDocument = new HtmlDocument();
                PCTStageRaceDocument.LoadHtml(PCTStageRaceHTML);

                var PCTStageRaceNamesExtraction = PCTStageRaceDocument.DocumentNode.Descendants("a")
                    .Where(node => node.GetAttributeValue("href", "")
                    .Contains("overview")).ToList();

                string[] PCTStageRaceNames = new string[25];
                string[] PCTStageRaceStatus = new string[25];

                for (int counter = 0; counter < 24; counter++)
                {
                    {
                        PCTStageRaceNames[counter] = PCTStageRaceNamesExtraction[counter].InnerText;
                        PCTStageRaceStatus[counter] = "2.Pro";
                    }
                }
                PCTStageRaceNames[24] = "Skoda-Tour de Luxembourg";
                PCTStageRaceStatus[24] = "2.Pro";

                string[] PCTStageURL = { "4-jours-de-dunkerque", "tour-of-turkey", "arctic-race-of-norway", "tour-of-belgium", "boucles-de-la-mayenne", "deutschland-tour", "tour-of-austria", "tour-de-langkawi", "tour-of-denmark", "tour-of-utah", "tour-cycliste-international-la-provence",
            "tour-de-yorkshire", "tour-of-britain", "tour-of-oman", "tour-of-qinghai-lake", "tour-of-slovenia", "tour-of-taihu-lake", "tour-of-the-alps", "vuelta-a-la-comunidad-valenciana", "volta-ao-algarve", "tour-de-wallonie", "ruta-del-sol", "vuelta-a-burgos",
            "vuelta-ciclista-a-la-provincia-de-san-juan","tour-de-luxembourg"};

                for (int RaceIterator = 0; RaceIterator < PCTStageRaceNames.Length; RaceIterator++)
                {
                    for (int year = 2011; year < 2020; year++)
                    {
                        List<stages> StagesInPCTStageRace = new List<stages>();
                        string RaceName = PCTStageURL[RaceIterator];

                        if (RaceName.Contains("tour-of-britain") && year == 2016)
                        {
                            continue;
                        }

                        var OverviewOfPCTStageRaceURL = "https://www.procyclingstats.com/race/" + RaceName + "/" + year + "/overview";

                        var OverViewOfPCTStageRaceClient = new HttpClient();
                        var OverViewHTML = await OverViewOfPCTStageRaceClient.GetStringAsync(OverviewOfPCTStageRaceURL);
                        var OverViewinDocument = new HtmlDocument();
                        OverViewinDocument.LoadHtml(OverViewHTML);

                        var OverviewOfStages = OverViewinDocument.DocumentNode.Descendants("li")
                            .Where(node => node.GetAttributeValue("style", "")
                            .Contains("padding")).ToList();

                        if (OverviewOfStages.Count == 0)
                        {
                            continue;
                        }

                        var PrologueChecker = OverviewOfStages[0].Descendants("span")
                            .Where(node => node.GetAttributeValue("style", "")
                            .Contains("color:")).ToList();

                        string LengthofStagePrologue = PrologueChecker[0].InnerText;
                        string LengthofStageParsed = Regex.Replace(LengthofStagePrologue, @"[^0-9\.]+", "");
                        float FloatLengthofStage = float.Parse(LengthofStageParsed);

                        bool Prologue = false;
                        bool PrologueFixer = false;
                        if (FloatLengthofStage < 8 || RaceName.Contains("tour-of-slovenia") && year == 2013)
                        {
                            Prologue = true;

                        }
                        if (Prologue == true)
                        {
                            PrologueFixer = true;
                        }
                        int StageNumbers = OverviewOfStages.Count;
                        bool TourDeAlpsB = false;
                        bool RutaDelSol = false;
                        bool TourOfBritain = false;
                        bool TOBComplete = false;

                        for (int stageCounter = 1; stageCounter <= StageNumbers; stageCounter++)
                        {

                            bool SecondIterationPrologue = false;
                            var FullStageURL = "";
                            string UrlOfStage;
                            if (Prologue == true)
                            {
                                UrlOfStage = "prologue";
                                Prologue = false;
                                SecondIterationPrologue = true;
                            }
                            else
                            {
                                UrlOfStage = "stage-" + stageCounter;
                            }
                            if (TourDeAlpsB == true)
                            {
                                UrlOfStage = "stage-1b";
                                TourDeAlpsB = false;
                            }
                            if (RaceName.Contains("tour-of-the-alps") && year == 2013 && stageCounter == 1)
                            {
                                // UrlOfStage = "stage-1a";
                                //  TourDeAlpsB = true;
                                break;
                            }

                            if (RutaDelSol == true)
                            {
                                UrlOfStage = "stage-1b";
                                RutaDelSol = false;
                            }

                            if (RaceName.Contains("ruta-del-sol") && year == 2015 && stageCounter == 1)
                            {
                                break;
                            }

                            if (TourOfBritain == true && TOBComplete == true)
                            {
                                UrlOfStage = "stage-8b";
                                TourOfBritain = false;
                            }


                            if (RaceName.Contains("tour-of-britain") && year == 2014 && stageCounter == 8 && TOBComplete == false)
                            {
                                UrlOfStage = "stage-8a";
                                TourOfBritain = true;
                                TOBComplete = true;
                            }


                            if (RaceName.Contains("tour-of-belgium") && year != 2011 || RaceName.Contains("boucles-de-la-mayenne") && year != 2011 || RaceName.Contains("tour-of-austria") || RaceName.Contains("tour-of-utah")
                                || RaceName.Contains("tour-cycliste-international-la-provence") || RaceName.Contains("tour-of-taihu-lake") || RaceName.Contains("ruta-del-sol") && year != 2011 || RaceName.Contains("tour-de-luxembourg") && year != 2011)
                            {
                                if (PrologueFixer == true)
                                {
                                    if (SecondIterationPrologue == false)
                                    {
                                        int StageInteger = stageCounter - 1;
                                        string StageMinus = "stage-" + StageInteger;
                                        FullStageURL = "https://www.procyclingstats.com/race/" + RaceName + "/" + year + "/" + StageMinus;
                                    }
                                    else
                                    {
                                        FullStageURL = "https://www.procyclingstats.com/race/" + RaceName + "/" + year + "/" + UrlOfStage;
                                    }
                                }
                                else
                                {
                                    FullStageURL = "https://www.procyclingstats.com/race/" + RaceName + "/" + year + "/" + UrlOfStage;
                                }
                            }
                            else
                            {
                                FullStageURL = "https://www.procyclingstats.com/race/" + RaceName + "/" + year + "/" + UrlOfStage;
                            }

                            var StageURLClient = new HttpClient();
                            var StageURLHtml = await StageURLClient.GetStringAsync(FullStageURL);
                            var StageURLDocument = new HtmlDocument();
                            StageURLDocument.LoadHtml(StageURLHtml);

                            bool ttchecker = false;
                            bool TTTchecker = false;

                            var TTchecker = StageURLDocument.DocumentNode.Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("blue")).ToList();

                            var ProfileScore = StageURLDocument.DocumentNode.Descendants("a")
                                .Where(node => node.GetAttributeValue("href", "")
                                .Contains("profile-score")).ToList();

                            var LengthOfStage = StageURLDocument.DocumentNode.Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("red distance")).ToList();


                            string FinalStageLength = LengthOfStage[0].InnerText;
                            string RegexFinalStageLength = Regex.Replace(FinalStageLength, @"[^0-9\.]+", "");
                            float FloatFinalStageLength = float.Parse(RegexFinalStageLength);

                            if (TTchecker[0].InnerText.Contains("rologue"))
                            {
                                ttchecker = true;
                            }
                            if (TTchecker[0].InnerText.Contains("TT"))
                            {
                                ttchecker = true;
                            }
                            if (TTchecker[0].InnerText.Contains("TTT"))
                            {
                                TTTchecker = true;
                            }

                            List<TTTformat> TTTStageResult = new List<TTTformat>();
                            date stagedate = new date();

                            foreach (var stage in OverviewOfStages)
                            {
                                if (stage.InnerText.Contains("Stage " + stageCounter))
                                {
                                    string parser = stage.InnerText;
                                    string daystring = parser.Substring(0, 2);
                                    int day = int.Parse(daystring);
                                    string monthstring = parser.Substring(3, 2);
                                    int month = int.Parse(monthstring);

                                    stagedate.Day = day;
                                    stagedate.Month = month;
                                    stagedate.Year = year;
                                }
                            }

                            string NameOfStage = PCTStageRaceNames[RaceIterator] + " " + UrlOfStage;
                            string unparsedScore = ProfileScore[0].InnerText;
                            string DifficultyRegex = Regex.Replace(unparsedScore, @"[^0-9\.]+", "");
                            int DifficultyOfStage = int.Parse(DifficultyRegex);

                            string racetype = "Default";

                            switch (DifficultyOfStage)
                            {
                                case var _ when DifficultyOfStage <= 20:
                                    racetype = "Sprinters Race";
                                    break;

                                case var _ when DifficultyOfStage > 20 && DifficultyOfStage <= 50:
                                    racetype = "Medium Hilly Race";
                                    break;

                                case var _ when DifficultyOfStage > 50 && DifficultyOfStage <= 125:
                                    racetype = "Hard Hilly Race";
                                    break;

                                case var _ when DifficultyOfStage > 125 && DifficultyOfStage <= 225:
                                    racetype = "Medium Mountain Race";
                                    break;

                                case var _ when DifficultyOfStage > 225:
                                    racetype = "Hardest Mountain Race/High-Cols";
                                    break;


                            }

                            if (ttchecker == true)
                            {
                                racetype = "Time Trial";
                            }
                            if (TTTchecker == true)
                            {
                                var TTTParser = StageURLDocument.DocumentNode.Descendants("div")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Contains("ttt")).ToList();

                                string[] TeamNames = new string[TTTParser.Count];
                                string[] Time = new string[TTTParser.Count];
                                string[] Splits = new string[TTTParser.Count];
                                float[] Speed = new float[TTTParser.Count];
                                List<Teamresult> ListofTTRidersPerTeam = new List<Teamresult>();

                                for (int TTTCounter = 0; TTTCounter < TTTParser.Count; TTTCounter++)
                                {
                                    var Teamname = TTTParser[TTTCounter].Descendants("a")
                                        .Where(node => node.GetAttributeValue("href", "")
                                        .Contains("team")).ToList();

                                    TeamNames[TTTCounter] = Teamname[0].InnerText;

                                    var time = TTTParser[TTTCounter].Descendants("div")
                                        .Where(node => node.GetAttributeValue("class", "")
                                        .Contains("resTTTb")).ToList();

                                    string ParsedTime = time[0].InnerHtml.ToString();

                                    string TeamTime = ParsedTime.Split(new string[] { "<span>" }, 7, StringSplitOptions.None)[3];
                                    string TimeSplits = ParsedTime.Split(new string[] { "<span>" }, 7, StringSplitOptions.None)[4];
                                    string SpeedofResult = ParsedTime.Split(new string[] { "<span>" }, 7, StringSplitOptions.None)[5];

                                    string RegexTeamTime = Regex.Replace(TeamTime, "[^0-9:]", "");
                                    string RegexTimeSplits = Regex.Replace(TimeSplits, "[^0-9:]", "");
                                    string RegexSpeed = Regex.Replace(SpeedofResult, "[^0-9:]", "");
                                    float SpeedInFloat;

                                    try
                                    {
                                        SpeedInFloat = float.Parse(RegexSpeed);
                                    }
                                    catch (Exception e)
                                    {
                                        SpeedInFloat = 0;
                                    }

                                    Time[TTTCounter] = RegexTeamTime;
                                    Splits[TTTCounter] = RegexTimeSplits;
                                    Speed[TTTCounter] = SpeedInFloat;

                                    var IndividualRiderData = TTTParser[TTTCounter].Descendants("div")
                                        .Where(node => node.GetAttributeValue("class", "")
                                        .Contains("res_line")).ToList();

                                    List<TTTriders> RidersForTTTTeam = new List<TTTriders>();

                                    foreach (var rider in IndividualRiderData)
                                    {
                                        var NameOfRiderInTTT = rider.Descendants("a")
                                            .Where(node => node.GetAttributeValue("href", "")
                                            .Contains("rider")).ToList();

                                        string RiderFullName = NameOfRiderInTTT[0].InnerText;
                                        TTTriders RiderInTeam = new TTTriders();
                                        RiderInTeam.Ridername = RiderFullName;
                                        RidersForTTTTeam.Add(RiderInTeam);
                                    }

                                    Teamresult ResultOfTeamTTT = new Teamresult();
                                    ResultOfTeamTTT.RidersinTT = RidersForTTTTeam;
                                    ListofTTRidersPerTeam.Add(ResultOfTeamTTT);

                                }

                                for (int TTTStreamer = 0; TTTStreamer < TeamNames.Length; TTTStreamer++)
                                {
                                    TTTformat FinalResultsOfTTT = new TTTformat(TeamNames[TTTStreamer], Time[TTTStreamer], Splits[TTTStreamer], Speed[TTTStreamer], ListofTTRidersPerTeam[TTTStreamer]);
                                    TTTStageResult.Add(FinalResultsOfTTT);
                                }

                                StagesInPCTStageRace.Add(new stages(NameOfStage, "2.Pro", racetype, year, TTTStageResult, stagedate));
                            }
                            else
                            {
                                var PCTStageResultsScraper = StageURLDocument.DocumentNode.Descendants("table")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Contains("basic results")).ToList();

                                if (PCTStageResultsScraper.Count == 0)
                                {
                                    Console.ReadLine();
                                    continue;
                                }

                                var PCTStageRaceStageRiders = PCTStageResultsScraper[0].Descendants("a")
                                    .Where(node => node.GetAttributeValue("href", "")
                                    .Contains("rider")).ToList();

                                var PCTStageRaceStageRankings = PCTStageResultsScraper[0].Descendants("tr")
                                    .Where(node => node.GetAttributeValue("data-id", "")
                                    .Contains("")).ToList();

                                var PCTStageRaceStageTime = PCTStageResultsScraper[0].Descendants("span")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Equals("timelag")).ToList();

                                var PCTStageRacestageTeam = PCTStageResultsScraper[0].Descendants("td")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Contains("cu600")).ToList();

                                string[] PlaceHolderForStageRankings = new string[PCTStageRaceStageRankings.Count - 1];
                                string[] FinalResultParsed = new string[PCTStageRaceStageRankings.Count - 1];
                                string[] ResultsRiderName = new string[PCTStageRaceStageRankings.Count - 1];
                                string[] ResultsTiming = new string[PCTStageRaceStageRankings.Count - 1];
                                string[] ResultsriderTeam = new string[PCTStageRaceStageRankings.Count - 1];

                                for (int x = 0; x < PCTStageRaceStageRankings.Count - 1; x++)
                                {
                                    ResultsRiderName[x] = PCTStageRaceStageRiders[x].InnerText;
                                    try
                                    {
                                        ResultsTiming[x] = PCTStageRaceStageTime[x].InnerText;
                                    }
                                    catch (Exception e)
                                    {
                                        ResultsTiming[x] = "0";
                                    }
                                    ResultsriderTeam[x] = PCTStageRacestageTeam[x].InnerText;
                                }

                                for (int ResultsParserInnerHTML = 1; ResultsParserInnerHTML < PCTStageRaceStageRankings.Count; ResultsParserInnerHTML++)
                                {
                                    PlaceHolderForStageRankings[ResultsParserInnerHTML - 1] = PCTStageRaceStageRankings[ResultsParserInnerHTML].InnerHtml;
                                }
                                for (int ResultsParsingLoop = 0; ResultsParsingLoop < PlaceHolderForStageRankings.Length; ResultsParsingLoop++)
                                {
                                    string FullResult = PlaceHolderForStageRankings[ResultsParsingLoop].Substring(0, 7);
                                    string FullParsedResult = "";

                                    for (int characteriterator = 0; characteriterator < 7; characteriterator++)
                                    {
                                        if (char.IsDigit(FullResult[characteriterator]))
                                        {
                                            FullParsedResult = FullParsedResult + FullResult[characteriterator];
                                        }
                                    }

                                    if (FullResult.Contains("OTL"))
                                    {
                                        FullParsedResult = "OTL";
                                    }
                                    if (FullResult.Contains("DNF"))
                                    {
                                        FullParsedResult = "DNF";
                                    }
                                    FinalResultParsed[ResultsParsingLoop] = FullParsedResult;
                                }
                                List<raceresults> StageResults = new List<raceresults>();

                                for (int StageResultsStore = 0; StageResultsStore < ResultsRiderName.Length; StageResultsStore++)
                                {
                                    StageResults.Add(new raceresults(FinalResultParsed[StageResultsStore], ResultsRiderName[StageResultsStore], ResultsTiming[StageResultsStore], ResultsriderTeam[StageResultsStore]));
                                }

                                StagesInPCTStageRace.Add(new stages(NameOfStage, "2.Pro", racetype, year, StageResults, stagedate, FloatFinalStageLength, DifficultyOfStage, 0));
                                if (TourOfBritain == true)
                                {
                                    stageCounter = stageCounter - 1;
                                }
                                if (TOBComplete == true)
                                {
                                    stageCounter = stageCounter + 1;
                                }
                            }

                        }
                        //GC here 
                        List<raceresults> GCResults = new List<raceresults>();
                        var GCParser = "https://www.procyclingstats.com/race/" + RaceName + "/" + year + "/gc";
                        var GCHTTPClient = new HttpClient();
                        var HTMLofGC = await GCHTTPClient.GetStringAsync(GCParser);
                        var GCHTMLinDocument = new HtmlDocument();
                        GCHTMLinDocument.LoadHtml(HTMLofGC);

                        var GCResultsScrape = GCHTMLinDocument.DocumentNode.Descendants("table")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("basic results")).ToList();

                        var GCResultsSeperated = GCResultsScrape[1];

                        if (year == 2012 && RaceName.Contains("tour-de-langkawi") || year == 2016 && RaceName.Contains("qinghai"))
                        {
                            Generalclassification PCTStageRaceGCLang = new Generalclassification();
                            Stageraces PCTStageRaceLangKawi = new Stageraces(StagesInPCTStageRace, PCTStageRaceGCLang);
                            Singleton_Class.ListofPCTStageRaces.Add(PCTStageRaceLangKawi);
                            continue;
                        }

                        if (year == 2012 && RaceName.Contains("oman"))
                        {
                            GCResultsSeperated = GCResultsScrape[2];

                        }

                        var GCRiders = GCResultsSeperated.Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("rider")).ToList();

                        var GCTeams = GCResultsSeperated.Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("team") || node.GetAttributeValue("href", "")
                            .Contains("nation")).ToList();

                        string[] GCRiderName = new string[GCRiders.Count / 2];
                        string[] GCRiderTime = new string[GCRiders.Count / 2];
                        string[] GCTeam = new string[GCRiders.Count / 2];
                        string[] GCPosition = new string[GCRiders.Count / 2];

                        int FirstArrayCounter = 0;
                        int SecondArrayCounter = 0;

                        for (int SortingIterator = 0; SortingIterator < GCRiders.Count; SortingIterator++)
                        {
                            int remainder = SortingIterator % 2;
                            if (remainder == 0)
                            {
                                GCRiderName[FirstArrayCounter] = GCRiders[SortingIterator].InnerText;
                                FirstArrayCounter = FirstArrayCounter + 1;
                            }
                            else
                            {
                                GCRiderTime[SecondArrayCounter] = GCRiders[SortingIterator].InnerText;
                                SecondArrayCounter = SecondArrayCounter + 1;
                            }
                        }

                        for (int TeamIterator = 0; TeamIterator < GCRiders.Count / 2; TeamIterator++)
                        {
                            try
                            {
                                GCTeam[TeamIterator] = GCTeams[TeamIterator].InnerText;
                            }
                            catch (Exception DenmarkGCFormatting)
                            {
                                GCTeam[TeamIterator] = "Team Postnord Danmark";
                            }
                            int finalposition = TeamIterator + 1;
                            GCPosition[TeamIterator] = finalposition.ToString();
                        }

                        for (int GCResultsIterator = 0; GCResultsIterator < GCRiders.Count / 2; GCResultsIterator++)
                        {
                            GCResults.Add(new raceresults(GCPosition[GCResultsIterator], GCRiderName[GCResultsIterator], GCRiderTime[GCResultsIterator], GCTeam[GCResultsIterator]));
                        }
                        Generalclassification PCTStageRaceGC = new Generalclassification(true, GCResults);

                        Stageraces PCTStageRace = new Stageraces(StagesInPCTStageRace, PCTStageRaceGC);
                        Singleton_Class.ListofPCTStageRaces.Add(PCTStageRace);

                    }
                    Console.ReadLine();
                }
                float maxlength = Singleton_Class.ListofPCTStageRaces[0].Stageresults[0].Racelength;
                float minlength = Singleton_Class.ListofPCTStageRaces[0].Stageresults[0].Racelength;

                for (int minmaxchecker = 0; minmaxchecker < Singleton_Class.ListofPCTStageRaces.Count(); minmaxchecker++)
                {
                    foreach (stages stage in Singleton_Class.ListofPCTStageRaces[minmaxchecker].Stageresults)
                    {
                        if (stage.Racelength > maxlength)
                        {
                            maxlength = stage.Racelength;
                        }
                        if (stage.Racelength < minlength)
                        {
                            minlength = stage.Racelength;
                        }
                    }
                }

                float maxdifficulty = Singleton_Class.ListofPCTStageRaces[0].Stageresults[0].Racedifficulty;
                float mindifficulty = Singleton_Class.ListofPCTStageRaces[0].Stageresults[0].Racedifficulty;

                for (int minmaxdifficulty = 0; minmaxdifficulty < Singleton_Class.ListofPCTStageRaces.Count(); minmaxdifficulty++)
                {
                    foreach (stages stage in Singleton_Class.ListofPCTStageRaces[minmaxdifficulty].Stageresults)
                    {
                        if (stage.Racedifficulty > maxdifficulty)
                        {
                            maxdifficulty = stage.Racedifficulty;
                        }
                        if (stage.Racedifficulty < mindifficulty)
                        {
                            mindifficulty = stage.Racedifficulty;
                        }
                    }
                }

                float stagelengthnormalized(float lengthofstage)
                {
                    float normalizedstagelength = ((lengthofstage - minlength) / (maxlength - minlength));
                    return normalizedstagelength;
                }

                float stagedifficultynormalized(float difficultyofstage)
                {
                    float normalizeddifficulty = ((difficultyofstage - mindifficulty) / (maxdifficulty - mindifficulty));
                    return normalizeddifficulty;
                }

                for (int normalizediterator = 0; normalizediterator < Singleton_Class.ListofPCTStageRaces.Count(); normalizediterator++)
                {
                    foreach (stages stage in Singleton_Class.ListofPCTStageRaces[normalizediterator].Stageresults)
                    {
                        float racelengthdifficulty = stage.Racelength;
                        stage.Lengthdifficulty = stagelengthnormalized(racelengthdifficulty);

                        float normalizedclimbingdifficulty = stage.Racedifficulty;
                        stage.Climbdifficultynormalized = stagedifficultynormalized(normalizedclimbingdifficulty);
                    }
                }

                using (TextWriter PCTGCTextWriter = new StreamWriter(@"C:\Users\Wenqing Huang\c#\PCTStageRaces.xml"))
                {
                    XmlSerializer PCTStageRaceSerializer = new XmlSerializer(typeof(List<Stageraces>));
                    PCTStageRaceSerializer.Serialize(PCTGCTextWriter, Singleton_Class.ListofPCTStageRaces);

                }

                filechecker = true;
            }
            
            if (filechecker == true)
            {
                XmlSerializer PCTStageRaceLoader = new XmlSerializer(typeof(List<Stageraces>));
                using (FileStream FileStreamer = File.OpenRead(@"C:\Users\Wenqing Huang\c#\PCTStageRaces.xml"))
                {
                    Singleton_Class.ListofPCTStageRaces = (List<Stageraces>)PCTStageRaceLoader.Deserialize(FileStreamer);
                }
            }
            ContiRaceDownloader.ContiOneDayRaceDownloader();
            DictionaryResultsMatcher.resultsmatcher();
        }
    }
}



