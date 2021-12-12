using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePoster.Utilities
{
    class LogWriter {

        private static string LOG_FILE = "CinemaPoster.log";
        private static string LOG_DIR = System.IO.Directory.GetCurrentDirectory() + @"\logs\";

        public LogWriter()
        {

        }


        public static void WriteLog(string message, string error)
        {

            try
            {
                File.AppendAllText(LOG_DIR + LOG_FILE, String.Format("{0}: {1}\n {2}", DateTime.Now, message, error));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
