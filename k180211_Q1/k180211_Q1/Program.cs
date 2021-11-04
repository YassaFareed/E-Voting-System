using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace k180211_Q1    
{
    class Program
    {
        static string CredentialListPath = ConfigurationManager.AppSettings["CredentialListPath"];
        static string campusID1 = ConfigurationManager.AppSettings["Campus1"];
        static string campusID2 = ConfigurationManager.AppSettings["Campus2"];
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Please provide 3 arguments");
            }
            else
            {
                string email, password, campus_id, encoded_password;
                email = args[0];
                password = args[1];
                campus_id = args[2];
                RecordsInfo record = new RecordsInfo(email,password,campus_id);
                if (Validate_inputs(record.Email, record.Campus_id))
                {
                    encoded_password = ConvertToBase64(record.Password);
                    WriteInFile(record.Email, encoded_password, record.Campus_id);
                }
            }
        }

        static bool IsPreviouslyExisted(List<string> records, string email)
        {
            List<string> previous_emails = new List<string>();
            foreach (string line in records)
            {
                String[] splitted = line.Split(',');
                var x = splitted[0];
                previous_emails.Add(x);
            }
            bool status = true;
            
            foreach (var l in previous_emails) { 
                
       
                if (l == email)
                {
                    status = false;
                }
       
            }
            if(status)
            {
                  return status; //returns true if not existed before
            }
            else
            {
                Trace.Assert(status, "Email Previously Existed");
                return status;
            }
         
        }
        static bool Validate_inputs(string email, string campus_id)
        {

            if (File.Exists(CredentialListPath)) //If file doesn't exist then don't need to check anything b/c it would be created in next step
            {

                List<string> lines = File.ReadAllLines(CredentialListPath).ToList();
                if (IsValidEmail(email) && IsPreviouslyExisted(lines, email) && IsCorrectCampus(campus_id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
      

        }
        static bool IsCorrectCampus(string campus_id)
        {
            if(campus_id == campusID1 || campus_id == campusID2)
            {
                return true;
            }
            else
            {
                Trace.Assert(false, "Campus id can be 210001 or 210002");
                return false;
            }
        }
        static void WriteInFile(string email, string encoded_password, string campus_id)
        {
        
            using (StreamWriter sw = File.AppendText(CredentialListPath))
            {

                string str = email + ',' + encoded_password + ',' + campus_id;
                sw.WriteLine(str);
            }

        }
        static string ConvertToBase64(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            string encodedText = Convert.ToBase64String(passwordBytes);
            return encodedText;
        }

        static bool IsValidEmail(string email)
        {
            Regex check = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            bool is_valid;
            is_valid = check.IsMatch(email);
            if(is_valid)
            {
                Console.WriteLine(is_valid);
                return is_valid;
               
            }
            else
            {
                Console.WriteLine(is_valid);
                Trace.Assert(is_valid, "Please follow email format xxx@xxx.com");
                return is_valid;

            }
          
        }
       

    }
}
