using System;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace k180211_Q2
{
    public partial class Form3 : Form
    {
        static string CandidatesPath = ConfigurationManager.AppSettings["CandidatesListPath"];
        static string RecordsPath = ConfigurationManager.AppSettings["RecordsPath"];
        static string VotersVotedPath = ConfigurationManager.AppSettings["VotersVotedPath"];
     

        public Form3()
        {
            InitializeComponent();

        }


        List<Candidate> candidate_obj = new List<Candidate>();

        private void Form3_Load(object sender, EventArgs e)
        {
           
            using (StreamReader reader = new StreamReader(CandidatesPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] splitted = line.Split(',');
                    var candname = splitted[1];
                    var candrollno = splitted[0];
                    var candposition = splitted[2];
                    candidate_obj.Add(new Candidate { Name = candname.Trim(), Rollno = candrollno, Position = candposition.Trim() });
               

                    if (candposition.Trim() == "President")
                    {
                        comboBox1.Items.Add(candname.Trim());
                    }
                    else if (candposition.Trim() == "Vice President")
                    {
                        comboBox2.Items.Add(candname.Trim());
                    }
                    else if (candposition.Trim() == "General Secretary")
                    {
                        comboBox3.Items.Add(candname.Trim());
                    }
                    label5.Text = Voter.Name;



                }
            
            }
         

        }

        private string GetFileFormat()
        {
            DateTime dt = DateTime.Now;

            return "AA_Elec_" + LoginUser.campus_id + "_" + dt.ToString("ddMMMyy") + "_" + DateTime.Now.ToString("HH") + "00" +".xml";
        }


        private string getCandidate(string selected)
        {
          
                for (int j = 0; j < candidate_obj.Count; j++)
                {
                    if (candidate_obj[j].Name == selected)
                    {
                        return candidate_obj[j].Rollno;

                    }
                }
            
            return " ";
        }
        private void WritetoXml(List<string> selected)
        {
            string[] positions = new string[] { "PRES", "VPRE", "GSEC" };
            string mypath = RecordsPath + GetFileFormat();

            XDocument doc;
         

            if (!File.Exists(mypath))
            {
                doc = new XDocument(new XElement("Votes"));
                doc.Save(mypath);

                Console.WriteLine("File didn't exist and now created successfully");
            }

            
            doc = XDocument.Load(mypath);
            for (int i = 0; i < 3; i++)
            {               
                doc.Element("Votes").Add(new XElement("Vote",
                                            new XElement("Nic", new XText(Voter.Nic)),
                                            new XElement("Position", new XText(positions[i])),
                                            new XElement("CandidateID", new XText(getCandidate(selected[i])))
                                            ));
            }
            doc.Save(mypath);

        }

        private void UpdatingVotersVoted()
        {
            
            using (StreamWriter sw = File.AppendText(VotersVotedPath))
            {
                sw.WriteLine(Voter.Nic);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text) || string.IsNullOrEmpty(comboBox3.Text))
            {
                MessageBox.Show("Please Select all 3 positions");
            }
            else if(comboBox1.Text == comboBox2.Text || comboBox2.Text == comboBox3.Text || comboBox1.Text == comboBox3.Text)
            {
                MessageBox.Show("One candidate cannot have multiple positions");
            }
            else
            {
                var selected = new List<string>();
                selected.Add(comboBox1.Text);
                selected.Add(comboBox2.Text);
                selected.Add(comboBox3.Text);
                WritetoXml(selected);
                UpdatingVotersVoted();
                this.Close();
                MessageBox.Show("Voted Successfully!");
                
            }
        }
    }
}


