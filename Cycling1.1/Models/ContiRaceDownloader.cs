using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Cycling1._1
{
    class ContiRaceDownloader
    {
        static string CheckForNoneFinish(string result)
        {
            string ProcessedResult = result;
            if (ProcessedResult.Contains("OTL"))
            {
                ProcessedResult = "OTL";
            }
            if (ProcessedResult.Contains("DNF"))
            {
                ProcessedResult = "DNF";
            }
            return ProcessedResult;
        }

        string RaceTypeCalculator 


        static async Task<HtmlDocument> HTTPDownloader(string url)
        {
            var CTListofOneDayRaces = url;
            var RaceClient = new HttpClient();
            var RaceHTMLFormat = await RaceClient.GetStringAsync(CTListofOneDayRaces);
            var document = new HtmlDocument();
            document.LoadHtml(RaceHTMLFormat);
            return document;
        }

        static List<HtmlNode> DocumentScraperNode(string FirstIdentifier, string SecondIdentifier, string ThirdIdentifier, HtmlDocument document)
        {
            var ListDocuments = document.DocumentNode.Descendants(FirstIdentifier)
                .Where(node => node.GetAttributeValue(SecondIdentifier, "")
                .Contains(ThirdIdentifier)).ToList();

            return ListDocuments;
        }

        static List<HtmlNode> DocumentScraperIndex(string FirstIdentifier, string SecondIdentifier, string ThirdIdentifier, List<HtmlNode> documentNode, int index)
        {
            var ListDocument = documentNode[index].Descendants(FirstIdentifier)
                .Where(node => node.GetAttributeValue(SecondIdentifier, "")
                .Contains(ThirdIdentifier)).ToList();

            return ListDocument;
        }

        static List<HtmlNode> DocumentScraperEquals(string FirstIdentifier, string SecondIdentifier, string ThirdIdentifier, List<HtmlNode> documentNode, int index)
        {
            var ListDocument = documentNode[index].Descendants(FirstIdentifier)
                .Where(node => node.GetAttributeValue(SecondIdentifier, "")
                .Equals(ThirdIdentifier)).ToList();

            return ListDocument;
        }
        
        
        public static async void ContiOneDayRaceDownloader()
        {
            
            var _RacesInDocument = await HTTPDownloader("https://www.procyclingstats.com/races.php?name=&pname=contains&nation=&class=1.1&category=1&filter=Filter&s=races-database");

            var _ListOfCTRaces = DocumentScraperNode("table", "class", "basic", _RacesInDocument);

            var _CTRaceNameList = DocumentScraperIndex("a", "href", "race", _ListOfCTRaces, 0);

            int counter = 0;
            string[] CTOneDayRaceNames = new string[_CTRaceNameList.Count];
            string[] CTOneDayRaceStatus = new string[_CTRaceNameList.Count];

            foreach (var race in _CTRaceNameList)
            {
                CTOneDayRaceNames[counter] = race.InnerText.ToString();
                CTOneDayRaceStatus[counter] = "1.1";
                counter = counter + 1;
            }

            var DocumentURL = _ListOfCTRaces[0].InnerHtml.ToString();
            var reg = new Regex("\".*?\"");
            List<string> URLsOfRaces = new List<String>();
            var matches = reg.Matches(DocumentURL);

            foreach( var match in matches )
            {
               if (match.ToString().Contains("race"))
                {
                    string url = match.ToString().Split(new string[] { "race/", "/2" }, 3, StringSplitOptions.None)[1];
                    URLsOfRaces.Add(url);
                }
            }

            for (int RaceURLIterator = 0; RaceURLIterator < URLsOfRaces.Count; RaceURLIterator++)
            {
                for (int year = 2018; year < 2021; year++)
                {
                    string race = URLsOfRaces[RaceURLIterator];

                    var RaceInDocument = await HTTPDownloader("https://www.procyclingstats.com/race/" + race + "/" + year);
                    var RaceInDocumentOverview = await HTTPDownloader("https://www.procyclingstats.com/race/" + race + "/" + year + "/" + "overview");

                    List<HtmlNode> CTOneDayRaceDateSection;
                    List<HtmlNode> CTOneDayLengthInitial;
                    List<HtmlNode> CTOneDayLengthParsed;

                    try
                    {
                       CTOneDayRaceDateSection = DocumentScraperNode("ul", "class", "infolist fs13", RaceInDocumentOverview);
                       CTOneDayLengthInitial = DocumentScraperNode("div", "class", "sub", RaceInDocumentOverview);
                       CTOneDayLengthParsed = DocumentScraperIndex("span", "class", "red fw400", CTOneDayLengthInitial, 0);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    Console.ReadLine();
                    string CTLengthOfRace = Regex.Replace(CTOneDayLengthParsed[0].InnerText.ToString(), @"[^0-9\.]", "");
                    float _CTLengthofRace = float.Parse(CTLengthOfRace);

                    string DateFormatter = CTOneDayRaceDateSection[0].InnerText.ToString().Split(new string[] { "Startdate:", "\n" }, 3, StringSplitOptions.None)[1];

                    int DayOfRace = int.Parse(DateFormatter.Substring(9, 2));
                    int MonthOfRace = int.Parse(DateFormatter.Substring(6, 2));

                    date racedate = new date();
                    racedate.Day = DayOfRace;
                    racedate.Month = MonthOfRace;
                    racedate.Year = year;

                    var CTOneDayRacesParsed = DocumentScraperNode("table", "class", "basic results", RaceInDocument);
                    var CTOneDayRacesRiders = DocumentScraperIndex("a", "href", "rider", CTOneDayRacesParsed, 0);
                    var CTOneDayRaceResultsRankings = DocumentScraperIndex("tr", "data-id", "", CTOneDayRacesParsed, 0);

                    string[] PlaceHolderRankings = new string[CTOneDayRaceResultsRankings.Count - 1];
                    string[] ParsedRankings = new string[CTOneDayRaceResultsRankings.Count - 1];

                    for (int FirstRankingLoop = 1; FirstRankingLoop < CTOneDayRaceResultsRankings.Count; FirstRankingLoop++)
                    {
                        PlaceHolderRankings[FirstRankingLoop - 1] = CTOneDayRaceResultsRankings[FirstRankingLoop].InnerHtml;
                    }

                    for (int SecondRankingLoop = 0; SecondRankingLoop < PlaceHolderRankings.Length; SecondRankingLoop++)
                    {
                        string RankingParsed = PlaceHolderRankings[SecondRankingLoop].Substring(0, 7);
                        string result = "";

                        for(int RankingThirdLoop = 0; RankingThirdLoop < 7; RankingThirdLoop++)
                        {
                            if (char.IsDigit(RankingParsed[RankingThirdLoop]))
                            {
                                result = result + RankingParsed[RankingThirdLoop];
                            }
                        }

                        ParsedRankings[SecondRankingLoop] = CheckForNoneFinish(result);
                    }

                    string[] ParsedCTOneDayRiderNames = new string[CTOneDayRacesRiders.Count];

                    for(int i = 0; i < CTOneDayRacesRiders.Count; i++)
                    {
                        ParsedCTOneDayRiderNames[i] = CTOneDayRacesRiders[i].InnerText.ToString();
                    }

                    var ParsedCTOneDayRaceTime = DocumentScraperEquals("td", "class", "time ar", CTOneDayRacesParsed, 0);
                    var CTOneDayRaceTeam = DocumentScraperIndex("td", "class", "cu600", CTOneDayRacesParsed, 0);

                    string[] CTOneDayRaceTimes = new string[ParsedCTOneDayRiderNames.Length];
                    string[] PositionInRace = new string[ParsedCTOneDayRiderNames.Length];
                    string[] TeamOfRider = new string[ParsedCTOneDayRiderNames.Length];

                    for (int j = 0; j < ParsedCTOneDayRiderNames.Length; j++)
                    {
                        if (j != 0)
                        {
                            string ParseTime = ParsedCTOneDayRaceTime[j].InnerHtml.ToString();

                            string Parsed = ParseTime.Split(new string[] { "<td class=\"time ar\"><span>", "</span>" }, 2, StringSplitOptions.None)[0];
                            string FinalTimeParse = Parsed.Replace("<span>", "");
                            CTOneDayRaceTimes[j] = FinalTimeParse;
                        }
                        else
                        {
                            CTOneDayRaceTimes[j] = ParsedCTOneDayRaceTime[j].InnerText.ToString();
                        }
                        PositionInRace[j] = ParsedRankings[j];
                        TeamOfRider[j] = CTOneDayRaceTeam[j].InnerText.ToString();
                    }

                    List<raceresults> ResultsOfCTRace = new List<raceresults>();

                    for (int k = 0; k < ParsedCTOneDayRiderNames.Length; k++)
                    {
                        ResultsOfCTRace.Add(new raceresults(PositionInRace[k], ParsedCTOneDayRiderNames[k], CTOneDayRaceTimes[k], TeamOfRider[k]));
                    }

                    var ClimbingDifficultyScraped = DocumentScraperNode("a", "href", "profile-score", RaceInDocument);

                    string DifficultyOfClimb = ClimbingDifficultyScraped[0].InnerText.ToString();
                    string RegexDifficulty = Regex.Replace(DifficultyOfClimb, @"[^0-9\.]+", "");
                    int IntegerDifficulty = int.Parse(RegexDifficulty);
                    int CobbledDifficulty = 0; 

                    Console.ReadLine();
                   
                }
            }

        }
    }
}
