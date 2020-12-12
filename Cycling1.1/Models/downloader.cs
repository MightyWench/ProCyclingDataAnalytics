using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;

namespace Cycling1._1
{
    class databaseimport
    {
       
            
        
        public static async void Teamslistdownloader()
        {
            bool teamsboolean = false;


            if (File.Exists(@"C:\Users\Wenqing Huang\c#\CyclingTeams.xml"))
            {
                teamsboolean = true;
            }

            while (teamsboolean == false)
            {
                var teamslist = "https://www.procyclingstats.com/rankings/me/season/teams";
                var downloadclient = new HttpClient();
                var PageinHTML = await downloadclient.GetStringAsync(teamslist);
                var PageinDocument = new HtmlDocument();
                PageinDocument.LoadHtml(PageinHTML);

                var Listofteams = PageinDocument.DocumentNode.Descendants("table")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("basic ")).ToList();

                var Teamnames = Listofteams[0].Descendants("a")
                    .Where(node => node.GetAttributeValue("href", "")
                    .Contains("season=2020")).ToList();

                string Stringofpage = Listofteams[0].InnerHtml.ToString();

                Match[] Teamstatus = Regex.Matches(Stringofpage, @"\bWT|PRT|CT|CLUB\b")
                           .Cast<Match>()
                           .ToArray();

                string[] status = new string[Teamstatus.Length];

                int count = 0;
                foreach (var teams in Teamstatus)
                {
                    string place = Teamstatus[count].ToString();
                    status[count] = place;
                    count = count + 1;

                }

                int stringcounter = 0;
                int i = 0;
                int j = 0;

                string[] ListofTeams = new string[Teamnames.Count / 2];
                int[] TeamPoints = new int[Teamnames.Count / 2];


                foreach (var Team in Teamnames)
                {
                    if (stringcounter % 2 == 0)
                    {
                        string value = Team.InnerHtml;
                        ListofTeams[i] = value;

                        i = i + 1;
                        stringcounter = stringcounter + 1;

                    }

                    else if (stringcounter % 2 != 0)
                    {
                        int points = int.Parse(Team.InnerHtml);
                        TeamPoints[j] = points;
                        j = j + 1;
                        stringcounter = stringcounter + 1;

                    }

                }



                for (i = 0; i < ListofTeams.Length; i++)
                {
                    Singleton_Class.InstancesofTeams.Add(new Teamdetails(ListofTeams[i], TeamPoints[i], status[i]));
                }

                //serializer downloader here
                using (TextWriter CyclingTeams = new StreamWriter(@"C:\Users\Wenqing Huang\c#\CyclingTeams.xml"))
                {
                    XmlSerializer CyclingTeamSerializer = new XmlSerializer(typeof(List<Teamdetails>));
                    CyclingTeamSerializer.Serialize(CyclingTeams, Singleton_Class.InstancesofTeams);

                }

                teamsboolean = true;
            }

            if (teamsboolean == true)
            {
                XmlSerializer TeamsLoader = new XmlSerializer(typeof(List<Teamdetails>));

                using (FileStream TeamLoader = File.OpenRead(@"C:\Users\Wenqing Huang\c#\CyclingTeams.xml"))
                {
                    Singleton_Class.InstancesofTeams = (List<Teamdetails>)TeamsLoader.Deserialize(TeamLoader);
                }
            }
            //serializer uploader here

            await Task.Run(() => ridersdownloader());

           

          

        }

