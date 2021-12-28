using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using K180211_Q4.Models;


namespace K180211_Q4.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        static string combinedxmlpath = ConfigurationManager.AppSettings["XMLPath"];
        static string q3path = ConfigurationManager.AppSettings["Q3_output"];
        static string candidates = ConfigurationManager.AppSettings["Candidates"];
        static void CombineCommonFiles(string[] files)
        {

            for (int i = 1; i < files.Length; i++)
            {
                var xml1 = XDocument.Load(files[0]);
                var xml2 = XDocument.Load(files[i]);
                xml1.Descendants("Vote").LastOrDefault().AddAfterSelf(xml2.Descendants("Vote"));
                xml1.Save(combinedxmlpath);
            }

        }
        public ActionResult Index()
        {
            try
            {
                string[] files = Directory.GetFiles(q3path, "AA_Elec_*", System.IO.SearchOption.AllDirectories);

                CombineCommonFiles(files);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            List<VotersModel> voters = new List<VotersModel>();


            XmlDocument doc = new XmlDocument();
            doc.Load(combinedxmlpath);

            //Loop through the selected Nodes.
            foreach (XmlNode node in doc.SelectNodes("/Votes/Vote"))
            {
                //Fetch the Node values and assign it to Model
                voters.Add(new VotersModel
                {
                    Nic = node["Nic"].InnerText,
                    Position = node["Position"].InnerText,
                    CandidateId = node["CandidateID"].InnerText
                });

            }


            List<Candidates> candidate_obj = new List<Candidates>();
            using (StreamReader reader = new StreamReader(candidates))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] splitted = line.Split(',');
                    var candrollno = splitted[0];
                    var candname = splitted[1];
                    var candposition = splitted[2];
                    candidate_obj.Add(new Candidates(candname.Trim(), candrollno, candposition.Trim()));
                }
            }

            
            var candidates_dict = new Dictionary<string, string>(); // stores distinct ids with their associated names
            for (int i = 0; i < candidate_obj.Count; i++)
            {
                if (candidates_dict.ContainsKey(candidate_obj[i].CandidateId))
                {
                    continue;
                }
                else
                {
                    candidates_dict[candidate_obj[i].CandidateId] = candidate_obj[i].Name;
                }
            }

            List<string> distinct_candidate_ids = new List<string>(); //stores distinct ids

            foreach (KeyValuePair<string, string> kvp in candidates_dict)
            {
                distinct_candidate_ids.Add(kvp.Key);
            }




            List<List<Results>> view_results = new List<List<Results>>();

            
            List<Results> president = new List<Results>();
            List<Results> vicepresident = new List<Results>();
            List<Results> generalsec = new List<Results>();

            //runs loop for each candidate id and finds the count of each of their position votes
            for (int j = 0; j < distinct_candidate_ids.Count; j++)
            {
                //add loop of different roll numbers
                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
             
                for (int i = 0; i < voters.Count; i++)
                {

                    if (voters[i].CandidateId == distinct_candidate_ids[j] && voters[i].Position == "PRES")
                    {
                        count1 += 1;
                    }
                    if (voters[i].CandidateId == distinct_candidate_ids[j] && voters[i].Position == "VPRE")
                    {
                        count2 += 1;
                    }
                    if (voters[i].CandidateId == distinct_candidate_ids[j] && voters[i].Position == "GSEC")
                    {
                        count3 += 1;
                    }
                }
                if (count1 != 0)
                {
                    president.Add(new Results(candidates_dict[distinct_candidate_ids[j]], count1));
                }
                if (count2 != 0)
                {
                    vicepresident.Add(new Results(candidates_dict[distinct_candidate_ids[j]], count2));
                }
                if (count3 != 0)
                {
                    generalsec.Add(new Results(candidates_dict[distinct_candidate_ids[j]], count3));
                }

            }

            ViewModels allmodels = new ViewModels();
            allmodels.President = president;
            allmodels.VicePresident = vicepresident;
            allmodels.GeneralSecretary = generalsec;
            return View(allmodels);

        }

    }
}
