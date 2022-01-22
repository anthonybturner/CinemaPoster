using CinemaPosterApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPoster.Utilities
{
    class FileNameParser
    {

        /**
         * Creates Xml Filename with the xml directory
         */
        public static string CreateXmlDirectory(string title)
        {
            var directory = CinemaForm.XMLDirectory;
            var XmlFileName = title.Replace(": ", "_").Replace("/", "_");
            XmlFileName = XmlFileName.Replace(" ", "_") + ".xml";
            var fileLocation = directory + XmlFileName;
            return fileLocation;
        }

        public static string CreateImageDirectory(string title)
        {
            var directory = CinemaForm.ImageDirectory;
            var XmlFileName = title.Replace(": ", "_").Replace("/", "_");
            XmlFileName = XmlFileName.Replace(" ", "_") + ".jpg";
            var fileLocation = directory + XmlFileName;

            return fileLocation;
        }
    }
}
