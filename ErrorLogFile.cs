using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    class ErrorLogFile
    {
        private string logFormat = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "\t";
        private string path = @"log/" + DateTime.Now.ToShortDateString() + ".txt";

        bool loggingEnabled;

        public ErrorLogFile()
        {
            //Checks if the log directory exists
            if (!Directory.Exists(@"log/"))
            {
                System.IO.Directory.CreateDirectory(@"log/");
            }

            recreateConfig(configValidationCheck());

            //Checks the enable entry of the configfile & sets loggingEnabled
            using (StreamReader sr = new StreamReader(@"log/config.txt"))
            {
                string line = sr.ReadToEnd();
                string subStr = line.Substring(line.IndexOf('=') + 1).Trim();

                loggingEnabled = Convert.ToBoolean(subStr);
            }
        }


        //Checks the existence and the validity of the config
        private bool configValidationCheck()
        {
            //Check if the configfile exists.
            if (!File.Exists(@"log/config.txt"))
            {
                return true;
            }

            //Check if the configfile has the right format
            using (StreamReader sr = new StreamReader(@"log/config.txt"))
            {
                string line = sr.ReadLine();

                if (line.Equals("enabled = false") || line.Equals("enabled = true"))
                {
                    return false;
                }
            }

            return true;
        }
        
        //Recreates the config when true
        private void recreateConfig(bool recreate)
        {
            if (recreate)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"log/config.txt"))
                {
                    file.WriteLine("enabled = false");
                }
            }
        }

        //Writes Error errMsg into the LogFile
        public void errorLog(string errMsg)
        {
            if(!loggingEnabled)
            {
                return;
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(logFormat + errMsg);
            }
        }
    }
}
