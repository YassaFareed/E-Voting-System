using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace k180211_Q2
{

 
    public partial class Form2 : Form
    {
        static string VotersVotedPath = ConfigurationManager.AppSettings["VotersVotedPath"];
        static string VotersListPath = ConfigurationManager.AppSettings["VotersListPath"];
        
        public Form2()
        {
            InitializeComponent();
        }

     

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private bool verify_voter(string Name,string Rollno, string Nic)
        {
            try
            {
                using (StreamReader reader = new StreamReader(VotersListPath))
                {
                    string line;
           
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] splitted = line.Split(',');
                        var rollno = splitted[0];
                        var name = splitted[1];                  
                        var nic = splitted[2];

                        

                        if (Rollno == rollno.Trim() && Nic == nic.Trim())
                        {
                            
                            return true;
                        }

                    }

                }

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return false;
            }
            return false;
        }

        private bool CheckingVoterVoted(string nic)
        {
          
            
            if (File.Exists(VotersVotedPath))
            {
                using (StreamReader reader = new StreamReader(VotersVotedPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == nic)
                        {
                            MessageBox.Show("The Voter has already Voted!");
                            return false;
                        }
                     
                    }
                    return true;
               }
            }
            else
            {
                return true;
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name =  textBox1.Text.Trim();
            string rollno = textBox2.Text.Trim();
            string nic = textBox3.Text.Trim();

            if (name == "" || rollno =="" || nic == "")
            {
                MessageBox.Show("Please Fill All Fields!");
            }

            else if (ValidateInputs(name, rollno, nic))
            {
                if (verify_voter(name, rollno, nic))
                {
                    if (CheckingVoterVoted(nic))
                    {
                        Voter.Name = name;
                        Voter.Rollno = rollno;
                        Voter.Nic = nic;

                        Form3 votingForm = new Form3();
                        votingForm.ShowDialog();
                        this.Hide(); this.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Wrong details provided");
                }
            }



        }
        static bool Validatenicnumber(string nic)
        {
            Regex checknic = new Regex(@"^[0-9]{5}-[0-9]{7}-[0-9]{1}$");
            bool isvalid;
            isvalid = checknic.IsMatch(nic);
            if (isvalid)
            {
                return isvalid;
            }
            else
            {
                MessageBox.Show("Please enter valid nic number");
                return isvalid;
            }
        }

        static bool Validaterollno(string rollno)
        {
            Regex checkrn = new Regex(@"^k[0-9]{6}$");
            bool isvalid;
            isvalid = checkrn.IsMatch(rollno);
            if (isvalid)
            {
                return isvalid;
            }
            else
            {
                MessageBox.Show("Please enter valid roll number");
                return isvalid;
            }

        }

        static bool Validatename(string name) //to be updated for single name as well
        {
            Regex checkname = new Regex(@"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)$");
            Regex checkname2 = new Regex(@"^([a-zA-Z]{2,})");
            bool isvalid, isvalid2;
            isvalid = checkname.IsMatch(name);
            isvalid2 = checkname2.IsMatch(name);
            if (isvalid)
            {
                return isvalid;
            }
            else if (isvalid2)
            {
                return isvalid2;
            }
            else
            {
                MessageBox.Show("Please enter valid name");
                return isvalid;
            }

        }
        static bool ValidateInputs(string name, string rollno, string nic)
        {
            if(Validatenicnumber(nic) && Validaterollno(rollno) && Validatename(name))
            {
                return true;
            }
            return false;
        }
    }
}
