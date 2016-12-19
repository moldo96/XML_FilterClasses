using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFilter_Classes
{
    class Process
    {
        static void Main(string[] args)
        {
            XmlFilteringReader readDocument = new XmlFilteringReader(new Filter());
            View view = new View();
            string xmlPathName = view.ReadXmlPath();
            readDocument.ComputeCoverage(xmlPathName, view.GetFileMaskToIgnore());
            Console.ReadKey();
        }
    }
}
