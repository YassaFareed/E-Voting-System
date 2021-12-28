using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace k180211_Q3
{
    class Program
    {
        static string RecordsPath = ConfigurationManager.AppSettings["RecordsPath"];
        static string Q3OutputPath = ConfigurationManager.AppSettings["Q3_output"];
      
        static string ExtractFileName(string file)
        {
            string[] words = file.Split('\\');

            string x = words[words.Length - 1];
            string[] y = x.Split('_');

            string newstring = "";
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    newstring += y[i];
                }
                else
                {
                    newstring += "_" + y[i];
                }

            }

            return newstring;
        }
        static void Main(string[] args)
        {
            try
            {
                string[] files_k1 = Directory.GetFiles(RecordsPath, "AA_Elec_210001_*", System.IO.SearchOption.AllDirectories);

                string[] files_k2 = Directory.GetFiles(RecordsPath, "AA_Elec_210002_*", System.IO.SearchOption.AllDirectories);

                CombineCommonFiles(files_k1);
                CombineCommonFiles(files_k2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void CombineCommonFiles(string[] files)
        {
            string res = ExtractFileName(files[0]);

            if (files.Length != 1)
            {
                for (int i = 1; i < files.Length; i++)
                {
                    var xml1 = XDocument.Load(files[0]);
                    var xml2 = XDocument.Load(files[i]);
                    xml1.Descendants("Vote").LastOrDefault().AddAfterSelf(xml2.Descendants("Vote"));
                    xml1.Save(Q3OutputPath + "/" + res + ".xml");
                }
            }
            else
            {
                var xml1 = XDocument.Load(files[0]);
                xml1.Save(Q3OutputPath + "/" + res + ".xml");
            }

        }
    }
}
