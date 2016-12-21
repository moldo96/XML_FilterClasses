using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLFilter_Classes
{
    //TODO make all members private and set them using the constructor
    public class ModuleCoverageInfo
    {
        public string ModuleName;
        public int noOfBlocksNotCovered;
        public int noOfBlocksCovered;
        public XmlNode nodeOfModule;
    }
}
