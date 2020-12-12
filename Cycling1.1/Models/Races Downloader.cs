using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;

namespace Cycling1._1
{
    class Races_Downloader
    {
        public static async void WTOneDayRacesdownloader()
        {

            bool filechecker = false;


            if (File.Exists(@"C:\Users\Wenqing Huang\c#\WTOneDayRaces.xml"))
            {
                filechecker = true;
            }

            while (filechecker == false)
            {
                var WTOneDayRacesURL = "https://www.procyclingstats.com/races.php?s=races-database&name=&nation=&class=1.UWT&category=1&filter=Filter";

                var WTClient = new HttpClient();
                var WTRacesinHTML = await WTClient.GetStringAsync(WTOneDayRacesURL);
                var WTRacesinDocument = new HtmlDocument();
                WTRacesinDocument.LoadHtml(WTRacesinHTML);

                var WTOnedayRaces = WTRacesinDocument.DocumentNode.Descendants("tr")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals(" ")).ToList();

                int counter = 0;
                string[] WTOneDayRaceNames = new string[WTOnedayRaces.Count];
                string[] WTOneDayStatus = new string[WTOnedayRaces.Count];

                foreach (var race in WTOnedayRaces)
                {
                    var racename = WTOnedayRaces[counter].Descendants("a")
                        .Where(node => node.GetAttributeValue("href", "")
                        .Contains("race")).ToList();

                    string WTOneDayRaceNameString = racename[0].InnerText.ToString();
                    WTOneDayRaceNames[counter] = WTOneDayRaceNameString;


                    WTOneDayStatus[counter] = "1.UWT";

                    counter = counter + 1;

                }



                for (int a = 0; a < 21; a++)
                {
                    for (int year = 2011; year < 2020; year++)
                    {
                        string race = WTOneDayRaceNames[a];
                        string DePanne = race.Replace("AG Driedaagse Brugge-De Panne", "driedaagse-vd-panne");
                        string bretagne = DePanne.Replace("Bretagne Classic - Ouest-France", "bretagne-classic");
                        string cadelrace = bretagne.Replace("Cadel Evans Great Ocean Road Race", "great-ocean-race");
                        string sansebastien = cadelrace.Replace("Clasica Ciclista San Sebastian", "san-sebastian");
                        string dwarsdor = sansebastien.Replace("Dwars door Vlaanderen - A travers la Flandre", "dwars-door-vlaanderen");
                        string e3harelbeke = dwarsdor.Replace("E3 BinckBank Classic", "e3-harelbeke");
                        string hamburg = e3harelbeke.Replace("EuroEyes Cyclassics Hamburg", "cyclassics-hamburg");
                        string GentWevelgem = hamburg.Replace("Gent-Wevelgem In Flanders Fields", "gent-wevelgem");
                        string montreal = GentWevelgem.Replace("Grand Prix Cycliste de Montréal", "gp-montreal");
                        string quebec = montreal.Replace("Grand Prix Cycliste de Quebec", "gp-quebec");
                        string flechewallonne = quebec.Replace("La Flèche Wallonne", "la-fleche-wallone");
                        string omloop = flechewallonne.Replace("Omloop Het Nieuwsblad Elite", "omloop-het-nieuwsblad");
                        string london = omloop.Replace("Prudential RideLondon-Surrey Classic", "ride-london-classic");
                        string ronde = london.Replace("Ronde van Vlaanderen - Tour des Flandres", "ronde-van-vlaanderen");
                        string whitespacereplacer = ronde.Replace(" ", "-");


                        var specificWTOnedayURL = "" + whitespacereplacer + "/" + year;
                        var testingloop = "https://www.procyclingstats.com/race/" + whitespacereplacer;
                        var parsedWTOneDayHTTP = new HttpClient();
                        var parsedWTOneDayHTML = await parsedWTOneDayHTTP.GetStringAsync(specificWTOnedayURL);
                        var parsedWTOneDayinDocument = new HtmlDocument();
                        parsedWTOneDayinDocument.LoadHtml(parsedWTOneDayHTML);

                        if (testingloop == "https://www.procyclingstats.com/race/great-ocean-race")
                        {
                            if (year < 2020)
                            {
                                continue;
                            }
                        }

                        if (testingloop == "https://www.procyclingstats.com/race/Eschborn-Frankfurt")
                        {
                            if (year == 2015)
                            {
                                continue;
                            }
                        }
                        if (testingloop == "https://www.procyclingstats.com/race/ride-london-classic")
                        {
                            if (year == 2012)
                            {
                                continue;
                            }
                        }

                        if (testingloop == "https://www.procyclingstats.com/race/driedaagse-vd-panne")
                        {
                            if (year < 2018)
                            {
                                continue;
                            }
                        }

                        var URLofWTOneDayDates = "https://www.procyclingstats.com/race/" + whitespacereplacer + "/" + year + "/" + "overview";

                        var parsedHTTPWTOneDayDates = new HttpClient();
                        var parsedHTMLWTOneDayDates = await parsedHTTPWTOneDayDates.GetStringAsync(URLofWTOneDayDates);
                        var parsedDocumentWTOneDayDates = new HtmlDocument();
                        parsedDocumentWTOneDayDates.LoadHtml(parsedHTMLWTOneDayDates);

                        var ParsedWTOneDayRaceDateSection = parsedDocumentWTOneDayDates.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("w36 left")).ToList();

                        var parsedOneDayRaceLengthInitial = parsedDocumentWTOneDayDates.DocumentNode.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("entry race")).ToList();

                        var parsedOneDayRaceLength = parsedOneDayRaceLengthInitial[0].Descendants("span")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("red distance")).ToList();

