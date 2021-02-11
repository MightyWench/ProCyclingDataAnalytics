using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Cycling1._1
{
    class ContiRaceDownloader
    {
        public static async void ContiOneDayRaceDownloader()
        {
            
            var _RaceInDocument = await HTTPDownloader("https://www.procyclingstats.com/races.php?name=&pname=contains&nation=&class=1.1&category=1&filter=Filter&s=races-database");

            var _ListOfCTRaces = DocumentScraperNode("table", "class", "basic", _RaceInDocument);

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

            string[] ArrayOfURLIdentifiers = {"antwerp-port-epic", "gp-lugano",   };

            Console.ReadLine();
            
            
            
            
            
            
            
            
            
            
            async Task<HtmlDocument> HTTPDownloader(string url)
            {
                var CTListofOneDayRaces = url;
                var RaceClient = new HttpClient();
                var RaceHTMLFormat = await RaceClient.GetStringAsync(CTListofOneDayRaces);
                var document = new HtmlDocument();
                document.LoadHtml(RaceHTMLFormat);
                return document;
            }

            List<HtmlNode> DocumentScraperNode(string FirstIdentifier, string SecondIdentifier, string ThirdIdentifier, HtmlDocument document)
            {
                var ListDocuments = document.DocumentNode.Descendants(FirstIdentifier)
                    .Where(node => node.GetAttributeValue(SecondIdentifier, "")
                    .Contains(ThirdIdentifier)).ToList();

                return ListDocuments; 
            }

            List<HtmlNode> DocumentScraperIndex(string FirstIdentifier, string SecondIdentifier, string ThirdIdentifier, List<HtmlNode> documentNode, int index)
            {
                var ListDocument = documentNode[index].Descendants(FirstIdentifier)
                    .Where(node => node.GetAttributeValue(SecondIdentifier, "")
                    .Contains(ThirdIdentifier)).ToList();

                return ListDocument;
            }

        }
    }
}