        public static async void ridersdownloader()
        {
            bool ridersverification = false;

            if (File.Exists(@"C:\Users\Wenqing Huang\c#\CyclingRiders.xml"))
            {
                ridersverification = true;
            }

            while (ridersverification == false)
            {
                for (int i = 0; i <= 50; i++)
                {
                    string placeholder = Singleton_Class.InstancesofTeams[i].Teamname;
                    placeholder = placeholder.Replace(" - ", "-");
                    string parsedplace = placeholder.Replace(" ", "-");
                    string cofidisparse = parsedplace.Replace(",", "");
                    string androniparse = cofidisparse.Replace("Sidermec", "bottecchia");
                    string androniparse2 = androniparse.Replace("Giocattoli", "sidermec");
                    string Bbparse = androniparse2.Replace("B&B-Hotels-Vital-Concept-p/b-KTM", "vital-concept-bb-hotels-pb-ktm");
                    string Euskatelparse = Bbparse.Replace("Euskaltel", "Euskatel");
                    string polishteam = Euskatelparse.Replace("Mazowsze-Serce-Polski", "mazowsze-serce");
                    string bardiani = polishteam.Replace("-Faizanè", "");
                    string canyonpbsoren = bardiani.Replace("/", "");
                    string tartu = canyonpbsoren.Replace("Tartu-2024-Balticchaincycling.com", "tartu-2024-balticchaincycling-com");
                    string giotti = tartu.Replace("Giotti-Victoria", "giotti-victoria-automotive");
                    string porto = giotti.Replace("W52--FC-Porto-", "w52-fc-porto");
                    string jumbodevo = porto.Replace("Jumbo-Visma-Development-Team", "team-jumbo-development-team");


                    var teampage = "https://www.procyclingstats.com/team/" + jumbodevo + "-2020";
                    var downloadclient = new HttpClient();
                    var PageinHTML = await downloadclient.GetStringAsync(teampage);
                    var PageinDocument = new HtmlDocument();
                    PageinDocument.LoadHtml(PageinHTML);

                    var Listofriders = PageinDocument.DocumentNode.Descendants("ul")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("riderlist riders")).ToList();

                    var Ridernames = Listofriders[0].Descendants("li")
                     .Where(node => node.GetAttributeValue("class", "")
                     .Contains("item")).ToList();

                    var victories = PageinDocument.DocumentNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Contains("topstat")).ToList();

                    string ranking = victories[1].InnerText.ToString();
                    int Ranking = int.Parse(ranking);

                    string seasonvictories = victories[0].InnerText.ToString();
                    int Seasonvictories = int.Parse(seasonvictories);



                    Singleton_Class.InstancesofTeams[i].Ranking = Ranking;
                    Singleton_Class.InstancesofTeams[i].Victories = Seasonvictories;

                    string[] fullnames = new string[Ridernames.Count];
                    int[] age = new int[Ridernames.Count];
                    int[] careerpoints = new int[Ridernames.Count];

                    string teamname = parsedplace;

                    for (int j = 0; j < Ridernames.Count; j++)
                    {
                        var Specificname = Ridernames[j].Descendants("a")
                            .Where(node => node.GetAttributeValue("href", "")
                            .Contains("rider")).ToList();

                        string fullname = Specificname[0].InnerText.ToString();
                        fullnames[j] = fullname;

                        var RiderAge = Ridernames[j].Descendants("div")
                             .Where(node => node.GetAttributeValue("class", "")
                             .Contains("age")).ToList();

                        string parser = RiderAge[0].InnerText.ToString();
                        int ages = int.Parse(parser);
                        age[j] = ages;

                        var Careerpoints = Ridernames[j].Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Contains("pnts")).ToList();

                        string pointsparser = Careerpoints[0].InnerText.ToString();
                        int points = int.Parse(pointsparser);
                        careerpoints[j] = points;
                    }

                    for (int k = 0; k < fullnames.Length; k++)
                    {
                        Singleton_Class.ListofRiders.Add(new Riders(fullnames[k], careerpoints[k], age[k], teamname));
                    }


                }

                //serialize here

                using (TextWriter RiderTextWriter = new StreamWriter(@"C:\Users\Wenqing Huang\c#\CyclingRiders.xml"))
                {
                    XmlSerializer RidersSerializer = new XmlSerializer(typeof(List<Riders>));
                    RidersSerializer.Serialize(RiderTextWriter, Singleton_Class.ListofRiders);

                }

                ridersverification = true; 


            }

            //deserialize here 
            if(ridersverification == true)
            {
                XmlSerializer RidersLoader = new XmlSerializer(typeof(List<Riders>));

                using (FileStream LoadingRiders = File.OpenRead(@"C:\Users\Wenqing Huang\c#\CyclingRiders.xml"))
                {
                    Singleton_Class.ListofRiders = (List<Riders>)RidersLoader.Deserialize(LoadingRiders);
                }
            }



            Races_Downloader.WTOneDayRacesdownloader();


        }
          


    }
}