                        string lengthUnformatted = parsedOneDayRaceLength[0].InnerText.ToString();

                        string regexlength = Regex.Replace(lengthUnformatted, @"[^0-9\.]+", "");
                        float lengthofrace = float.Parse(regexlength);

                        string parser = ParsedWTOneDayRaceDateSection[0].InnerText.ToString();
                        string daystring = parser.Substring(18, 2);
                        int day = int.Parse(daystring);
                        string monthstring = parser.Substring(15, 2);
                        int month = int.Parse(monthstring);

                        date racedate = new date();
                        racedate.Day = day;
                        racedate.Month = month;
                        racedate.Year = year;



                        var parsedWTOneDayRaces = parsedWTOneDayinDocument.DocumentNode.Descendants("table")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("basic results")).ToList();



                        var parsedWTOneDayRaceRiders = parsedWTOneDayRaces[0].Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("rider")).ToList();

                        var parsedrankings = parsedWTOneDayRaces[0].Descendants("tr")
                            .Where(node => node.GetAttributeValue("data-id", "")
                            .Contains("")).ToList();

                        string[] placeholderforrankings = new string[parsedrankings.Count - 1];
                        string[] resultsnumbers = new string[parsedrankings.Count - 1];

                        for (int rankingsfirstloop = 1; rankingsfirstloop < parsedrankings.Count; rankingsfirstloop++)
                        {
                            placeholderforrankings[rankingsfirstloop - 1] = parsedrankings[rankingsfirstloop].InnerHtml;
                        }

                        for (int rankingssecondloop = 0; rankingssecondloop < placeholderforrankings.Length; rankingssecondloop++)
                        {
                            string a1 = placeholderforrankings[rankingssecondloop].Substring(0, 7);
                            string result = "";
                            for (int rankingsthirdloop = 0; rankingsthirdloop < 7; rankingsthirdloop++)
                            {
                                if (char.IsDigit(a1[rankingsthirdloop]))
                                {
                                    result = result + a1[rankingsthirdloop];
                                }
                            }

                            if (a1.Contains("OTL"))
                            {
                                result = "OTL";
                            }
                            if (a1.Contains("DNF"))
                            {
                                result = "DNF";

                            }
                            resultsnumbers[rankingssecondloop] = result;
                        }
                            
                        string[] parsedWTOneDayRaceRidersNames = new string[parsedWTOneDayRaceRiders.Count];

                        for (int i = 0; i < parsedWTOneDayRaceRiders.Count; i++)
                        {
                            parsedWTOneDayRaceRidersNames[i] = parsedWTOneDayRaceRiders[i].InnerText.ToString();
                        }

                        var paredWTOneDayRaceTime = parsedWTOneDayRaces[0].Descendants("span")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("timelag")).ToList();

                        var parsedWTOneDayRaceTeam = parsedWTOneDayRaces[0].Descendants("td")
                         .Where(node => node.GetAttributeValue("class", "")
                         .Contains("cu600")).ToList();


                        string[] racetimes = new string[parsedWTOneDayRaceRidersNames.Length];
                        string[] positioninrace = new string[parsedWTOneDayRaceRidersNames.Length];
                        string[] riderteam = new string[parsedWTOneDayRaceRidersNames.Length];

                        for (int j = 0; j < parsedWTOneDayRaceRidersNames.Length; j++)
                        {
                            racetimes[j] = paredWTOneDayRaceTime[j].InnerText.ToString();
                            positioninrace[j] = resultsnumbers[j];
                            riderteam[j] = parsedWTOneDayRaceTeam[j].InnerText.ToString();
                        }

                        List<raceresults> ResultsofRace = new List<raceresults>();

                        for (int k = 0; k < parsedWTOneDayRaceRidersNames.Length; k++)
                        {
                            ResultsofRace.Add(new raceresults(positioninrace[k], parsedWTOneDayRaceRidersNames[k], racetimes[k], riderteam[k]));
                        }

                        var climbdifficulty = parsedWTOneDayinDocument;

                        var difficultyparsed = climbdifficulty.DocumentNode.Descendants("a").
                            Where(node => node.GetAttributeValue("href", "")
                            .Contains("profile-score")).ToList();

                        string difficultyinstring = difficultyparsed[0].InnerText.ToString();

                        string regexdifficulty = Regex.Replace(difficultyinstring, @"[^0-9\.]+", "");

                        int difficultyinteger = int.Parse(regexdifficulty);

                        int cobbledifficulty = 0;

                        switch (race)
                        {
                            case "E3 BinckBank Classic":
                                cobbledifficulty = 65;
                                break;

                            case "Gent-Wevelgem In Flanders Fields":
                                cobbledifficulty = 15;
                                break;

                            case "Omloop Het Nieuwsblad Elite":
                                cobbledifficulty = 40;
                                break;

                            case "Paris-Roubaix":
                                cobbledifficulty = 100;
                                break;

                            case "Ronde van Vlaanderen - Tour des Flandres":
                                cobbledifficulty = 80;
                                break;

                            case "Dwars door Vlaanderen - A travers la Flandre":
                                cobbledifficulty = 50;
                                break;

                            default:
                                cobbledifficulty = 0;
                                break;

                        }

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


                        Singleton_Class.ListofWTOneDayRaces.Add(new Races(race, WTOneDayStatus[a], racetype, year, ResultsofRace, racedate, lengthofrace, difficultyinteger, cobbledifficulty));





                    }
                }

                float maxlength = Singleton_Class.ListofWTOneDayRaces[0].Racelength;
                float minlength = Singleton_Class.ListofWTOneDayRaces[0].Racelength;

                for (int maxminchecker = 0; maxminchecker < Singleton_Class.ListofWTOneDayRaces.Count(); maxminchecker++)
                {
                    if (Singleton_Class.ListofWTOneDayRaces[maxminchecker].Racelength > maxlength)
                    {
                        maxlength = Singleton_Class.ListofWTOneDayRaces[maxminchecker].Racelength;
                    }
                    if (Singleton_Class.ListofWTOneDayRaces[maxminchecker].Racelength < minlength)
                    {
                        minlength = Singleton_Class.ListofWTOneDayRaces[maxminchecker].Racelength;
                    }
                }



                float racelengthnormalized(float racelength)
                {
                    float normalizedlength = ((racelength - minlength) / (maxlength - minlength));
                    return normalizedlength;
                }


                float maxdifficulty = Singleton_Class.ListofWTOneDayRaces[0].Racedifficulty;
                float mindifficulty = Singleton_Class.ListofWTOneDayRaces[0].Racedifficulty;

                for (int difficultychecker = 0; difficultychecker < Singleton_Class.ListofWTOneDayRaces.Count(); difficultychecker++)
                {
                    if (Singleton_Class.ListofWTOneDayRaces[difficultychecker].Racedifficulty > maxdifficulty)
                    {
                        maxdifficulty = Singleton_Class.ListofWTOneDayRaces[difficultychecker].Racedifficulty;
                    }
                    if (Singleton_Class.ListofWTOneDayRaces[difficultychecker].Racedifficulty < mindifficulty)
                    {
                        mindifficulty = Singleton_Class.ListofWTOneDayRaces[difficultychecker].Racedifficulty;
                    }
                }

                float racedifficultynormalized(float racedifficulty)
                {
                    float normalizeddifficulty = ((racedifficulty - mindifficulty) / (maxdifficulty - mindifficulty));
                    return normalizeddifficulty;
                }

                Console.ReadLine();

                foreach (var race in Singleton_Class.ListofWTOneDayRaces)
                {
                    float racedlengthdifficulty = race.Racelength;
                    race.Lengthdifficulty = racelengthnormalized(racedlengthdifficulty);

                    float normalizedclimbdifficulty = race.Racedifficulty;
                    race.Climbdifficultynormalized = racedifficultynormalized(normalizedclimbdifficulty);

                }


                using (TextWriter WToneDayTextWriter = new StreamWriter(@"C:\Users\Wenqing Huang\c#\WTOneDayRaces.xml"))
                {

                    XmlSerializer OnedayWTSerializer = new XmlSerializer(typeof(List<Races>));
                    OnedayWTSerializer.Serialize(WToneDayTextWriter, Singleton_Class.ListofWTOneDayRaces);
                }

                filechecker = true;


            }

            Console.ReadLine();

            if (filechecker == true)
            {
                XmlSerializer OneDayWTLoader = new XmlSerializer(typeof(List<Races>));

                using (FileStream Filestreamer = File.OpenRead(@"C:\Users\Wenqing Huang\c#\WTOneDayRaces.xml"))
                {
                    Singleton_Class.ListofWTOneDayRaces = (List<Races>)OneDayWTLoader.Deserialize(Filestreamer);
                }
            }

            Races_Downloader.WTStageRaceDownloader();


        }

        

        public static async void WTStageRaceDownloader()
        {
            bool checkifGCfileExists = false;

            if (File.Exists(@"C:\Users\Wenqing Huang\c#\WTStageRace.xml"))
            {
                checkifGCfileExists = true;
            }

            while (checkifGCfileExists == false)
            {

                var WTStageRaces = "https://www.procyclingstats.com/races.php?s=races-database&name=&nation=&class=2.UWT&category=1&filter=Filter";

                var WTStageClient = new HttpClient();
                var WTStageRaceinHTML = await WTStageClient.GetStringAsync(WTStageRaces);
                var WTStageRaceinDocument = new HtmlDocument();
                WTStageRaceinDocument.LoadHtml(WTStageRaceinHTML);

                var WTStageRaceNAmesExtraction = WTStageRaceinDocument.DocumentNode.Descendants("a")
                    .Where(node => node.GetAttributeValue("href", "")
                    .Contains("overview")).ToList();

                string[] WTStageRaceNames = new string[14];
                string[] WTStageRaceStatus = new string[14];

                for (int counter = 0; counter < 14; counter++)
                {
                    //var Nameextraction = WTStageRaceNAmesExtraction[counter].Descendants("a").
                    // Where(node => node.GetAttributeValue("href", "")
                    // .Contains("race")).ToList();

                    WTStageRaceNames[counter] = WTStageRaceNAmesExtraction[counter].InnerText;
                    WTStageRaceStatus[counter] = "2.UWT";


                }


                for (int raceiterator = 0; raceiterator < WTStageRaceNames.Length; raceiterator++)
                {
                    for (int year = 2011; year < 2020; year++)
                    {
                        List<stages> stagesInWtStageRace = new List<stages>();
                        string raceurl = WTStageRaceNames[raceiterator];
                        string binckbank = raceurl.Replace("BinckBank Tour", "binckbank-tour");
                        string dauphine = binckbank.Replace("Critérium du Dauphiné", "dauphine");
                        string giro = dauphine.Replace("Giro d'Italia", "giro-d-italia");
                        string guangxi = giro.Replace("Gree-Tour of Guangxi", "tour-of-guangxi");
                        string basque = guangxi.Replace("Itzulia Basque Country", "itzulia-basque-country");
                        string vuelta = basque.Replace("La Vuelta ciclista a España", "vuelta-a-espana");
                        string pn = vuelta.Replace("Paris - Nice", "paris-nice");
                        string tdu = pn.Replace("Santos Tour Down Under", "tour-down-under");
                        string tirreno = tdu.Replace("Tirreno-Adriatico", "tirreno-adriatico");
                        string tour = tirreno.Replace("Tour de France", "tour-de-france");
                        string poland = tour.Replace("Tour de Pologne", "tour-de-pologne");
                        string romandie = poland.Replace("Tour de Romandie", "tour-de-romandie");
                        string swiss = romandie.Replace("Tour de Suisse", "tour-de-suisse");
                        string uae = swiss.Replace("UAE Tour", "uae-tour");
                        string catalan = uae.Replace("Volta Ciclista a Catalunya", "volta-a-catalunya");
                        string cali = catalan.Replace("Amgen Tour Of California", "tour-of-california");

                        var overviewurl = "https://www.procyclingstats.com/race/" + cali + "/" + year + "/overview";

                        var overviewclient = new HttpClient();
                        var overviewHTML = await overviewclient.GetStringAsync(overviewurl);
                        var overviewinDocument = new HtmlDocument();
                        overviewinDocument.LoadHtml(overviewHTML);

                        var OverviewStages = overviewinDocument.DocumentNode.Descendants("li")
                            .Where(node => node.GetAttributeValue("style", "")
                            .Contains("padding:")).ToList();

                        if (OverviewStages.Count == 0)
                        {
                            continue;
                        }

                        if (year == 2015 && cali.Contains("Paris"))
                        {
                            Console.ReadLine();
                        }


                        var checkifprologue = OverviewStages[0].Descendants("span")
                            .Where(node => node.GetAttributeValue("style", "")
                            .Contains("color:")).ToList();

                        string lengthofstage1 = checkifprologue[0].InnerText;

                        string regexstage1length = Regex.Replace(lengthofstage1, @"[^0-9\.]+", "");

                        float stage1Length = float.Parse(regexstage1length);

                        bool prologue = false;

                        if (stage1Length < 8)
                        {
                            prologue = true;
                        }

                        if (cali.Contains("vuelta") && year == 2015)
                        {

                            prologue = false;

                        }

                        if (cali.Contains("suisse"))
                        {
                            if (year == 2016 || year == 2017)
                            {
                                prologue = false;
                            }
                        }

                        bool exteriordauphinebool = false;

                        if (prologue == true)
                        {
                            exteriordauphinebool = true;
                        }


                        int numberofstages = OverviewStages.Count;




                        for (int stageCounter = 1; stageCounter <= numberofstages; stageCounter++)
                        {


                            bool dauphinefuckthisshit = false;
                            string stageurl;

                            if (prologue == true)
                            {
                                stageurl = "prologue";
                                prologue = false;
                                dauphinefuckthisshit = true;



                            }
                            else
                            {
                                stageurl = "stage-" + stageCounter;
                            }

                            var fullstageurl = "";

                            if (cali.Contains("dauphine") || cali.Contains("Paris") || cali.Contains("romandie") && year != 2011 || cali.Contains("france") || cali.Contains("suisse") && year == 2012)
                            {
                                if (exteriordauphinebool == true)
                                {
                                    if (dauphinefuckthisshit == false)
                                    {
                                        int stageinteger = stageCounter - 1;
                                        string stageminus = "stage-" + stageinteger;
                                        fullstageurl = "https://www.procyclingstats.com/race/" + cali + "/" + year + "/" + stageminus;
                                        Console.ReadLine();

                                    }

                                    else
                                    {
                                        fullstageurl = "https://www.procyclingstats.com/race/" + cali + "/" + year + "/" + stageurl;
                                    }
                                }
                                else
                                {
                                    fullstageurl = "https://www.procyclingstats.com/race/" + cali + "/" + year + "/" + stageurl;
                                }
                            }
                            else

                            {
                                fullstageurl = "https://www.procyclingstats.com/race/" + cali + "/" + year + "/" + stageurl;
                            }
                            var OverviewofstageURL = fullstageurl + "/overview";

                            if (fullstageurl.Contains("Paris-Nice/2012/stage-1") && year == 2012)
                            {
                                fullstageurl = "https://www.procyclingstats.com/race/Paris-Nice/2012/prologue";
                            }

                            var stageurlclient = new HttpClient();
                            var stageurlHTML = await stageurlclient.GetStringAsync(fullstageurl);
                            var stageurldocument = new HtmlDocument();
                            stageurldocument.LoadHtml(stageurlHTML);

                            bool ttchecker = false;
                            bool TTTchecker = false;

                            var TTchecker = stageurldocument.DocumentNode.Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("blue")).ToList();

                            var profilescore = stageurldocument.DocumentNode.Descendants("a")
                                .Where(node => node.GetAttributeValue("href", "")
                                .Contains("profile-score")).ToList();

                            var lengthofstage = stageurldocument.DocumentNode.Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("red distance")).ToList();

                            string finalstagelength = lengthofstage[0].InnerText;

                            string regexlengthofstage = Regex.Replace(finalstagelength, @"[^0-9\.]+", "");

                            float formattedstagelength = float.Parse(regexlengthofstage);


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

                            List<TTTformat> TTTstageresult = new List<TTTformat>();

                            date stagedate = new date();

                            foreach (var stages in OverviewStages)
                            {
                                if (stages.InnerText.Contains("Stage " + stageCounter))
                                {
                                    string parser = stages.InnerText.ToString();
                                    string daystring = parser.Substring(0, 2);
                                    int day = int.Parse(daystring);
                                    string monthstring = parser.Substring(3, 2);
                                    int month = int.Parse(monthstring);


                                    stagedate.Day = day;
                                    stagedate.Month = month;
                                    stagedate.Year = year;

                                }
                            }

                            string stagename = raceurl + " " + stageurl;

                            string unparsedscore = profilescore[0].InnerText;

                            string regexdifficulty = Regex.Replace(unparsedscore, @"[^0-9\.]+", "");


                            int difficultyinteger = int.Parse(regexdifficulty);

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

                            if (ttchecker == true)
                            {
                                racetype = "Time Trial";
                            }




                            if (TTTchecker == true)
                            {



                                var TTTparser = stageurldocument.DocumentNode.Descendants("div")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Contains("ttt")).ToList();

                                string[] Teamnames = new string[TTTparser.Count];
                                string[] Time = new string[TTTparser.Count];
                                string[] Splits = new string[TTTparser.Count];
                                float[] Speed = new float[TTTparser.Count];
                                List<Teamresult> ListofTTRidersPerTeam = new List<Teamresult>();


                                for (int TTTcounter = 0; TTTcounter < TTTparser.Count; TTTcounter++)
                                {

                                    var Teamname = TTTparser[TTTcounter].Descendants("a")
                                        .Where(node => node.GetAttributeValue("href", "")
                                        .Contains("team")).ToList();

                                    Teamnames[TTTcounter] = Teamname[0].InnerText.ToString();

                                    var time = TTTparser[TTTcounter].Descendants("div")
                                         .Where(node => node.GetAttributeValue("class", "")
                                         .Contains("resTTTb")).ToList();

                                    string parsedtime = time[0].InnerHtml.ToString();

                                    string teamtime = parsedtime.Split(new string[] { "<span>" }, 7, StringSplitOptions.None)[3];
                                    string splits = parsedtime.Split(new string[] { "<span>" }, 7, StringSplitOptions.None)[4];
                                    string speed = parsedtime.Split(new string[] { "<span>" }, 7, StringSplitOptions.None)[5];

                                    string regexteamtime = Regex.Replace(teamtime, "[^0-9:]", "");
                                    string regexsplits = Regex.Replace(splits, "[^0-9:]", "");
                                    string regexspeed = Regex.Replace(speed, "[^0-9.]", "");


                                    float floatspeed;
                                    try
                                    {
                                        floatspeed = float.Parse(regexspeed);
                                    }
                                    catch (Exception e)
                                    {
                                        floatspeed = 0;
                                    }

                                    Time[TTTcounter] = regexteamtime;
                                    Splits[TTTcounter] = regexsplits;
                                    Speed[TTTcounter] = floatspeed;

                                    var riderdata = TTTparser[TTTcounter].Descendants("div")
                                        .Where(node => node.GetAttributeValue("class", "")
                                        .Contains("res_line")).ToList();

                                    List<TTTriders> ridersforteam = new List<TTTriders>();

                                    foreach (var rider in riderdata)
                                    {
                                        var ridername = rider.Descendants("a")
                                        .Where(node => node.GetAttributeValue("href", "")
                                        .Contains("rider")).ToList();



                                        string riderfullname = ridername[0].InnerText.ToString();

                                        TTTriders riderinteam = new TTTriders();
                                        riderinteam.Ridername = riderfullname;


                                        ridersforteam.Add(riderinteam);

                                    }

                                    Teamresult resultofteam = new Teamresult();
                                    resultofteam.RidersinTT = ridersforteam;

                                    ListofTTRidersPerTeam.Add(resultofteam);




                                }

                                for (int TTTstreamer = 0; TTTstreamer < Teamnames.Length; TTTstreamer++)
                                {
                                    TTTformat finalresultsofTTT = new TTTformat(Teamnames[TTTstreamer], Time[TTTstreamer], Splits[TTTstreamer], Speed[TTTstreamer], ListofTTRidersPerTeam[TTTstreamer]);
                                    TTTstageresult.Add(finalresultsofTTT);

                                }

                                stagesInWtStageRace.Add(new stages(stagename, "2.UWT", racetype, year, TTTstageresult, stagedate));
                                Console.ReadLine();




                            }

                            else
                            {

                                var StageResultScraper = stageurldocument.DocumentNode.Descendants("table")
                               .Where(node => node.GetAttributeValue("class", "")
                               .Contains("basic results")).ToList();

                                if (StageResultScraper.Count == 0)
                                {
                                    Console.ReadLine();
                                    continue;

                                }

                                var WtStageRaceStageRiders = StageResultScraper[0].Descendants("a")
                                    .Where(node => node.GetAttributeValue("href", "")
                                    .Contains("rider")).ToList();

                                var WtStageRaceStageRankings = StageResultScraper[0].Descendants("tr")
                                    .Where(node => node.GetAttributeValue("data-id", "")
                                    .Contains("")).ToList();

                                var WTStageraceStageTime = StageResultScraper[0].Descendants("span")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Equals("timelag")).ToList();

                                var WTStageRaceStageTeam = StageResultScraper[0].Descendants("td")
                                 .Where(node => node.GetAttributeValue("class", "")
                                 .Contains("cu600")).ToList();





                                string[] placeholderforstagerankings = new string[WtStageRaceStageRankings.Count - 1];
                                string[] finalresultparsed = new string[WtStageRaceStageRankings.Count - 1];
                                string[] resultsridername = new string[WtStageRaceStageRankings.Count - 1];
                                string[] resultstiming = new string[WtStageRaceStageRankings.Count - 1];
                                string[] resultsriderteam = new string[WtStageRaceStageRankings.Count - 1];

                                for (int x = 0; x < WtStageRaceStageRankings.Count - 1; x++)
                                {
                                    resultsridername[x] = WtStageRaceStageRiders[x].InnerText;

                                    try
                                    {
                                        resultstiming[x] = WTStageraceStageTime[x].InnerText;
                                    }
                                    catch (Exception e)
                                    {

                                        resultstiming[x] = "0";
                                    }

                                    resultsriderteam[x] = WTStageRaceStageTeam[x].InnerText;
                                }




                                for (int Resultsparserinnerhtml = 1; Resultsparserinnerhtml < WtStageRaceStageRankings.Count; Resultsparserinnerhtml++)
                                {
                                    placeholderforstagerankings[Resultsparserinnerhtml - 1] = WtStageRaceStageRankings[Resultsparserinnerhtml].InnerHtml;
                                }

                                for (int resultsparsingloop = 0; resultsparsingloop < placeholderforstagerankings.Length; resultsparsingloop++)
                                {
                                    string fullresult = placeholderforstagerankings[resultsparsingloop].Substring(0, 7);
                                    string fullparsedresult = "";

                                    for (int characteriterator = 0; characteriterator < 7; characteriterator++)
                                    {
                                        if (char.IsDigit(fullresult[characteriterator]))
                                        {
                                            fullparsedresult = fullparsedresult + fullresult[characteriterator];
                                        }
                                    }

                                    if (fullresult.Contains("OTL"))
                                    {
                                        fullparsedresult = "OTL";
                                    }
                                    if (fullresult.Contains("DNF"))
                                    {
                                        fullparsedresult = "DNF";
                                    }

                                    finalresultparsed[resultsparsingloop] = fullparsedresult;



                                }

                                List<raceresults> stageResults = new List<raceresults>();

                                for (int stageresultstore = 0; stageresultstore < resultsridername.Length; stageresultstore++)
                                {
                                    stageResults.Add(new raceresults(finalresultparsed[stageresultstore], resultsridername[stageresultstore], resultstiming[stageresultstore], resultsriderteam[stageresultstore]));
                                }

                                stagesInWtStageRace.Add(new stages(stagename, "2.UWT", racetype, year, stageResults, stagedate, formattedstagelength, difficultyinteger, 0));

                            }

                        }

                        //GC goes here 
                        List<raceresults> GCresults = new List<raceresults>();
                        var GCparser = "https://www.procyclingstats.com/race/" + cali + "/" + year + "/gc";
                        var GCHTTPClient = new HttpClient();
                        var HTMLofGC = await GCHTTPClient.GetStringAsync(GCparser);
                        var GCHtmlInDocument = new HtmlDocument();
                        GCHtmlInDocument.LoadHtml(HTMLofGC);

                        var GCResults = GCHtmlInDocument.DocumentNode.Descendants("table")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("basic results")).ToList();

                        var GCResultsSeperated = GCResults[1];

                        var GCRiders = GCResultsSeperated.Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("rider")).ToList();

                        var GCTeams = GCResultsSeperated.Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("team") || node.GetAttributeValue("href", "")
                            .Contains("nation")).ToList();

                        var GCTeamsRomandie2019 = GCResultsSeperated.Descendants("td")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("cu600")).ToList();

                        string[] GCridername = new string[GCRiders.Count / 2];
                        string[] ridertime = new string[GCRiders.Count / 2];
                        string[] GCTeam = new string[GCRiders.Count / 2];
                        string[] GCPosition = new string[GCRiders.Count / 2];

                        int firstarraycounter = 0;
                        int secondarraycounter = 0;

                        for (int sortingiterator = 0; sortingiterator < GCRiders.Count; sortingiterator++)
                        {
                            int remainder = sortingiterator % 2;
                            if (remainder == 0)
                            {
                                GCridername[firstarraycounter] = GCRiders[sortingiterator].InnerText;
                                firstarraycounter = firstarraycounter + 1;
                            }
                            else
                            {
                                ridertime[secondarraycounter] = GCRiders[sortingiterator].InnerText;
                                secondarraycounter = secondarraycounter + 1;
                            }


                        }

                        for (int teamiterator = 0; teamiterator < GCRiders.Count / 2; teamiterator++)
                        {
                            if (GCparser.Contains("romandie") && year == 2019)
                            {
                                GCTeam[teamiterator] = GCTeamsRomandie2019[teamiterator].InnerText;
                            }
                            else
                            {
                                GCTeam[teamiterator] = GCTeams[teamiterator].InnerText;
                            }
                            int finalposition = teamiterator + 1;
                            GCPosition[teamiterator] = finalposition.ToString();
                        }

                        for (int GCresultsiterator = 0; GCresultsiterator < GCRiders.Count / 2; GCresultsiterator++)
                        {
                            GCresults.Add(new raceresults(GCPosition[GCresultsiterator], GCridername[GCresultsiterator], ridertime[GCresultsiterator], GCTeam[GCresultsiterator]));
                        }

                        Generalclassification WTStageRaceGC = new Generalclassification(true, GCresults);


                        //entireracehere; 
                        Stageraces WTStagerace = new Stageraces(stagesInWtStageRace, WTStageRaceGC);
                        Singleton_Class.ListofStageRaces.Add(WTStagerace);

                    }

                    Console.ReadLine();
                }
                //lengthformater goes here 

                float maxlength = Singleton_Class.ListofStageRaces[0].Stageresults[0].Racelength;
                float minlength = Singleton_Class.ListofStageRaces[0].Stageresults[0].Racelength;

                for (int minmaxchecker = 0; minmaxchecker < Singleton_Class.ListofStageRaces.Count(); minmaxchecker++)
                {
                    foreach (stages stage in Singleton_Class.ListofStageRaces[minmaxchecker].Stageresults)
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

                float maxdifficulty = Singleton_Class.ListofStageRaces[0].Stageresults[0].Racedifficulty;
                float mindifficulty = Singleton_Class.ListofStageRaces[0].Stageresults[0].Racedifficulty;

                for (int minmaxdifficultychecker = 0; minmaxdifficultychecker < Singleton_Class.ListofStageRaces.Count(); minmaxdifficultychecker++)
                {
                    foreach (stages stage in Singleton_Class.ListofStageRaces[minmaxdifficultychecker].Stageresults)
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

                for (int normalizediterator = 0; normalizediterator < Singleton_Class.ListofStageRaces.Count(); normalizediterator++)
                {
                    foreach (stages stage in Singleton_Class.ListofStageRaces[normalizediterator].Stageresults)
                    {
                        float racelengthdifficulty = stage.Racelength;
                        stage.Lengthdifficulty = stagelengthnormalized(racelengthdifficulty);

                        float normalizedclimbdifficulty = stage.Racedifficulty;
                        stage.Climbdifficultynormalized = stagedifficultynormalized(normalizedclimbdifficulty);
                    }
                }

                using (TextWriter GCRaceTextWriter = new StreamWriter(@"C:\Users\Wenqing Huang\c#\WTStageRace.xml"))
                {
                    XmlSerializer WTStageRaceSerializer = new XmlSerializer(typeof(List<Stageraces>));
                    WTStageRaceSerializer.Serialize(GCRaceTextWriter, Singleton_Class.ListofStageRaces);
                }

                checkifGCfileExists = true;

            }
           
            if (checkifGCfileExists == true)
            {
                XmlSerializer WTGCRaceLoader = new XmlSerializer(typeof(List<Stageraces>));

                using (FileStream Filestreamer = File.OpenRead(@"C:\Users\Wenqing Huang\c#\WTStageRace.xml"))
                {
                    Singleton_Class.ListofStageRaces = (List<Stageraces>)WTGCRaceLoader.Deserialize(Filestreamer);
                }
            }

            PCTDownloader.PCTOneDayRacesDownloader();

          

            
        }
    }


}
