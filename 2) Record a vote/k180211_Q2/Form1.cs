using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;


namespace k180211_Q2
{
    public partial class Form1 : Form
    {
        readonly string CredentialListPath = ConfigurationManager.AppSettings["CredentialListPath"];
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string Base64StringDecode(string encodedString)
        {
            var bytes = Convert.FromBase64String(encodedString);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }
     
        private bool verify_credentials()
        {
            try
            {
                // Create a StreamReader  
                using (StreamReader reader = new StreamReader(CredentialListPath))
                {
                    string line;
                    // Read line by line  
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] splitted = line.Split(',');
                        var pass = splitted[1];
                        var email = splitted[0];
                       
                        string decoded = Base64StringDecode(pass);
                
                        if (LoginUser.Password == decoded && LoginUser.Email == email)
                        {
                            var campus = splitted[2];
                            LoginUser.campus_id = campus;                 
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
      

        private void button1_Click(object sender, EventArgs e)
        {

            LoginUser.Email = textBox1.Text;
            LoginUser.Password = textBox2.Text;

              if (verify_credentials()) 
              {
                  Form2 secondForm = new Form2();
                  secondForm.ShowDialog();
                  this.Hide(); this.Close();

             }
             else
             {
                 MessageBox.Show("Incorrect Username or Password !!!");
              }
        }
    }
}
